using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEngine.Localization.Settings;

public class AttackManager : MonoBehaviour
{
    private bool isHighStakesBattle = false;

    private int AttackRole = 0;
    private int AttackedRole = 0;

    private int AttributeAdvantage;
    private float Multiplier = 0f;

    [SerializeField] Transform EnemyGeneratePos;
    public GameObject[] AttackedChara = new GameObject[5];
    public GameObject[] Weapons = new GameObject[5];
    private GameObject Weapon;
    public GameObject[] MagicObjects = new GameObject[5];

    private GrabWeapon[] _grabWeapon = new GrabWeapon[2];
    private GunManager[] _gunManager = new GunManager[2];

    [SerializeField] GameObject BlockCube;
    [SerializeField] GameObject PedestalCube;
    [SerializeField] GameObject DirectionalArrow;

    [TextArea][SerializeField]
    string[] ALLExplanationText;
    [SerializeField] TextMeshProUGUI _explanationText;
    [SerializeField] GameObject Explanations;

    private int BattleMode = 0;
    [SerializeField] GameObject ModeChangeButtons;
    [SerializeField] Button[] ModeButton = new Button[3];
    [SerializeField] Sprite[] ModeButtonSprite = new Sprite[2];

    [SerializeField] TextMeshProUGUI _timerText;
    private float time = 30f;
    public bool isPauseTimer = false;//HourGlassでのみ使用

    [SerializeField] MeshRenderer TimerEffectRenderer;
    private bool once = true;

    private GameObject Results;
    private TextMeshProUGUI _resultText;
    [SerializeField] string[] ALLResultText;
    private StringBuilder sb = new StringBuilder();
    private GameObject HighStakesText;

    private OVRScreenFade _fade;
    private BattleSceneManagement _manager;
    private GameInfomation _gameInfomation;
    private AttackInfomation _info;
    private InAttackSettingManager _settingManager;
    private OVRGrabber[] _grabber;

    private void Awake()
    {
        Application.targetFrameRate = 72;
        if (GameObject.FindGameObjectWithTag("GameController") != null)
            _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        if (_gameInfomation != null)
            isHighStakesBattle = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().isHighStakesBattle;
        _fade = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<OVRScreenFade>();
        if (GameObject.FindGameObjectWithTag("GameParent") != null)
            _manager = GameObject.FindGameObjectWithTag("GameParent").GetComponent<BattleSceneManagement>();
        _info = GetComponent<AttackInfomation>();
        _settingManager = GetComponent<InAttackSettingManager>();
        _grabber = GameObject.FindGameObjectWithTag("Player").GetComponentsInChildren<OVRGrabber>();
        DirectionalArrow.SetActive(false);
        Results = GameObject.FindGameObjectWithTag("AttackResult");
        Results.SetActive(false);
        _resultText = Results.GetComponentInChildren<TextMeshProUGUI>();
        HighStakesText = Results.transform.GetChild(2).gameObject;
        HighStakesText.SetActive(false);
        ModeChangeButtons.SetActive(false);
    }

    private void Start()
    {
        TimerEffectRenderer.enabled = false;
        StartCoroutine(Set());
        Invoke("ActiveArrow", 10f);

        AudioManager.Instance.StopAllSounds();
        AudioManager.Instance.SetBGM(3);
    }

