using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleLineRenderer : MonoBehaviour
{
    private GameObject currentObject;
    [SerializeField] private bool useWorldSpace = true;
    private int[] positionCount = new int[2] { 0, 0 };

    private LineRenderer[] _lineRenderer;
    private MagicCircleManager _manager;

    [SerializeField] AudioClip clip;
    private Transform _audioTrans;
    private AudioSource _audio;

    private void Start()
    {
        _lineRenderer = GetComponentsInChildren<LineRenderer>();
        _manager = GetComponent<MagicCircleManager>();

        _audioTrans = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Transform>();
        _audio = GetComponentInChildren<AudioSource>();

        _lineRenderer[0].useWorldSpace = useWorldSpace;
        _lineRenderer[1].useWorldSpace = useWorldSpace;
    }

    int num = 0;
    public void HitSphere(GameObject hitObject, int type, OVRInput.Controller controller)
    {
        if (controller == OVRInput.Controller.RTouch)
            num = 0;
        else
            num = 1;
        currentObject = hitObject;
        positionCount[num]++;
        _lineRenderer[num].positionCount = positionCount[num];
        _lineRenderer[num].SetPosition(positionCount[num] - 1, currentObject.transform.position);

        _audioTrans.position = currentObject.transform.position;
        _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        if (type == 0)
        {
            _audio.volume = 1f;
            _audio.PlayOneShot(_audio.clip);
        }
        else
        {
            _audio.volume = 0.4f;
            _audio.PlayOneShot(clip);
        }

        _manager.HitSphere(type, controller);
    }

    public void ResetLine()
    {
        currentObject = null;

        positionCount[0] = 0;
        positionCount[1] = 0;
        _lineRenderer[0].positionCount = 1;
        _lineRenderer[1].positionCount = 1;
        _lineRenderer[0].SetPosition(positionCount[0], Vector3.zero);
        _lineRenderer[1].SetPosition(positionCount[1], Vector3.zero);
    }
}
