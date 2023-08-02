using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using OculusSampleFramework;

public class BattleSceneManagement : MonoBehaviour
{
    public bool rejectSceneChange = false;
    public bool neverAsk = false;

    public float damage = 0f;

    public PieceInfomation AttackInfo { get; private set; }
    public PieceInfomation AttackedInfo { get; private set; }

    [SerializeField] GameObject AllObjects;
    private GameObject Player;
    private OVRPlayerController ovrPlayerController;
    private OVRGrabber[] _grabber;
    private CharacterController _controller;
    [SerializeField] OVRScreenFade _fade;
    [SerializeField] GameObject AttackResults;
    private Vector3 beforePosition;

    public bool isPractice { get; private set; } = true;
    private BattlePracticeManager _battlePracticeManager;
    [SerializeField] GameObject AskSceneChangeObject;
    private GameInfomation _gameInfomation;
    private TileManager _tileManager;
    private TurnManager _turnManager;

    [SerializeField] Image NeverAskCheckImage;
    [SerializeField] Sprite[] checkImage = new Sprite[2];

    void Start()
    {
        AskSceneChangeObject.SetActive(false);
        _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        Player = GameObject.FindGameObjectWithTag("Player");
        ovrPlayerController = Player.GetComponent<OVRPlayerController>();
        _grabber = Player.GetComponentsInChildren<OVRGrabber>();
        _controller = Player.GetComponent<CharacterController>();
        _battlePracticeManager = GetComponent<BattlePracticeManager>();
    }

    public void Set()
    {
        Destroy(_battlePracticeManager);
        _tileManager = GetComponentInChildren<TileManager>();
        _turnManager = GetComponentInChildren<TurnManager>();
        isPractice = false;
    }

    public void AskAttackScene(PieceInfomation _allyInfo, PieceInfomation _enemyInfo)
    {
        _turnManager.ActiveChangeButton(false);
        _gameInfomation.isShowAttackMessage = true;
        AttackInfo = _allyInfo;
        AttackedInfo = _enemyInfo;
        if(neverAsk)
            GoAttackScene();
        else
            AskSceneChangeObject.SetActive(true);
    }

    public void GoAttackScene()
    {
        StartCoroutine(AdditiveScene());
    }

    private IEnumerator AdditiveScene()
    {
        AudioManager.Instance.SE(0);
        AudioManager.Instance.StopBGM(1);
        AudioManager.Instance.SE(18);
        _tileManager.StopBlinking(2);
        AskSceneChangeObject.SetActive(false);
        _fade.FadeOut();
        yield return new WaitForSeconds(2f);
        AttackResults.SetActive(true);
        beforePosition = ovrPlayerController.gameObject.transform.position;
        ovrPlayerController.enabled = false;
        _controller.enabled = false;
        yield return new WaitForSeconds(0.15f);
        _controller.enabled = true;
        SceneManager.LoadScene("AttackScene", LoadSceneMode.Additive);
        yield return null;
        for (int i = 0; i < 5; i++)//プレイヤーの位置の上書きがされないタイミングがあるので対策
        {
            ovrPlayerController.gameObject.transform.position = new Vector3(0, 1, 0);
            yield return null;
        }
        yield return new WaitForSeconds(0.15f);
        ovrPlayerController.enabled = true;
        _gameInfomation.isShowAttackMessage = false;
        _fade.FadeIn();
        AllObjects.SetActive(false);
        yield break;
    }

    public void RemainScene()
    {
        StartCoroutine(Cancel());
    }

    private IEnumerator Cancel()
    {
        AudioManager.Instance.SE(1);
        rejectSceneChange = true;
        AskSceneChangeObject.SetActive(false);
        AttackInfo.ChangeCanAttack(true);
        yield return null;
        _tileManager.AttackPiece(AttackInfo, AttackedInfo);
        _gameInfomation.isShowAttackMessage = false;
        _turnManager.ActiveChangeButton(true);
        yield break;
    }

