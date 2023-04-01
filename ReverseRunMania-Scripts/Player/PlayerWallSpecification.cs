using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerWallSpecification : MonoBehaviour
{
    [SerializeField] float stayWallTime = 10f;

    [SerializeField] bool isHeal = false;

    private bool onceCoroutine = true;
    private bool onceCoroutine2 = true;
    private Coroutine playCoroutine = null;
    private Coroutine playWarnCoroutine = null;

    private CarState _state;

    [SerializeField] GameObject[] BlockWalls;

    [SerializeField] GameObject MeterUI;
    [SerializeField] Image _meterImage;
    [SerializeField] TextMeshProUGUI _meterText;

    private float warnTime = 3f;
    [SerializeField] TextMeshProUGUI _warnText;

    void Start()
    {
        _state = GetComponent<CarState>();
        _warnText.gameObject.SetActive(false);
    }

    void Update()
    {
        //壁走り時間が最大ならUIは消す
        if (stayWallTime >= 10f)
            MeterUI.SetActive(false);
        else
        {
            MeterUI.SetActive(true);
            _meterText.text = stayWallTime.ToString("f1");
            _meterImage.fillAmount = 1f - stayWallTime / 10f;
        }

        //道を規定秒待つと壁走り時間が回復するようになる
        if (!_state.isStickWall && stayWallTime < 10f && onceCoroutine)
        {
            onceCoroutine = false;
            if (playCoroutine != null)
            {
                StopCoroutine(playCoroutine);
            }
            playCoroutine = StartCoroutine("WaitStartHeal");
        }

        //壁走り中は回復や回復待機時間がリセットされる
        if (_state.isStickWall)
        {
            StopCoroutine(WaitStartHeal());
            onceCoroutine = true;
            isHeal = false;
            AchievementDetection.wallStayTime += Time.deltaTime;
        }

        //壁走り中は壁走り時間が減っていく
        if (_state.isStickWall && stayWallTime > 0f)
        {
            stayWallTime -= Time.deltaTime;
            if (stayWallTime <= 0f)
                stayWallTime = 0f;
        }

        //回復出来る時は壁走り時間が回復する
        if (isHeal)
        {
            if(stayWallTime >= 10f)
                stayWallTime = 10f;
            else
                stayWallTime += Time.deltaTime;
        }

        //壁走り時間がない且つ壁走り中なら外に出るよう警告する
        if (stayWallTime <= 0f)
        {
            if (_state.isStickWall)
            {
                if (onceCoroutine2)
                {
                    onceCoroutine2 = false;
                    _warnText.gameObject.SetActive(true);
                    //警告を出す
                    playWarnCoroutine = StartCoroutine("WarnEffect");
                }
            }
            else
            {
                onceCoroutine2 = true;
                if (playWarnCoroutine != null)
                    StopCoroutine(playWarnCoroutine);
                //左右の壁を封鎖する
                if (!BlockWalls[0].activeSelf)
                {
                    for (int i = 0; i < BlockWalls.Length; i++)
                        BlockWalls[i].SetActive(true);
                }
            }
        }
        else
        {
            _warnText.gameObject.SetActive(false);
            warnTime = 3f;
            if (playWarnCoroutine != null)
                playWarnCoroutine = null;
            //左右の壁を開放する
            if (BlockWalls[0].activeSelf)
            {
                for (int i = 0; i < BlockWalls.Length; i++)
                    BlockWalls[i].SetActive(false);
            }
        }
    }

    private IEnumerator WaitStartHeal()
    {
        if(stayWallTime <= 0f)
            yield return new WaitForSeconds(3.5f);
        else
            yield return new WaitForSeconds(2f);
        if (!_state.isStickWall)
            isHeal = true;
        yield break;
    }

    private IEnumerator WarnEffect()
    {
        AudioManager.instance.SE(17);
        while (warnTime > 0f)
        {
            warnTime -= Time.deltaTime;
            if (warnTime < 0f)
                warnTime = 0f;
            _warnText.text = "道路に戻って下さい\n" + warnTime.ToString("f2");
            yield return null;
        }
        //ゲームオーバー
        GetComponent<PlayerInformation>().TimeOver();
        yield break;
    }

    public void HealStayWallTime(float time)
    {
        if (time < 0f && stayWallTime <= 0f) return;
        if (time > 0f && stayWallTime <= 0f)
        {
            _warnText.gameObject.SetActive(false);
            warnTime = 3f;
            if (playWarnCoroutine != null)
                StopCoroutine(playWarnCoroutine);
            playWarnCoroutine = null;
        }
        playCoroutine = null;
        stayWallTime += time;
        if (stayWallTime > 10f)
        {
            stayWallTime = 10f;
        }
        if (stayWallTime < 0f)
        {
            stayWallTime = 0f;
        }
    }
}