    private IEnumerator Set()
    {
        yield return new WaitForSeconds(0.1f);
        //言語で説明文を変更する
        for (int i = 0; i < ALLExplanationText.Length; i++)
            ALLExplanationText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "BattleExplanationText" + (i + 1).ToString());
        for (int i = 0; i < ALLResultText.Length; i++)
            ALLResultText[i] = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "BattleResultText" + (i + 1).ToString());
        AttackRole = _info.AttackPiece.Role;
        AttackedRole = _info.AttackedPiece.Role;
        yield return null;
        //指揮官以外のModeパターン
        for (int i = 0; i < 2; i++)
            ModeButton[i].gameObject.transform.localPosition = new Vector3(-0.1875f + (i * 0.375f), 1.1f, 1.0999f);
        ModeButton[0].GetComponent<Image>().sprite = ModeButtonSprite[0];
        ModeButton[1].GetComponent<Image>().sprite = ModeButtonSprite[1];
        //課金してるならMode2解放
        if (_gameInfomation.isPay)
        {
            ModeButton[1].GetComponent<Image>().sprite = null;
            ModeButton[1].enabled = true;
        }
        else
        {
            ModeButton[1].GetComponent<Image>().sprite = ModeButtonSprite[1];
            ModeButton[1].enabled = false;
        }
        ModeButton[2].gameObject.SetActive(false);
        switch (AttackRole)
        {
            case 0:
                gameObject.AddComponent<InfantryAttack>();
                Weapons[0].SetActive(true);
                break;
            case 1:
                gameObject.AddComponent<KnightAttack>();
                Weapons[0].SetActive(true);
                Weapons[1].SetActive(true);
                break;
            case 2:
                gameObject.AddComponent<MagicianAttack>();
                Weapons[2].SetActive(true);
                break;
            case 3:
                gameObject.AddComponent<AssaulterAttack>();
                Weapons[3].SetActive(true);
                break;
            case 4:
                gameObject.AddComponent<CommanderAttack>();
                Weapons[4].SetActive(true);
                yield return null;
                _gunManager = null;
                _gunManager = GetComponentsInChildren<GunManager>();
                ModeButton[2].gameObject.SetActive(true);
                for (int i = 0; i < 3; i++)
                    ModeButton[i].gameObject.transform.localPosition = new Vector3(-0.375f + (i * 0.375f), 1.1f, 1.0999f);
                ModeButton[0].GetComponent<Image>().sprite = ModeButtonSprite[0];
                ModeButton[1].GetComponent<Image>().sprite = null;
                ModeButton[1].enabled = true;
                //課金してるならMode3解放
                if (_gameInfomation.isPay)
                {
                    ModeButton[2].enabled = true;
                    ModeButton[2].GetComponent<Image>().sprite = null;
                }
                else
                {
                    ModeButton[2].enabled = false;
                    ModeButton[2].GetComponent<Image>().sprite = ModeButtonSprite[1];
                }
                break;
        }
        yield return null;
        ModeChangeButtons.SetActive(true);
        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
        yield return null;
        if (AttackRole != 2)
        {
            Instantiate(AttackedChara[AttackedRole], EnemyGeneratePos);
            _timerText.text = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "RemaingTime");
        }
        else
        {
            Vector3 pos = new Vector3(-4, 0, 0);
            var g = Instantiate(AttackedChara[AttackedRole], Vector3.zero, Quaternion.Euler(0, 90, 0), EnemyGeneratePos);
            g.transform.localPosition = pos;
            _timerText.text = "";
        }
        yield return null;
        switch (_info.AttackPiece.Attribute)
        {
            case 0:
                if (_info.AttackedPiece.Attribute == 0 || _info.AttackedPiece.Attribute == 3)
                    AttributeAdvantage = 1;
                else if (_info.AttackedPiece.Attribute == 1)
                    AttributeAdvantage = 0;
                else
                    AttributeAdvantage = 2;
                break;
            case 1:
                if (_info.AttackedPiece.Attribute == 0)
                    AttributeAdvantage = 2;
                else if (_info.AttackedPiece.Attribute == 1 || _info.AttackedPiece.Attribute == 3)
                    AttributeAdvantage = 1;
                else
                    AttributeAdvantage = 0;
                break;
            case 2:
                if (_info.AttackedPiece.Attribute == 0)
                    AttributeAdvantage = 0;
                else if (_info.AttackedPiece.Attribute == 1)
                    AttributeAdvantage = 2;
                else
                    AttributeAdvantage = 1;
                break;
            case 3:
                AttributeAdvantage = 1;
                break;
        }
        float Defence = _info.AttackedPiece.Defense;
        if (_info.AttackPiece.Role == 2)
            Defence = _info.AttackedPiece.MagicDefense;
        Multiplier = _info.AttackPiece.Attack / Defence;
        if (PlayerPrefs.GetInt("isShowExplanation", 1) != 0)
            _explanationText.text = ALLExplanationText[AttackRole];
        else
            Explanations.SetActive(false);
    }

    private void ActiveArrow()
    {
        if (!PedestalCube.activeSelf) return;
        DirectionalArrow.SetActive(true);
    }

    private void Update()
    {
        if (BlockCube.activeSelf || AttackRole == 2 || isPauseTimer) return;
        time -= Time.deltaTime;
        if (time < 10 && time > 9 && once)
        {
            once = false;
            StartCoroutine(TimerEffect(false));
        }
        if (time < 5 && time > 4 && !once)
        {
            once = true;
            StartCoroutine(TimerEffect(true));
        }

        if (time < 0 && time > -1 && once)
        {
            once = false;
            _timerText.text = "00.00";
            if (AttackRole == 4 && BattleMode == 1)
            {
                _gunManager[0].FinishTrajectoryAttack();
                _gunManager[1].FinishTrajectoryAttack();
                AudioManager.Instance.SE(9);
                return;
            }
            ShowResult();
        }

        if(time < -2 && time > -3 && !once)
        {
            once = true;
            if (AttackRole == 4 && BattleMode == 1)
                ShowResult();
        }

        if (time < 0) return;
        _timerText.text = time.ToString("00.00");
    }

    int grabNum = 0;
    public void StartGame()
    {
        ModeChangeButtons.SetActive(false);
        switch (AttackRole)
        {
            case 0:
                break;
            case 1:
                grabNum++;
                break;
            case 2:
                if (BattleMode == 1)
                    grabNum++;
                break;
            case 3:
                grabNum++;
                if (BattleMode == 0)
                    GetComponent<AssaulterAttack>().SetModeDecide(false);
                else
                    GetComponent<AssaulterAttack>().SetModeDecide(true);
                break;
            case 4:
                grabNum++;
                GetComponent<CommanderAttack>().SetModeDecide(BattleMode);
                break;
        }
        if (grabNum == 1) return;

        _settingManager.InActiveButton();

        if (DirectionalArrow.activeSelf)
            DirectionalArrow.SetActive(false);
        _timerText.color = new Color(1, 1, 1, 1);
        PedestalCube.transform.DOMoveY(-1f, 1.4f).SetRelative(true).SetDelay(0.1f)
            .OnComplete(() => {
                PedestalCube.SetActive(false);
                if(DirectionalArrow.activeSelf)
                    DirectionalArrow.SetActive(false);
            });
        BlockCube.transform.DOScale(new Vector3(5f, 5f, 5f), 1.5f)
            .OnComplete(() => {
                switch (AttackRole)
                {
                    case 0:
                        if (BattleMode == 0)
                        {
                            GetComponentInChildren<AttackedCharaMove>().SetSphere(0, AttackedRole);
                            GetComponent<InfantryAttack>().SetModeDecide(false);
                        }
                        else
                        {
                            GetComponentInChildren<AttackedCharaMove>().SetSphere(1, AttackedRole);
                            GetComponent<InfantryAttack>().SetModeDecide(true);
                        }
                        break;
                    case 1:
                        if (BattleMode == 0)
                        {
                            GetComponentInChildren<AttackedCharaMove>().SetSphere(0, AttackedRole);
                            GetComponent<KnightAttack>().Start(false);
                        }
                        else
                        {
                            GetComponentInChildren<AttackedCharaMove>().SetSphere(1, AttackedRole);
                            GetComponent<KnightAttack>().Start(true);
                        }
                        break;
                    case 2:
                        var circle = Instantiate(MagicObjects[0], transform);
                        var obj = Instantiate(MagicObjects[1], transform);
                        Instantiate(MagicObjects[2], transform);
                        var guide = Instantiate(MagicObjects[3], transform);
                        GetComponent<MagicianAttack>().SetMagicCircle(circle, obj, guide, MagicObjects[4]);
                        break;
                    case 3:
                        GetComponentInChildren<AttackedCharaMove>().SetSphere(2, AttackedRole);
                        GetComponent<AssaulterAttack>().isStart = true;
                        break;
                    case 4:
                        GetComponent<CommanderAttack>().isStart = true;
                        if (BattleMode == 0)
                        {
                            GetComponentInChildren<AttackedCharaMove>().SetSphere(3, AttackedRole);
                        }
                        else if (BattleMode == 1)
                        {
                            GetComponentInChildren<AttackedCharaMove>().SetSphere(4, AttackedRole);
                            _gunManager[0].BattleMode = true;
                            _gunManager[1].BattleMode = true;
                        }
                        else
                        {
                            GetComponentInChildren<AttackedCharaMove>().SetSphere(5, AttackedRole);
                        }
                        break;
                }
                GetComponentInChildren<AttackAnimation>().StartAnimation(AttackRole);
                BlockCube.SetActive(false);
            });
        Explanations.SetActive(false);
        AudioManager.Instance.SE(19);
        if (AttackRole != 2)
            AudioManager.Instance.SetBGM(8);
        else
            AudioManager.Instance.SetBGM(8, true);
    }

    private IEnumerator TimerEffect(bool type)
    {
        Material mat = TimerEffectRenderer.material;
        if (!type)
            mat.color = new Color(1, 1, 0, 0);
        else
            mat.color = new Color(1, 0, 0, 0);
        TimerEffectRenderer.enabled = true;
        Color col = mat.color;
        for (int i = 0; i < 30; i++)
        {
            col.a += 1 / 30f;
            mat.color = col;
            yield return null;
        }
        for (int i = 0; i < 30; i++)
        {
            col.a -= 1 / 30f;
            mat.color = col;
            yield return null;
        }
        TimerEffectRenderer.enabled = false;
        yield break;
    }

    //魔法攻撃の時のみMagicianAttackから呼ばれる
    public void ShowResult()
    {
        if (Results.activeSelf) return;
        Results.SetActive(true);
        float score = GetComponentInChildren<AttackGauge>().gaugeValue;
        float damage = 0f;
        string scoreStr;
        if (score > 145)
            scoreStr = "<color=yellow>X";
        else if (score > 125)
            scoreStr = "<color=yellow>S+";
        else if (score > 100)
            scoreStr = "<color=red>S";
        else if (score > 90)
            scoreStr = "<color=red>S-";
        else if (score > 70)
            scoreStr = "<color=blue>A+";
        else if (score > 60)
            scoreStr = "<color=blue>A";
        else if (score > 50)
            scoreStr = "<color=blue>A-";
        else if (score > 40)
            scoreStr = "<color=#8C8C8C>B+";
        else if (score > 30)
            scoreStr = "<color=#8C8C8C>B";
        else if (score > 20)
            scoreStr = "<color=#8C8C8C>B-";
        else
            scoreStr = "<color=#a52a2a>C";

        switch (AttributeAdvantage)
        {
            case 0:
                if (score > 100)
                    damage = 35 + ((score - 100) / 5f);
                else
                    damage = 10 + (score / 4f);
                break;
            case 1:
                if (score > 100)
                    damage = 55 + ((score - 100) / 5f);
                else if (score > 50)
                    damage = 42.75f + (score / 4f);
                else
                    damage = 25 + (score / 4f);
                break;
            case 2:
                if (score > 100)
                    damage = 75 + ((score - 100) / 5f);
                else if (score > 50)
                    damage = 62.75f + (score / 4f);
                else
                    damage = 45 + (score / 4f);
                break;
        }
        damage *= Multiplier;
        damage = (int)damage;

        if (isHighStakesBattle)
        {
            if (score > 100)
            {
                sb.Append(ALLResultText[0] + scoreStr + "</color>\n" + ALLResultText[1] + damage.ToString());
                damage *= 1.25f;
                damage = (int)damage;
                HighStakesText.SetActive(true);
            }
            else
            {
                damage = 0;
                sb.Append(ALLResultText[0] + scoreStr + " </color>\n" + ALLResultText[1] + damage.ToString());
            }
        }
        else
            sb.Append(ALLResultText[0] + scoreStr + "</color>\n" + ALLResultText[1] + damage.ToString());

        if (!_manager.isPractice)
            _manager.damage = damage;

        _resultText.text = sb.ToString();
        StartCoroutine(ReturnScene());
    }

    private IEnumerator ReturnScene()
    {
        AudioManager.Instance.SetBGM(4);
        GetComponentInChildren<AttackedCharaMove>().Finish();
        GetComponentInChildren<AttackAnimation>().Finish();
        GetComponentInChildren<AttackGauge>().isFinishGame = true;
        yield return new WaitForSeconds(2f);
        _info.ReleaseReference();
        sb.Append("\n" + ALLResultText[2] + "3");
        _resultText.text = sb.ToString();
        yield return new WaitForSeconds(1f);
        sb.Remove(sb.Length - 1, 1);
        sb.Append("2");
        _resultText.text = sb.ToString();
        yield return new WaitForSeconds(1f);
        sb.Remove(sb.Length - 1, 1);
        sb.Append("1");
        _resultText.text = sb.ToString();
        yield return new WaitForSeconds(0.5f);
        AudioManager.Instance.StopBGM(0);
        switch (AttackRole)
        {
            case 0:
                _grabWeapon[0].FinishAttack();
                break;
            case 2:
                _grabWeapon[0].FinishAttack();
                if (BattleMode == 1)
                    _grabWeapon[1].FinishAttack();
                break;
            case 1:
            case 3:
                _grabWeapon[0].FinishAttack();
                _grabWeapon[1].FinishAttack();
                break;
            case 4:
                if (BattleMode == 1)
                    _gunManager[1].FinishAttack();
                else
                    _grabWeapon[0].FinishAttack();
                _gunManager[0].FinishAttack();
                break;
        }
        _fade.FadeOut();
        yield return new WaitForSeconds(0.5f);
        sb.Remove(sb.Length - 1, 1);
        sb.Append("0");
        _resultText.text = sb.ToString();
        yield return new WaitForSeconds(1f);
        _manager.gameObject.SetActive(true);
        _manager = null;
        _fade = null;
        yield break;
    }

    public void ChangeMode(int num)
    {
        if (num == 1 && !_gameInfomation.isPay && AttackRole != 4) return;
        if (num == 2 && !_gameInfomation.isPay) return;
        if (ModeButton[num].GetComponent<Image>().sprite == ModeButtonSprite[0])
            return;
        else
            ModeButton[num].GetComponent<Image>().sprite = ModeButtonSprite[0];
        BattleMode = num;
        AudioManager.Instance.SE(0);
        for (int i = 0; i < _grabber.Length; i++)
            _grabber[i].WeaponForceRelease();
        switch (num)
        {
            case 0:
                ModeButton[1].GetComponent<Image>().sprite = null;
                //解放してるかしてないかで別れる
                if (_gameInfomation.isPay)
                {
                    ModeButton[2].GetComponent<Image>().sprite = null;
                }
                else
                {
                    ModeButton[2].GetComponent<Image>().sprite = ModeButtonSprite[1];
                }
                switch (AttackRole)
                {
                    case 0:
                        _explanationText.text = ALLExplanationText[0];
                        break;
                    case 1:
                        _explanationText.text = ALLExplanationText[1];
                        Weapons[0].SetActive(true);
                        Weapons[7].SetActive(false);
                        _grabWeapon = null;
                        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
                        break;
                    case 2:
                        _explanationText.text = ALLExplanationText[2];
                        Weapons[8].SetActive(false);
                        _grabWeapon = null;
                        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
                        break;
                    case 3:
                        _explanationText.text = ALLExplanationText[3];
                        Weapons[6].SetActive(false);
                        Weapons[3].SetActive(true);
                        _grabWeapon = null;
                        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
                        break;
                    case 4:
                        _explanationText.text = ALLExplanationText[4];
                        Weapons[5].SetActive(false);
                        Weapons[4].SetActive(true);
                        Weapons[9].SetActive(false);
                        _grabWeapon = null;
                        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
                        _gunManager = null;
                        _gunManager = GetComponentsInChildren<GunManager>();
                        break;
                }
                break;
            case 1:
                ModeButton[0].GetComponent<Image>().sprite = null;
                if (_gameInfomation.isPay)
                    ModeButton[2].GetComponent<Image>().sprite = null;
                else
                    ModeButton[2].GetComponent<Image>().sprite = ModeButtonSprite[1];
                switch (AttackRole)
                {
                    case 0:
                        _explanationText.text = ALLExplanationText[8];
                        break;
                    case 1:
                        _explanationText.text = ALLExplanationText[9];
                        Weapons[0].SetActive(false);
                        Weapons[7].SetActive(true);
                        _grabWeapon = null;
                        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
                        break;
                    case 2:
                        _explanationText.text = ALLExplanationText[10];
                        Weapons[8].SetActive(true);
                        _grabWeapon = null;
                        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
                        break;
                    case 3:
                        _explanationText.text = ALLExplanationText[7];
                        Weapons[6].SetActive(true);
                        Weapons[3].SetActive(false);
                        _grabWeapon = null;
                        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
                        break;
                    case 4:
                        _explanationText.text = ALLExplanationText[5];
                        Weapons[5].SetActive(true);
                        Weapons[4].SetActive(false);
                        Weapons[9].SetActive(false);
                        _gunManager = null;
                        _gunManager = GetComponentsInChildren<GunManager>();
                        break;
                }
                break;
            case 2:
                ModeButton[0].GetComponent<Image>().sprite = null;
                ModeButton[1].GetComponent<Image>().sprite = null;
                switch (AttackRole)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        _explanationText.text = ALLExplanationText[6];
                        Weapons[5].SetActive(false);
                        Weapons[4].SetActive(false);
                        Weapons[9].SetActive(true);
                        _grabWeapon = null;
                        _grabWeapon = GetComponentsInChildren<GrabWeapon>();
                        _gunManager = null;
                        _gunManager = GetComponentsInChildren<GunManager>();
                        break;
                }
                break;
        }
    }
}
