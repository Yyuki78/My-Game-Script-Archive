using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isEnd = false;//ゲーム終了フラッグ

    [SerializeField] GameObject NPC;
    [SerializeField] GameObject superRichNPC;

    [SerializeField] Transform[] generatePoints;

    //消滅時のParticle
    [SerializeField] GameObject VanishParticle;

    //NPC頭上に表示するObjの色　役割で変わる
    [SerializeField] Material[] colorMat;

    [SerializeField] GameObject sun; //Directional Light
    private Light _light;

    //Skybox変更用
    [SerializeField] Material[] skyboxMat;

    public bool waitStart = true;//スタート待機用

    public float elapsedTime { private set; get; } //経過時間

    //アイテム取得イベント系の変数
    private bool isAdditionNPC = false;
    private bool isPopSuperRich = false;
    [SerializeField] GameObject[] Items;//出現するアイテムのPrefab
    private bool popItem = false;//これが切り替わるとアイテムが出現する
    private int beforeItem = 4;//二連続同じアイテムを出さない用

    private bool isFirstHour = true;//最初の1時間目

    [SerializeField] GameObject ItemTextControllerObj;
    private ItemTextController _itemText;

    public bool isArrest { private set; get; } = false;
    [SerializeField] GameObject FinishPanel;//ゲーム終了時のパネル

    private AudioManager _audio;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        _light = sun.GetComponent<Light>();
        _itemText = ItemTextControllerObj.GetComponent<ItemTextController>();
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        for (int i = 0; i < 20; i++)
        {
            PopNPC();
        }
        FinishPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitStart || isEnd) return;
        updateTime();
        popUpItem();
    }

    private void PopNPC()
    {
        int ran = Random.Range(0, generatePoints.Length);
        var ins = Instantiate(NPC, generatePoints[ran].position, Quaternion.identity, transform);
        NPCInformation info = ins.GetComponent<NPCInformation>();
        int parcent = Random.Range(0, 100);
        if (parcent < 60)
        {
            info.role = 0;
        }
        else if (parcent < 65)
        {
            info.role = 1;
        }
        else if (parcent < 85)
        {
            info.role = 2;
        }
        else if (parcent < 95)
        {
            info.role = 3;
        }
        else
        {
            info.role = 4;
        }

        var rotateObj = ins.GetComponentInChildren<RotateObj>();
        rotateObj.gameObject.GetComponent<MeshRenderer>().material = colorMat[info.role];
    }

    public void Repop(Vector3 pos)
    {
        Instantiate(VanishParticle, pos, Quaternion.EulerAngles(-90, 0, 0), transform);
        PopNPC();
    }

    private void updateTime()
    {
        elapsedTime += Time.deltaTime;

        int hour = (int)elapsedTime / 15;
        if (hour >= 12)
            GameOver(false);

        if (hour < 12)
        {
            RenderSettings.skybox = skyboxMat[hour];
        }
        
        if (sun.transform.eulerAngles.x > 180)
        {
            sun.transform.rotation = Quaternion.EulerAngles(180, 0, 0);
        }
        else
        {
            sun.transform.Rotate(0.015f, 0, 0);
        }

        if (_light.intensity > 0)
            _light.intensity = 1f - (elapsedTime / 125);

        if (RenderSettings.ambientIntensity > 0.2f)
            RenderSettings.ambientIntensity = 1f - (elapsedTime / 140);
    }

    //Itemの生成　ついでにNPCも1時間ごとに一人増やす
    private void popUpItem()
    {
        if ((int)elapsedTime % 15 == 14 && !popItem)
        {
            if (isFirstHour)//最初の1時間経過時のみ5人追加する(6人)
            {
                isFirstHour = false;
                for (int i = 0; i < 5; i++)
                    PopNPC();
            }

            //30%の確率で警察が増える
            int ran = Random.Range(0, generatePoints.Length);
            if (ran < 3)
            {
                var ins = Instantiate(NPC, generatePoints[ran].position, Quaternion.identity, transform);
                NPCInformation info = ins.GetComponent<NPCInformation>();
                info.role = 2;
            }

            popItem = true;
            PopNPC();

            float ranX, ranY, ranZ;
            ranX = Random.Range(-1, -13);
            ranY= Random.Range(0.3f, 2);
            ranZ = Random.Range(-19, 43);
            int ranItem = Random.Range(0, 3);
            do
            {
                ranItem = Random.Range(0, 3);
            } while (beforeItem == ranItem);
            beforeItem = ranItem;
            Instantiate(Items[ranItem], new Vector3(ranX, ranY, ranZ), Quaternion.identity, transform);
        }
        if ((int)elapsedTime % 15 == 2)
            popItem = false;
    }

    //Itemを取得した   ItemBehaviorで呼ばれる
    public void getItem(int type)
    {
        _audio.SE12();
        _itemText.GetItem(type);
        if (type == 1)
        {
            Debug.Log("5人NPCが増加します");
            for (int i = 0; i < 5; i++)
            {
                PopNPC();
            }
        }else if (type == 2)
        {
            Debug.Log("超お金持ちが出現します");
            int ran = Random.Range(0, generatePoints.Length);
            Instantiate(superRichNPC, generatePoints[ran].position, Quaternion.identity, transform);
        }
    }

    //ゲーム終了　CameraControllerでも呼ばれる
    public void GameOver(bool isarrest)
    {
        if (isEnd) return;
        Debug.Log("ゲーム終了!");
        _audio.StopBGM();
        isArrest = isarrest;//逮捕か時間オーバーか
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().EnableMove = false;
        isEnd = true;
        StartCoroutine(finish());

        //超お金持ちの音楽を止める
        var rich = GameObject.FindGameObjectsWithTag("SuperRich");
        if (rich != null)
        {
            for(int i = 0; i < rich.Length; i++)
            {
                var audios = rich[i].GetComponents<AudioSource>();
                audios[0].volume = 0;
                audios[1].volume = 0;
            }
        }

        Time.timeScale = 0f;
    }

    private IEnumerator finish()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        FinishPanel.SetActive(true);
        _audio.BGM3();
        yield break;
    }
}
