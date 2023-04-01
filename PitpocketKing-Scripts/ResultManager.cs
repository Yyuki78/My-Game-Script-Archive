using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Image[] BGs = new Image[2];
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _rankText;
    [SerializeField] TextMeshProUGUI _commentText;
    [SerializeField] GameObject[] buttons = new GameObject[2];

    private AudioManager _audio;

    void Start()
    {
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        BGs[0].color = new Color(1, 1, 1, 0);
        BGs[1].color = new Color(1, 1, 1, 0);
        _titleText.color = new Color(1, 1, 1, 0);
        _scoreText.color = new Color(1, 1, 1, 0);
        _rankText.color = new Color(1, 1, 1, 0);
        _commentText.color = new Color(1, 1, 1, 0);
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);
    }

    private void OnEnable()
    {
        BGs[0].color = new Color(1, 1, 1, 0);
        BGs[1].color = new Color(1, 1, 1, 0);
        _titleText.color = new Color(1, 1, 1, 0);
        _scoreText.color = new Color(1, 1, 1, 0);
        _rankText.color = new Color(1, 1, 1, 0);
        _commentText.color = new Color(1, 1, 1, 0);
        buttons[0].SetActive(false);
        buttons[1].SetActive(false);

        bool isArrest = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().isArrest;
        int getmoney = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().getMoney;
        _scoreText.text = "獲得金額:" + getmoney;
        int ran = Random.Range(0, 2);
        if (getmoney > 75000000)
        {
            _rankText.text = "ランク:神級すり";
            if (ran == 0)
                _commentText.text = "一言:すり界最強です";
            else
                _commentText.text = "一言:経歴不明億万長者";

            if (isArrest)
                _commentText.text = "一言:逆にどうしてこうなった";
        }
        else if (getmoney > 3000000)
        {
            _rankText.text = "ランク:超人すり";
            if (ran == 0)
                _commentText.text = "一言:すり、家を買う";
            else
                _commentText.text = "一言:人ではないのカモ";

            if (isArrest)
                _commentText.text = "一言:金で逃げよう";
        }
        else if (getmoney > 1500000)
        {
            _rankText.text = "ランク:熟練すり";
            if (ran == 0)
                _commentText.text = "一言:すりとして充分";
            else
                _commentText.text = "一言:車でも買うか…";

            if (isArrest)
                _commentText.text = "一言:天国から地獄";
        }
        else if (getmoney > 750000)
        {
            _rankText.text = "ランク:一般すり";
            if (ran == 0)
                _commentText.text = "一言:盗みたい放題";
            else
                _commentText.text = "一言:今日は焼肉だ!";

            if (isArrest)
                _commentText.text = "一言:優秀な警察だぁ";
        }
        else if (getmoney >= 0)
        {
            _rankText.text = "ランク:新米すり";
            if (ran == 0)
                _commentText.text = "一言:10コンボを目指そう";
            else
                _commentText.text = "一言:アイテムを探そう";

            if (isArrest)
                _commentText.text = "一言:マークされてたのかも";
        }
        else
        {
            _rankText.text = "ランク:逆すり";
            if (ran == 0)
                _commentText.text = "一言:この街は治安が悪い";
            else
                _commentText.text = "一言:生きては行けぬ";

            if (isArrest)
                _commentText.text = "一言:泣きっ面に蜂";
        }
        if (isArrest)
            _rankText.text = "ランク:逮捕";

        StartCoroutine(ResultAnimation());
        Debug.Log("リザルトスタート");
    }

    private IEnumerator ResultAnimation()
    {
        yield return new WaitForSecondsRealtime(3.5f);
        _audio.SE18();
        for (int i = 0; i < 100; i++)
        {
            transform.localPosition += new Vector3(0, 1, 0);
            BGs[0].color += new Color(0, 0, 0, 0.01f);
            BGs[1].color += new Color(0, 0, 0, 0.01f);
            _titleText.color += new Color(0, 0, 0, 0.01f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < 50; i++)
        {
            _scoreText.gameObject.transform.localPosition += new Vector3(0, 1, 0);
            _scoreText.color += new Color(0, 0, 0, 0.02f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < 50; i++)
        {
            _rankText.gameObject.transform.localPosition += new Vector3(0, 1, 0);
            _rankText.color += new Color(0, 0, 0, 0.02f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        for (int i = 0; i < 50; i++)
        {
            _commentText.gameObject.transform.localPosition += new Vector3(0, 1, 0);
            _commentText.color += new Color(0, 0, 0, 0.02f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        yield return new WaitForSecondsRealtime(0.25f);
        buttons[0].SetActive(true);
        buttons[1].SetActive(true);
    }

    public void OnClickRetryButton()
    {
        GameManager.isEnd = false;
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAndWait(0));
    }

    public void OnClickTitleButton()
    {
        GameManager.isEnd = false;
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneAndWait(1));
    }

    private IEnumerator LoadSceneAndWait(int type)
    {
        float start = Time.realtimeSinceStartup;
        AsyncOperation ope;
        if (type == 0)
            ope = SceneManager.LoadSceneAsync("SampleScene");
        else
            ope = SceneManager.LoadSceneAsync("Title");

        ope.allowSceneActivation = false;

        while (Time.realtimeSinceStartup - start < 0.5f)
        {
            yield return null;
        }
        ope.allowSceneActivation = true;
    }
}
