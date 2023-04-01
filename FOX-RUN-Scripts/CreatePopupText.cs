using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatePopupText : MonoBehaviour
{
    [SerializeField] private GameObject Fox;
    private EnemyMove _move;

    [SerializeField] private GameObject Light;
    private RoundLight _light;

    //土管イベント用の変数
    [SerializeField] GameObject _DokanDetect;
    private DokanEvent _DokanEve;
    private bool onceEve = false;
    private bool onceEve2 = false;//これはネズミイベント用
    private bool onceEve4 = false;//これは農具イベント用

    //Jumpイベント用の変数
    [SerializeField] GameObject JumpDetect;
    private JumpEvent _jump;
    private bool onceEve3 = false;

    [SerializeField] BGMControl _Attack;//攻撃時のゲージ上昇用
    private bool onceAttack = false;

    [SerializeField] GameObject _popuptextPrefab;
    [SerializeField] GameObject _popuptextEvePrefab;

    private Text _text;

    private bool once = false;
    private bool once2 = false;
    // Start is called before the first frame update
    void Start()
    {
        _move = Fox.GetComponent<EnemyMove>();
        _light = Light.GetComponent<RoundLight>();
        _DokanEve = _DokanDetect.GetComponent<DokanEvent>();
        _jump = JumpDetect.GetComponent<JumpEvent>();
        _text = _popuptextPrefab.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //それぞれのイベントに対応してPrayerPrefsで図鑑埋めを行う
        //土管イベント
        if (_DokanEve.DokanEventBool == true && onceEve == false)
        {
            PlayerPrefs.SetInt("Event4", 1);
            StartCoroutine("CreateEJT");
            onceEve = true;
        }

        //ネズミイベント
        if (_move.RatTrue == true && onceEve2 == false)
        {
            StartCoroutine("CreateERT");
            onceEve2 = true;
        }
        if (_move.RatTrue == false && onceEve2 == true)
        {
            onceEve2 = false;
        }

        //Jumpイベント
        if (_jump.JumpEve == true && onceEve3 == false)
        {
            PlayerPrefs.SetInt("Event3", 1);
            StartCoroutine("CreateEJT");
            onceEve3 = true;
        }
        if (_jump.JumpEve == false && onceEve3 == true)
        {
            onceEve3 = false;
        }

        //農具イベント
        if (_move.ObsTrue == true && onceEve4 == false)
        {
            PlayerPrefs.SetInt("Event5", 1);
            StartCoroutine("CreateEDT");
            onceEve4 = true;
        }

        //攻撃イベント
        if (_Attack.AttackTrue == true && onceAttack == false)
        {
            PlayerPrefs.SetInt("Event2", 1);
            StartCoroutine("CreateEAT");
            onceAttack = true;
        }
        if (_Attack.AttackTrue == false && onceAttack == true)
        {
            onceAttack = false;
        }

        //ゲージが増えている系
        if (_move.plus == true && once == false)
        {
            PlayerPrefs.SetInt("Event1", 1);
            once = true;
            StartCoroutine("CreateT");
        }

        if (_move.plus == false)
        {
            once = false;
            StopCoroutine("CreateT");
        }
        //ゲージが減っている系
        if (_move.reduce == true && once2 == false && _move.fill > 0 && _move.plus == false)
        {
            //once = false;
            once2 = true;
            StopCoroutine("CreateT");
            StartCoroutine("CreateMT");
        }

        if (_move.fill <= 0)
        {
            StopCoroutine("CreateMT");
        }

        if (_move.reduce == false)
        {
            once2 = false;
            StopCoroutine("CreateMT");
        }

        //光での色切り替え
        if (_light.night == true)
        {
            _text.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        }
        else
        {
            _text.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        }

        //ゲームクリア時に停止する
        if (_move.clear == true)
        {
            StopCoroutine("CreateT");
            StopCoroutine("CreateMT");
            this.enabled = false;
        }
    }

    //+1のテキスト表示
    IEnumerator CreateT()
    {
        for(; ; )
        {
            yield return new WaitForSeconds(0.2f);
            _popuptextPrefab.gameObject.GetComponent<Text>().text = "+1";
            var text = Instantiate(_popuptextPrefab, UnityEngine.Object.FindObjectOfType<Canvas>().transform);
            //yield break;
        }
    }
    //-1のテキスト表示
    IEnumerator CreateMT()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.2f);
            _popuptextPrefab.gameObject.GetComponent<Text>().text = "-1";
            var text = Instantiate(_popuptextPrefab, UnityEngine.Object.FindObjectOfType<Canvas>().transform);
            //yield break;
        }
    }

    //農具イベント成功時に一度だけ起こすテキスト表示
    IEnumerator CreateEDT()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.2f);
            _popuptextEvePrefab.gameObject.GetComponent<Text>().text = "+100";
            var text = Instantiate(_popuptextEvePrefab, UnityEngine.Object.FindObjectOfType<Canvas>().transform);
            yield return new WaitForSeconds(2.0f);
            yield break;
        }
    }
    //ネズミイベント成功時に一度だけ起こすテキスト表示
    IEnumerator CreateERT()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.2f);
            _popuptextEvePrefab.gameObject.GetComponent<Text>().text = "+10";
            var text = Instantiate(_popuptextEvePrefab, UnityEngine.Object.FindObjectOfType<Canvas>().transform);
            yield return new WaitForSeconds(2.0f);
            yield break;
        }
    }
    //土管イベントorJumpイベント成功時に一度だけ起こすテキスト表示
    IEnumerator CreateEJT()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.2f);
            _popuptextEvePrefab.gameObject.GetComponent<Text>().text = "+50";
            var text = Instantiate(_popuptextEvePrefab, UnityEngine.Object.FindObjectOfType<Canvas>().transform);
            yield return new WaitForSeconds(2.0f);
            yield break;
        }
    }
    //攻撃イベント成功時に一度だけ起こすテキスト表示
    IEnumerator CreateEAT()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(0.2f);
            _popuptextEvePrefab.gameObject.GetComponent<Text>().text = "+3";
            var text = Instantiate(_popuptextEvePrefab, UnityEngine.Object.FindObjectOfType<Canvas>().transform);
            yield return new WaitForSeconds(2.0f);
            yield break;
        }
    }
}
