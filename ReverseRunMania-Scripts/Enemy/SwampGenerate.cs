using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampGenerate : MonoBehaviour
{
    [SerializeField] GameObject Swamp;
    [SerializeField] SwampMove swampMove;
    [SerializeField] Mesh[] swampMesh;

    private bool isGenerate = false;

    private bool isSpecialGenerate = false;
    private bool plusSpecialGenerateNum = false;
    private float timeMax = 4f;

    private GameObject Player;
    private float playerPos;

    // アクティブな沼のリスト
    private List<SwampMove> activeList = new List<SwampMove>();
    // 非アクティブな沼のオブジェクトプール
    private Stack<SwampMove> inactivePool = new Stack<SwampMove>();

    //実績用
    private bool[] hitSwamp = new bool[6];
    [SerializeField] GameObject Notification;
    private AchieveNotification _notify;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _notify = Notification.GetComponent<AchieveNotification>();
        for (int i = 0; i < hitSwamp.Length; i++)
        {
            hitSwamp[i] = false;
        }
    }
    
    void Update()
    {
        playerPos = Player.transform.position.x;
        if (playerPos < 1000f) return;
        if (!isGenerate)
        {
            isGenerate = true;
            StartCoroutine(swampGenerate());
        }

        // 逆順にループを回して、activeListの要素が途中で削除されても正しくループが回るようにする
        for (int i = activeList.Count - 1; i >= 0; i--)
        {
            var swampMove = activeList[i];
            if (!swampMove.IsActive)
            {
                Remove(swampMove);
            }
        }

        if (playerPos < 8750f) return;
        if (!isSpecialGenerate)
            isSpecialGenerate = true;
        if (playerPos < 17750f) return;
        if (!isSpecialGenerate)
            isSpecialGenerate = true;
        if (playerPos < 20750f) return;
        if (!plusSpecialGenerateNum)
            plusSpecialGenerateNum = true;
    }

    // 沼を非アクティブ化するメソッド
    public void Remove(SwampMove swampMove)
    {
        activeList.Remove(swampMove);
        inactivePool.Push(swampMove);
    }

    // 沼をアクティブ化するメソッド　自分のみ
    private void Active(Vector3 pos, Quaternion rotate)
    {
        // 非アクティブのコインがあれば使い回す、なければ生成する
        var SwampMove = (inactivePool.Count > 0) ? inactivePool.Pop() : Instantiate(swampMove, transform);
        
        MeshFilter mesh = SwampMove.gameObject.GetComponentInChildren<MeshFilter>();
        int ranMesh = Random.Range(0, 10);
        mesh.mesh = swampMesh[ranMesh];

        int size = Random.Range(0, 3);
        SwampMove.gameObject.SetActive(true);
        SwampMove.Init(pos, rotate, size);
        activeList.Add(SwampMove);
    }

    private IEnumerator swampGenerate()
    {
        int ran;
        float posZ, angle, time;
        Vector3 rotate = new Vector3(0, 1, 0);
        Vector3 pos;
        while (true)
        {
            posZ = Random.Range(2.5f, 11.2f);

            ran = Random.Range(0, 2);
            if (ran == 0)
                pos = new Vector3(Player.transform.position.x + 250f, 0.02f, posZ);
            else
                pos = new Vector3(Player.transform.position.x + 250f, 6.03f, posZ);
            angle = Random.Range(0, 360);
            Active(pos, Quaternion.AngleAxis(angle, rotate));

            //特殊生成
            if (isSpecialGenerate)
            {
                int ranGene = Random.Range(0, 15);
                if (ranGene == 0)
                {
                    time = Random.Range(1f, timeMax);
                    yield return new WaitForSeconds(time);
                    SpecialGenerate();
                }
            }

            time = Random.Range(1f, timeMax);
            yield return new WaitForSeconds(time);
        }
    }

    private void SpecialGenerate()
    {
        int ran, ranPos, generateNum;
        float posZ, angle;
        Vector3 rotate = new Vector3(0, 1, 0);
        Vector3 pos;

        ranPos = Random.Range(0, 2);
        ran = Random.Range(0, 2);
        if (plusSpecialGenerateNum)
            generateNum = Random.Range(2, 4);
        else
            generateNum = 2;
        if (generateNum == 2)
        {
            if (ranPos == 0)
                posZ = Random.Range(8.2f, 11.2f);
            else
                posZ = Random.Range(2.5f, 5.5f);

            if (ran == 0)
                pos = new Vector3(Player.transform.position.x + 250f, 0.02f, posZ);
            else
                pos = new Vector3(Player.transform.position.x + 250f, 6.03f, posZ);
            angle = Random.Range(0, 360);
            Active(pos, Quaternion.AngleAxis(angle, rotate));

            if (ranPos == 0)
                posZ -= 6f;
            else
                posZ += 6f;

            if (ran == 0)
                pos = new Vector3(Player.transform.position.x + 250f, 0.02f, posZ);
            else
                pos = new Vector3(Player.transform.position.x + 250f, 6.03f, posZ);
            angle = Random.Range(0, 360);
            Active(pos, Quaternion.AngleAxis(angle, rotate));
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                if(i==0)
                    posZ= Random.Range(10.25f, 11.2f);
                else if(i==1)
                    posZ= Random.Range(6f, 7.65f);
                else
                    posZ = Random.Range(2.5f, 3.5f);

                if (ran == 0)
                    pos = new Vector3(Player.transform.position.x + 250f, 0.02f, posZ);
                else
                    pos = new Vector3(Player.transform.position.x + 250f, 6.03f, posZ);
                angle = Random.Range(0, 360);
                Active(pos, Quaternion.AngleAxis(angle, rotate));
            }
        }    }

    //実績・図鑑判定用
    public void CheckSwampHit(int SwampNum)
    {
        PlayerPrefs.SetInt("HitSwamp" + SwampNum.ToString(), 1);
        hitSwamp[SwampNum] = true;
        for (int i = 0; i < hitSwamp.Length; i++)
            if (!hitSwamp[i]) return;
        //全ての沼を踏んだ
        if (!PlayerPrefs.HasKey("AllSwamp"))
        {
            PlayerPrefs.SetInt("AllSwamp", 1);
            _notify.PlayNotification("何物にも左右されず");
        }
    }
}
