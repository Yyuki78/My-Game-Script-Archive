using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemManager : MonoBehaviour
{
    [SerializeField] GameObject[] Item;

    [SerializeField] int CoinNum = 0;

    [SerializeField] bool canUseItem = true;
    [SerializeField] bool canGacha = true;
    public int FirstItem { private set; get; }
    public int SecondItem { private set; get; }

    private BoxCollider insCol;
    private int punchNum = -1;

    [SerializeField] Material[] carTransparentMat;

    private bool isCoinGetChance = false;
    private int clickNum = 0;

    private int allItemNum = -1;
    private bool allItemUsing = false;

    private GameObject Player;
    [SerializeField] Transform forwardObj;
    [SerializeField] GameObject EnemyGeneratePoints;
    [SerializeField] GameObject[] CarMesh;
    [SerializeField] GameObject ShieldParent;
    private Coroutine playCoroutine = null;

    [SerializeField] ItemUI _ui;
    [SerializeField] TextMeshProUGUI _coinText;
    [SerializeField] GameObject clickCoinText;
    [SerializeField] Image mouse;
    [SerializeField] Sprite[] clickMouse;
    [SerializeField] ImageAnimation[] gachaEffect;
    [SerializeField] GameObject FirstTimeItemText;
    private bool isFirstTimeItem = true;

    [SerializeField] GetCoinText getCoinText;
    // アクティブなコインテキストのリスト
    private List<GetCoinText> activeList = new List<GetCoinText>();
    // 非アクティブなコインテキストのオブジェクトプール
    private Stack<GetCoinText> inactivePool = new Stack<GetCoinText>();

    //実績用
    private bool[] useItem = new bool[13];
    [SerializeField] GameObject Notification;
    private AchieveNotification _notify;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _notify = Notification.GetComponent<AchieveNotification>();
        FirstItem = 0;
        SecondItem = 0;
        _coinText.text = ":0";
        FirstTimeItemText.SetActive(false);
        for (int i = 0; i < carTransparentMat.Length; i++)
        {
            Color col = carTransparentMat[i].color;
            float plus;
            if (col.a != 1f)
            {
                plus = 1f - col.a;
                col += new Color(0, 0, 0, plus);
                carTransparentMat[i].color = col;
            }
        }
        for (int i = 0; i < useItem.Length; i++)
        {
            useItem[i] = false;
        }
    }
    
    void Update()
    {
        // 逆順にループを回して、activeListの要素が途中で削除されても正しくループが回るようにする
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var getCoinText = activeList[i];
            if (!getCoinText.IsActive)
            {
                Remove(getCoinText);
            }
        }

        if (isCoinGetChance)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                clickNum++;
                mouse.sprite = clickMouse[1];
                StartCoroutine(reverseImage());
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (!canUseItem) return;
            StartCoroutine(UseItem());
        }
        if (canGacha && CoinNum >= 5)
        {
            canGacha = false;
            StartCoroutine(GachaEffect());
        }
        if (FirstItem == 0 && SecondItem != 0)//念のため
        {
            FirstItem = SecondItem;
            _ui.changeUI1(SecondItem);
            SecondItem = 0;
            _ui.changeUI2(0);
            canGacha = true;
        }
    }

    private IEnumerator GachaEffect()
    {
        GetCoin(-5);
        AudioManager.instance.SE(3);

        yield return null;
        if(FirstItem != 0)
            gachaEffect[1].isGacha = true;
        else
            gachaEffect[0].isGacha = true;
        yield return new WaitForSeconds(2f);
        gachaEffect[0].isGacha = false;
        gachaEffect[1].isGacha = false;
        yield return null;
        int ran = Random.Range(1, 14);
        if (FirstItem != 0)
        {
            SecondItem = ran;
            _ui.changeUI2(ran);
        }
        else
        {
            FirstItem = ran;
            _ui.changeUI1(ran);
            canGacha = true;

            if (isFirstTimeItem)
                FirstTimeItemText.SetActive(true);
        }

        yield break;
    }

    private IEnumerator UseItem()
    {
        if (FirstItem == 0) yield break;
        if(!canUseItem) yield break;
        canUseItem = false;

        if (FirstTimeItemText.activeSelf)
        {
            isFirstTimeItem = false;
            FirstTimeItemText.SetActive(false);
        }

        //アイテム種類が13の時の仕様は特殊なのでここで行う
        if (FirstItem == 13)
        {
            allItemUsing = true;
            if (allItemNum == -1)
            {
                AudioManager.instance.SE(12);
                allItemNum = 4;
            }
            //アイテムを使った
            CheckItemUse(FirstItem);
        }
        int ranUseItem;
        if (allItemUsing)
        {
            int isContinue = 0;
            //出るアイテムの種類は1,3,5,6,7,9,10,11,12
            ranUseItem = Random.Range(1, 10);
            if (ranUseItem == 2)
            {
                ranUseItem = 3;
            }else if (ranUseItem > 2)
            {
                ranUseItem += 2;
            }
            else if (ranUseItem > 5)
            {
                ranUseItem += 3;
            }
            FirstItem = ranUseItem;
            yield return null;

            switch (allItemNum)
            {
                case 4:
                    isContinue = Random.Range(0, 9);
                    break;
                case 3:
                    isContinue = Random.Range(0, 5);
                    break;
                case 2:
                    isContinue = Random.Range(0, 3);
                    break;
                case 1:
                    isContinue = Random.Range(0, 1);
                    break;
                case 0:
                    isContinue = 0;
                    break;
            }
            allItemNum--;

            if (isContinue == 0)
            {
                allItemUsing = false;
                allItemNum = -1;
            }
        }

        float y = 0.5f;
        Vector3 generatePos = Player.transform.localPosition + new Vector3(0, y, 0);
        Vector3 rotation = new Vector3(0, 1, 0);
        float angle = 90;
        yield return null;
        switch (FirstItem)
        {
            case 1://ロケット
                if (Player.transform.rotation.z > 0.5f|| Player.transform.rotation.z < -0.5f)
                    y *= -1;
                rotation = new Vector3(0, 0, 0.12f);
                generatePos = Player.transform.localPosition + new Vector3(0, y, 0);
                Instantiate(Item[0], generatePos, Quaternion.LookRotation(forwardObj.forward));
                Instantiate(Item[0], generatePos, Quaternion.LookRotation(forwardObj.forward + rotation));
                Instantiate(Item[0], generatePos, Quaternion.LookRotation(forwardObj.forward - rotation));
                yield return new WaitForSeconds(0.5f);
                break;
            case 2://地震
                AudioManager.instance.SE(5);
                generatePos = forwardObj.transform.position + new Vector3(2.5f, 0, 0);
                if (punchNum == -1)
                {
                    insCol = Item[1].GetComponentInChildren<BoxCollider>();
                    punchNum = 3;
                    yield return null;
                }
                if (punchNum > 0)
                {
                    Item[1].transform.position = generatePos;
                    Item[1].transform.rotation= Quaternion.LookRotation(forwardObj.forward);
                    yield return null;
                    Item[1].GetComponent<ParticleSystem>().Play();

                    punchNum--;

                    //アイテムを使った
                    CheckItemUse(FirstItem);

                    if (punchNum == 0)
                    {
                        yield return new WaitForSeconds(1f);

                        punchNum = -1;

                        FirstItem = 0;
                        _ui.changeUI1(0);
                        if (SecondItem != 0)
                        {
                            FirstItem = SecondItem;
                            _ui.changeUI1(SecondItem);
                            SecondItem = 0;
                            _ui.changeUI2(0);
                        }
                        yield return null;
                        canGacha = true;
                        yield return new WaitForSeconds(0.1f);
                        canUseItem = true;
                        yield break;
                    }
                    else
                    {
                        yield return new WaitForSeconds(0.4f);
                        canUseItem = true;
                        yield break;
                    }
                }
                break;
            case 3://レーザー
                insCol = Item[2].GetComponentInChildren<BoxCollider>();
                Item[2].SetActive(true);
                Item[2].transform.position = new Vector3(Player.transform.localPosition.x + 130, 15, 6.85f);
                yield return null;
                Item[2].GetComponent<ParticleSystem>().Play();
                AudioManager.instance.SE(21);
                yield return new WaitForSeconds(1f);
                insCol.enabled = true;
                yield return new WaitForSeconds(3f);
                insCol.enabled = false;
                Item[2].SetActive(false);
                break;
            case 4://お化け
                //車のMaterial変化　Collider変化
                AudioManager.instance.SE(4);
                AudioManager.instance.SE(6);
                MeshRenderer[] renderer = new MeshRenderer[5];
                renderer[0] = CarMesh[0].GetComponent<MeshRenderer>();
                Material[] BeforeMaterials = renderer[0].sharedMaterials;
                Material[] materials = renderer[0].sharedMaterials;
                for(int i = 0; i < 3; i++)
                {
                    materials[i] = carTransparentMat[i];
                }
                renderer[0].sharedMaterials = materials;
                yield return null;

                Material[][] BeforeMaterials2 = new Material[4][];
                for (int i = 0; i < 4; i++)
                {
                    renderer[i + 1] = CarMesh[i + 1].GetComponent<MeshRenderer>();
                    BeforeMaterials2[i] = renderer[i + 1].sharedMaterials;
                    Material[] materials2 = renderer[i + 1].sharedMaterials;
                    for (int j = 0; j < 2; j++)
                    {
                        materials2[j] = carTransparentMat[j + 3];
                    }
                    renderer[i + 1].sharedMaterials = materials2;
                }
                yield return null;

                insCol = Player.GetComponentInChildren<BoxCollider>();
                Color col = new Color(0, 0, 0, 0.025f);
                for (int i = 0; i < 30; i++)
                {
                    if (i == 10)
                    {
                        //ここから無敵
                        insCol.gameObject.layer = 12;
                    }
                    for (int j = 0; j < 5; j++)
                    {
                        carTransparentMat[j].color -= col;
                    }
                    yield return null;
                }

                yield return new WaitForSeconds(4f);

                //元に戻す
                renderer[0].sharedMaterials = BeforeMaterials;
                col *= 30;
                yield return null;
                for (int j = 0; j < 5; j++)
                {
                    carTransparentMat[j].color += col;
                }
                yield return null;
                for (int i = 0; i < 4; i++)
                {
                    renderer[i + 1].sharedMaterials = BeforeMaterials2[i];
                }
                insCol.gameObject.layer = 9;
                yield return null;
                break;
            case 5://壁時間回復
                AudioManager.instance.SE(4);
                AudioManager.instance.SE(7);
                PlayerWallSpecification pw = GetComponent<PlayerWallSpecification>();
                pw.HealStayWallTime(5f);
                yield return new WaitForSeconds(0.4f);
                break;
            case 6://鎧装備
                AudioManager.instance.SE(8);
                Item[3].SetActive(true);
                yield return new WaitForSeconds(0.4f);
                break;
            case 7://回転シールド
                generatePos = ShieldParent.transform.position + new Vector3(2f, 0, 0);
                Instantiate(Item[4], generatePos, Quaternion.AngleAxis(angle, rotation), ShieldParent.transform);
                generatePos = ShieldParent.transform.position + new Vector3(-2f, 0, 0);
                angle *= -1;
                Instantiate(Item[4], generatePos, Quaternion.AngleAxis(angle, rotation), ShieldParent.transform);

                if (ShieldParent.transform.childCount >= 8 && !PlayerPrefs.HasKey("Shield"))
                {
                    PlayerPrefs.SetInt("Shield", 1);
                    _notify.PlayNotification("守りガチ勢");
                }

                yield return new WaitForSeconds(0.4f);
                break;
            case 8://コイン取得
                AudioManager.instance.SE(4);
                clickCoinText.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                isCoinGetChance = true;
                yield return new WaitForSeconds(3f);
                AudioManager.instance.SE(9);
                clickCoinText.SetActive(false);
                if (clickNum > 30)
                    clickNum = 30;
                if (clickNum >= 30 && !PlayerPrefs.HasKey("Clicker"))
                {
                    PlayerPrefs.SetInt("Clicker", 1);
                    _notify.PlayNotification("連打の鬼");
                }
                GetCoin(4 + clickNum / 2);
                clickNum = 0;
                yield return new WaitForSeconds(0.4f);
                break;
            case 9://マグネット
                AudioManager.instance.SE(4);
                Item[5].SetActive(true);
                yield return new WaitForSeconds(0.4f);
                if (playCoroutine != null)
                {
                    StopCoroutine(playCoroutine);
                }
                playCoroutine = StartCoroutine("WaitFinishMagnet");
                break;
            case 10://雷
                AudioManager.instance.SE(10);
                rotation = new Vector3(0, 10f, 0);
                EnemyMove sample = EnemyGeneratePoints.GetComponentInChildren<EnemyMove>();
                if (sample.transform.localScale.x < 1f)
                    rotation = new Vector3(0, 5f, 0);
                foreach (Transform move in EnemyGeneratePoints.transform)
                {
                    generatePos = move.gameObject.transform.position + rotation;
                    Instantiate(Item[6], generatePos, Quaternion.identity, move);
                    var enemy = move.gameObject.GetComponent<EnemyMove>();
                    if (enemy != null)
                        enemy.ChangeSize();
                }
                yield return new WaitForSeconds(0.75f);
                break;
            case 11://テレポート
                AudioManager.instance.SE(4);
                //敵の車をすべて取得
                GameObject randomObj;
                int choiceNum;
                GameObject[] enemyCar = GameObject.FindGameObjectsWithTag("EnemyCar");
                yield return null;
                List<GameObject> myList = new List<GameObject>(enemyCar.Length);
                List<GameObject> useList = new List<GameObject>(enemyCar.Length / 2);
                for (int i = 0; i < enemyCar.Length; i++)
                    myList.Add(enemyCar[i]);

                yield return null;
                //その内の半分を選出
                for (int i = 0; i < enemyCar.Length / 2; i++)
                {
                    //Listの中からランダムで1つを選ぶ
                    randomObj = myList[Random.Range(0, myList.Count)];
                    //選んだオブジェクトをuseListに追加
                    useList.Add(randomObj);
                    //選んだオブジェクトのリスト番号を取得
                    choiceNum = myList.IndexOf(randomObj);
                    //同じリスト番号をmyListから削除
                    myList.RemoveAt(choiceNum);
                }
                yield return null;
                //最大対象数は12台
                while (useList.Count > 12)
                {
                    useList.RemoveAt(useList.Count - 1);
                }

                for (int i = 0; i < useList.Count; i++)
                {
                    if (useList[i] != null)
                    {
                        if (useList[i].transform.position.x > Player.transform.position.x + 15f)
                        {
                            //自分と上下が同じ奴が対象
                            if (Player.transform.position.y <= 3f&& useList[i].transform.position.y < 3f)
                                useList[i].GetComponentInParent<EnemyMove>().ReversalUpDown();
                            if (Player.transform.position.y > 3f && useList[i].transform.position.y > 3f)
                                useList[i].GetComponentInParent<EnemyMove>().ReversalUpDown();
                        }
                    }
                }

                AchievementDetection.TeleportNum++;
                yield return new WaitForSeconds(1f);
                break;
            case 12://波動
                AudioManager.instance.SE(11);
                Item[7].GetComponent<ParticleSystem>().Play();
                foreach (Transform move in EnemyGeneratePoints.transform)
                {
                    var enemy = move.gameObject.GetComponent<EnemyMove>();
                    if (enemy != null)
                        enemy.ChangeSpeed(enemy.speed / 2f);
                    yield return null;
                }

                yield return new WaitForSeconds(0.4f);
                break;
        }
        yield return null;
        if (!allItemUsing)
        {
            //アイテムを使った
            CheckItemUse(FirstItem);

            FirstItem = 0;
            _ui.changeUI1(0);
            if (SecondItem != 0)
            {
                FirstItem = SecondItem;
                _ui.changeUI1(SecondItem);
                SecondItem = 0;
                _ui.changeUI2(0);
            }
            yield return null;
            canGacha = true;
        }
        yield return new WaitForSeconds(0.1f);
        canUseItem = true;
        if (allItemUsing)
        {
            FirstItem = 14;
            StartCoroutine(UseItem());
        }
        yield break;
    }

    private IEnumerator WaitFinishMagnet()
    {
        yield return new WaitForSeconds(20f);
        Item[5].SetActive(false);
        yield break;
    }

    private IEnumerator reverseImage()
    {
        yield return new WaitForSeconds(0.1f);
        mouse.sprite = clickMouse[0];
        yield break;
    }

    //実績・図鑑判定用
    private void CheckItemUse(int ItemNum)
    {
        PlayerPrefs.SetInt("UseItem" + ItemNum.ToString(), 1);
        useItem[ItemNum - 1] = true;
        for (int i = 0; i < useItem.Length; i++)
            if (!useItem[i]) return;
        //全てのアイテムを使用した
        if (!PlayerPrefs.HasKey("ItemMaster"))
        {
            PlayerPrefs.SetInt("ItemMaster", 1);
            _notify.PlayNotification("アイテムマスター");
        }
    }

    public void GetCoin(int num)
    {
        AudioManager.instance.SE(0);
        CoinNum += num;
        Active(num);
        _coinText.text = ":" + CoinNum;
        if (CoinNum >= 100 && !PlayerPrefs.HasKey("Millionaire"))
        {
            PlayerPrefs.SetInt("Millionaire" , 1);
            _notify.PlayNotification("大富豪");
        }
    }

    // コインテキストを非アクティブ化するメソッド
    public void Remove(GetCoinText getCoinText)
    {
        activeList.Remove(getCoinText);
        inactivePool.Push(getCoinText);
    }

    // コインテキストをアクティブ化するメソッド　自分のみ
    private void Active(int num)
    {
        // 非アクティブのコインがあれば使い回す、なければ生成する
        var GetCoinText = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(getCoinText, _coinText.gameObject.transform);
        
        GetCoinText.gameObject.SetActive(true);
        GetCoinText.Init(num);
        activeList.Add(GetCoinText);
    }
}