    public void NeverAsk()
    {
        AudioManager.Instance.SE(0);
        if (!neverAsk)
        {
            neverAsk = true;
            NeverAskCheckImage.sprite = checkImage[1];
        }
        else
        {
            neverAsk = false;
            NeverAskCheckImage.sprite = checkImage[0];
        }
    }

    private void OnEnable()
    {
        if (isPractice) return;
        StartCoroutine(ResultAttackScene());
    }

    private IEnumerator ResultAttackScene()
    {
        var attackInfo = AttackInfo;
        var attackedInfo = AttackedInfo;

        yield return null;
        for (int i = 0; i < _grabber.Length; i++)
        {
            _grabber[i].WeaponForceRelease();
        }

        yield return null;
        var op = SceneManager.UnloadSceneAsync("AttackScene");
        yield return op;

        _fade.FadeIn();
        ovrPlayerController.enabled = false;
        _controller.enabled = false;
        ovrPlayerController.transform.position = beforePosition;

        //不使用アセットをアンロードしてメモリを解放する
        yield return Resources.UnloadUnusedAssets();
        AttackResults.SetActive(false);
        ovrPlayerController.transform.position = beforePosition;
        _controller.enabled = true;
        ovrPlayerController.enabled = true;
        yield return new WaitForSeconds(2f);

        int rotateNum = -1;
        if (attackInfo.CurrentPosition.x < attackedInfo.CurrentPosition.x)
        {
            if (attackInfo.CurrentPosition.y > attackedInfo.CurrentPosition.y)
                rotateNum = 1;
            else if (attackInfo.CurrentPosition.y == attackedInfo.CurrentPosition.y)
                rotateNum = 2;
            else
                rotateNum = 3;
        }
        else if (attackInfo.CurrentPosition.x == attackedInfo.CurrentPosition.x)
        {
            if (attackInfo.CurrentPosition.y > attackedInfo.CurrentPosition.y)
                rotateNum = 0;
            else
                rotateNum = 4;
        }
        else
        {
            if (attackInfo.CurrentPosition.y > attackedInfo.CurrentPosition.y)
                rotateNum = 7;
            else if (attackInfo.CurrentPosition.y == attackedInfo.CurrentPosition.y)
                rotateNum = 6;
            else
                rotateNum = 5;
        }
        int AttributeAdvantage = 0;
        switch (attackInfo.Attribute)
        {
            case 0:
                if (attackedInfo.Attribute == 0 || attackedInfo.Attribute == 3)
                    AttributeAdvantage = 1;
                else if (attackedInfo.Attribute == 1)
                    AttributeAdvantage = 0;
                else
                    AttributeAdvantage = 2;
                break;
            case 1:
                if (attackedInfo.Attribute == 0)
                    AttributeAdvantage = 2;
                else if (attackedInfo.Attribute == 1 || attackedInfo.Attribute == 3)
                    AttributeAdvantage = 1;
                else
                    AttributeAdvantage = 0;
                break;
            case 2:
                if (attackedInfo.Attribute == 0)
                    AttributeAdvantage = 0;
                else if (attackedInfo.Attribute == 1)
                    AttributeAdvantage = 2;
                else
                    AttributeAdvantage = 1;
                break;
            case 3:
                AttributeAdvantage = 1;
                break;
        }
        attackedInfo.GetComponentInChildren<PieceAnimation>().FaceTarget(rotateNum);
        attackInfo.GetComponentInChildren<PieceAnimation>().Attacking(AttributeAdvantage, (int)damage, rotateNum, attackedInfo.GetComponentInChildren<PieceAnimation>());
        _tileManager.CheckDown(attackedInfo, (int)damage);
        _tileManager.CalculateCanAttackPieceCount();
        _turnManager.ActiveChangeButton(true);

        AudioManager.Instance.SetBGM(7);
        yield break;
    }
}
