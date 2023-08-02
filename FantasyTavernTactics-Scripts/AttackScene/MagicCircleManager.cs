using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleManager : MonoBehaviour
{
    [SerializeField] int FinishMagicCircleNum = -1;

    private int circleCount;

    private int activeNum = 0;
    private int beforeActiveNum = 0;
    private int sphereCount = 0;
    private int hitCount = 0;

    [SerializeField] GameObject[] MagicCircle;
    [SerializeField] GameObject[] MagicCircleFlame;
    private GameObject FinishCircleEffectObj;
    private GameObject currentFlame;

    private MagicianAttack _attack;
    private MagicCircleLineRenderer _renderer;

    void Start()
    {
        _attack = GetComponentInParent<MagicianAttack>();
        _renderer = GetComponent<MagicCircleLineRenderer>();

        circleCount = this.transform.childCount;
        MagicCircle = new GameObject[circleCount - 1];
        
        for (int i = 1; i < circleCount; i++)
        {
            Transform childTransform = this.transform.GetChild(i);
            MagicCircle[i - 1] = childTransform.gameObject;
            MagicCircle[i - 1].SetActive(false);
        }

        ActiveMagicCircle();
    }

    public void HitSphere(int type, OVRInput.Controller controller)
    {
        hitCount++;
        _attack.Hit(type, Vector3.zero, 0f, controller);
        if (sphereCount == hitCount)
        {
            ActiveMagicCircle();
        }
    }

    public void ActiveMagicCircle()
    {
        StartCoroutine(goNextCircle());
    }

    private IEnumerator goNextCircle()
    {
        if (FinishCircleEffectObj != null)
            FinishCircleEffectObj.SetActive(true);

        hitCount = 0;
        beforeActiveNum = activeNum;
        while (beforeActiveNum == activeNum)
            activeNum = Random.Range(0, circleCount - 1);
        sphereCount = MagicCircle[activeNum].transform.childCount;
        yield return null;

        GameObject[] CircleSphere = new GameObject[sphereCount];
        for (int i = 0; i < sphereCount; i++)
        {
            Transform childTransform = MagicCircle[activeNum].transform.GetChild(i);
            CircleSphere[i] = childTransform.gameObject;
            if (!CircleSphere[i].activeSelf)
                CircleSphere[i].SetActive(true);
        }
        yield return null;

        FinishMagicCircleNum++;
        if (FinishMagicCircleNum > 0)
            _attack.FinishOneMagicCircle(FinishMagicCircleNum);

        yield return new WaitForSeconds(0.2f);

        MagicCircle[beforeActiveNum].SetActive(false);
        _renderer.ResetLine();
        yield return null;

        if (currentFlame != null)
            Destroy(currentFlame);
        MagicCircle[activeNum].SetActive(true);
        currentFlame = Instantiate(MagicCircleFlame[FinishMagicCircleNum % MagicCircleFlame.Length], transform);
        FinishCircleEffectObj = currentFlame.transform.GetChild(2).gameObject;
        if (FinishCircleEffectObj.activeSelf)
            FinishCircleEffectObj.SetActive(false);
        yield break;
    }
}
