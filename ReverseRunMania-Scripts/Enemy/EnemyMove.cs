using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public int CarSize;
    public float speed;

    private GameObject Player;
    private Vector3 pos;
    private float sideMoveVal = 0;
    private Vector3 effectPos;
    [SerializeField] GameObject teleportParticle;
    private bool isTeleport = false;

    private AudioSource _audio;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        pos = new Vector3(speed, 0, sideMoveVal);
        _audio = GetComponent<AudioSource>();

        //少し初期位置を左右にずらす
        float ranPosX;
        if (transform.localPosition.z < 3f)
        {
            ranPosX = Random.Range(0, 0.5f);
        }
        else if (transform.localPosition.z > 11f)
        {
            ranPosX = Random.Range(-0.5f, 0);
        }
        else
        {
            ranPosX = Random.Range(-0.5f, 0.5f);
        }
        transform.position += new Vector3(0, 0, ranPosX);

        //左右に動く
        int ran = Random.Range(0, 3);
        float time, MaxSlide;
        switch (ran)
        {
            case 0://何もしない(真っすぐ進む)
                break;
            case 1://左右にふらふら動く(小刻み)
                time = Random.Range(1f, 2.5f);
                StartCoroutine(SideToSide(time));
                MaxSlide = 0.005f - (CarSize / 1000f);
                sideMoveVal = Random.Range(0.001f, MaxSlide);
                break;
            case 2://左右にふらふら動く(ゆっくり)
                time = Random.Range(3.5f, 4.5f);
                StartCoroutine(SideToSide(time));
                MaxSlide = 0.003f - (CarSize / 1500f);
                sideMoveVal = Random.Range(0.001f, MaxSlide);
                break;
        }
    }
    
    void Update()
    {
        if (GameManager.isGameOver) return;
        transform.position -= pos;
    }

    private IEnumerator SideToSide(float time)
    {
        var wait = new WaitForSeconds(time);

        pos = new Vector3(speed, 0, sideMoveVal);
        yield return new WaitForSeconds(time / 2);

        while (true)
        {
            sideMoveVal *= -1;
            pos = new Vector3(speed, 0, sideMoveVal);
            yield return wait;
        }
    }

    public void ChangeSpeed(float changeValue)
    {
        speed = changeValue;
        pos = new Vector3(speed, 0, sideMoveVal);
    }

    public void ChangeSize()
    {
        StartCoroutine(changeSize());
    }

    private IEnumerator changeSize()
    {
        var wait = new WaitForSeconds(0.2f);
        Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);
        for (int i = 0; i < 3; i++)
        {
            yield return wait;
            if (transform.localScale.x > 0f)
                transform.localScale -= scale;
            if (transform.localScale.x <= 0f && !PlayerPrefs.HasKey("Vanish"))
                AchievementDetection.VanishCar = true;
        }
        yield break;
    }

    public void HitItem(float upSpeed)
    {
        StartCoroutine(hitItem(upSpeed));
    }

    private IEnumerator hitItem(float upSpeed)
    {
        AchievementDetection.DestroyCarNum++;

        effectPos = new Vector3(0, upSpeed - (CarSize / 50f), 0);
        if (transform.localRotation.x > 0.5f || transform.localRotation.x < -0.5f)
            effectPos *= -1;

        int n = Random.Range(0, 2);
        float ran;
        if (n==0)
            ran= Random.Range(-10f + (CarSize * 3f), -14f + (CarSize * 3f));
        else
            ran = Random.Range(10f - (CarSize * 3f), 14f - (CarSize * 3f));

        for (int i = 0; i < 50; i++)
        {
            transform.position += effectPos;
            transform.Rotate(ran/2f, ran, 0);
            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }

    public void ReversalUpDown()
    {
        if (transform.position.y < 3f)
        {
            StartCoroutine(reversalUpDown(true));
        }
        else
        {
            StartCoroutine(reversalUpDown(false));
        }
    }

    private IEnumerator reversalUpDown(bool goUp)
    {
        if (isTeleport) yield break;
        isTeleport = true;
        _audio.PlayOneShot(_audio.clip);
        float posY;
        if (goUp)
        {
            posY = 6.05f;
        }
        else
        {
            posY = -6.05f;
        }

        //テレポートパーティクルの生成
        Instantiate(teleportParticle, transform);

        yield return new WaitForSeconds(1.5f);
        //テレポート
        transform.localEulerAngles += new Vector3(180, 0, 0);
        transform.position += new Vector3(0, posY, 0);

        isTeleport = false;
        yield break;
    }
}
