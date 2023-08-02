using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;

public class BattlePracticeManager : MonoBehaviour
{
    public int AttackInfoRole = 0;
    public int AttackedInfoRole = 0;

    [SerializeField] string[] roleText = new string[5];
    [SerializeField] TextMeshProUGUI _attackText;
    [SerializeField] TextMeshProUGUI _attackedText;

    [SerializeField] GameObject[] AttackChara = new GameObject[5];
    [SerializeField] GameObject[] AttackedChara = new GameObject[5];

    [SerializeField] GameObject AllObjects;
    private GameObject Player;
    private OVRPlayerController ovrPlayerController;
    private OVRGrabber[] _grabber;
    private CharacterController _controller;
    [SerializeField] OVRScreenFade _fade;
    [SerializeField] GameObject AttackResults;
    private Vector3 beforePosition;
    private bool isFirst = true;
    private bool isGameStart = false;

    [SerializeField] GameObject[] ChangeBoardSizeButtonRight = new GameObject[2];

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        ovrPlayerController = Player.GetComponent<OVRPlayerController>();
        _grabber = Player.GetComponentsInChildren<OVRGrabber>();
        _controller = Player.GetComponent<CharacterController>();
        SetText();
    }

    //LanguageでTextを変える
    public void SetText()
    {
        for (int i = 0; i < roleText.Length; i++)
            roleText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "Role" + (i + 1).ToString());
        _attackText.text = roleText[AttackInfoRole];
        _attackedText.text = roleText[AttackedInfoRole];
    }

    //キャラを変更するボタン
    public void ChangeAttackRole(bool isPlus)
    {
        if (isGameStart) return;
        AudioManager.Instance.SE(0);
        if (isPlus)
        {
            if (AttackInfoRole != 4)
                AttackInfoRole++;
        }
        else
        {
            if (AttackInfoRole != 0)
                AttackInfoRole--;
        }
        if (AttackInfoRole <= -1|| AttackInfoRole >= 5) return;
        for(int i = 0; i < 5; i++)
            AttackChara[i].SetActive(false);
        AttackChara[AttackInfoRole].SetActive(true);
        _attackText.text = roleText[AttackInfoRole];
    }

    public void ChangeAttackedRole(bool isPlus)
    {
        if (isGameStart) return;
        AudioManager.Instance.SE(0);
        if (isPlus)
        {
            if (AttackedInfoRole != 4)
                AttackedInfoRole++;
        }
        else
        {
            if (AttackedInfoRole != 0)
                AttackedInfoRole--;
        }
        if (AttackedInfoRole <= -1 || AttackedInfoRole >= 5) return;
        for (int i = 0; i < 5; i++)
            AttackedChara[i].SetActive(false);
        AttackedChara[AttackedInfoRole].SetActive(true);
        _attackedText.text = roleText[AttackedInfoRole];
    }

    //戦闘シーンへ行く
    public void GoAttackScene()
    {
        if (isGameStart) return;
        StartCoroutine(AdditiveScene());
    }

    private IEnumerator AdditiveScene()
    {
        AudioManager.Instance.SE(3);
        AudioManager.Instance.StopBGM(1);
        isGameStart = true;
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
        _fade.FadeIn();
        AllObjects.SetActive(false);
        yield break;
    }

    private void OnEnable()
    {
        StartCoroutine(ResultAttackScene());
    }

    private IEnumerator ResultAttackScene()
    {
        if (isFirst)
        {
            isFirst = false;
            yield break;
        }
        
        for (int i = 0; i < _grabber.Length; i++)
        {
            if (_grabber[i].grabbedObject != null)
                _grabber[i].ForceRelease(_grabber[i].grabbedObject);
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
        isGameStart = false;
        ChangeBoardSizeButtonRight[0].SetActive(true);
        ChangeBoardSizeButtonRight[1].SetActive(true);
        AudioManager.Instance.SetBGM(0);
        yield break;
    }
}
