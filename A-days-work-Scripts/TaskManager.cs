using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] ShortTask[] _shorts;
    [SerializeField] MiddleTask[] _middles;
    [SerializeField] LongTask[] _longs;
    private int[] sTaskType = new int[4];
    private int[] mTaskType = new int[2];
    private int[] lTaskType = new int[2];

    private int start, end, count;
    List<int> numbers = new List<int>();

    private int sTaskClearCount = 0;
    private int mTaskClearCount = 0;
    private int lTaskClearCount = 0;
    private int finishTaskCount = 0;

    private int taskNum = 0;

    public float ElapsedTime { get; private set; } = 0;

    [SerializeField] GameObject TaskClearText;

    [SerializeField] GameObject AllFinishText;

    private Coroutine playCoroutine = null;

    private TaskTextManager _textmanager;

    [SerializeField] AudioClip[] _clip;
    private AudioSource[] _audio;

    void Start()
    {
        _textmanager = GetComponentInChildren<TaskTextManager>();
        _audio = GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (finishTaskCount == 8)
            AllFinishText.SetActive(true);
        else
            ElapsedTime += Time.deltaTime;
    }

    public void SetTask()
    {
        //ShortTaskSetting
        start = 0;
        end = 10;
        count = 4;
        for (int i = start; i < end; i++)
        {
            numbers.Add(i);
        }

        while (count-- > 0)
        {
            int index = Random.Range(0, numbers.Count);

            sTaskType[count] = numbers[index];
            //Debug.Log(sTaskType[count]);

            numbers.RemoveAt(index);
        }
        numbers = new List<int>();

        //MiddleTaskSetting
        start = 0;
        end = 6;
        count = 2;
        for (int i = start; i < end; i++)
        {
            numbers.Add(i);
        }

        while (count-- > 0)
        {
            int index = Random.Range(0, numbers.Count);

            mTaskType[count] = numbers[index];
            //Debug.Log(mTaskType[count]);

            numbers.RemoveAt(index);
        }
        numbers = new List<int>();

        //LongTaskSetting
        start = 0;
        end = 5;
        count = 2;
        for (int i = start; i < end; i++)
        {
            numbers.Add(i);
        }

        while (count-- > 0)
        {
            int index = Random.Range(0, numbers.Count);

            lTaskType[count] = numbers[index];
            //Debug.Log(lTaskType[count]);

            numbers.RemoveAt(index);
        }

        _shorts[sTaskType[0]].Prepare();
        _shorts[sTaskType[1]].Prepare();
        _middles[mTaskType[0]].Prepare();
        _longs[lTaskType[0]].Prepare();

        _textmanager.Prepare(sTaskType[0], sTaskType[1], mTaskType[0], lTaskType[0]);

        ElapsedTime = 0;

        _audio[0].clip = _clip[1];
        _audio[0].volume = 0.075f;
        _audio[0].Play();
    }

    public void FinishTask(int taskDifficult)
    {
        //Debug.Log(taskDifficult + "のタスク完了！");
        StartCoroutine(taskClearEffect(taskDifficult));
    }

    public void FinishTaskNumber(int num)
    {
        //Debug.Log(num + "番のショートタスク完了！");
        taskNum = num;
    }

    private IEnumerator taskClearEffect(int num)
    {
        finishTaskCount++;
        yield return new WaitForSeconds(0.01f);
        switch (num)
        {
            case 0:
                sTaskClearCount++;
                if (sTaskClearCount < 3)
                {
                    _shorts[sTaskType[sTaskClearCount + 1]].Prepare();
                    _textmanager.FinishTask(taskNum, sTaskType[sTaskClearCount + 1]);
                }
                else
                {
                    _textmanager.FinishTask(taskNum, 100);
                }
                break;
            case 1:
                mTaskClearCount++;
                if (mTaskClearCount < 2)
                {
                    _middles[mTaskType[mTaskClearCount]].Prepare();
                    _textmanager.FinishTask(11, mTaskType[mTaskClearCount]);
                }
                else
                {
                    _textmanager.FinishTask(11, 100);
                }
                break;
            case 2:
                lTaskClearCount++;
                if (lTaskClearCount < 2)
                {
                    _longs[lTaskType[lTaskClearCount]].Prepare();
                    _textmanager.FinishTask(12, lTaskType[lTaskClearCount]);
                }
                else
                {
                    _textmanager.FinishTask(12, 100);
                }
                break;
            default:
                Debug.Log("タスクの難易度が違います");
                break;
        }

        if (finishTaskCount == 8)
        {
            _audio[0].clip = _clip[2];
            _audio[0].volume = 0.15f;
            _audio[0].Play();
        } 

        if (playCoroutine != null)
        {
            StopCoroutine(playCoroutine);
        }
        playCoroutine = StartCoroutine("ClearEffect");

        yield break;
    }

    private IEnumerator ClearEffect()
    {
        _audio[1].PlayOneShot(_audio[1].clip);
        TaskClearText.SetActive(true);
        yield return new WaitForSeconds(3f);
        TaskClearText.SetActive(false);
        yield break;
    }
}
