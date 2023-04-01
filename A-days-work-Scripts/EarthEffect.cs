using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class EarthEffect : MonoBehaviour
{
    [SerializeField] GameObject _hitEffect;
    [SerializeField] Renderer _renderer;
    [SerializeField] Material[] _changeMats;
    [SerializeField] GameObject CoverSphere;

    private XRGrabInteractable _grab;
    private Rigidbody _rigid;
    private ParticleSystem _particle;
    private ParticleSystem.EmissionModule _emission;
    private ParticleSystem.MinMaxCurve _curve;

    private bool isRotate = false;
    private float rotateSpeed = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _grab = GetComponent<XRGrabInteractable>();
        _rigid = GetComponent<Rigidbody>();
        _particle = _hitEffect.GetComponent<ParticleSystem>();
        _curve = _emission.rateOverTime;
        _emission = _particle.emission;
        _grab.enabled = false;
        _rigid.isKinematic = true;
        StartCoroutine(generateEffect());
    }

    private void Update()
    {
        if(isRotate)
            transform.Rotate(0, rotateSpeed, 0);
    }

    private IEnumerator generateEffect()
    {
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(2.5f);
        transform.localScale = new Vector3(0.4f, 0, 0.4f);

        var wait = new WaitForSeconds(0.05f);
        Vector3 scale = new Vector3(0, 0.4f / 50f, 0);
        for (int i = 0; i < 50; i++)
        {
            transform.localScale += scale;
            yield return wait;
        }
        transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        _grab.enabled = true;

        gameObject.AddComponent<Outline>();

        yield return new WaitForSeconds(1f);
        _rigid.isKinematic = false;

        yield break;
    }

    public void finish()
    {
        StartCoroutine(finishEffect());
    }

    private IEnumerator finishEffect()
    {
        //Debug.Log("’n‹…‹V‰ñ“]");
        _curve.constant = 5f;
        yield return new WaitForSeconds(0.75f);

        _hitEffect.SetActive(true);

        yield return new WaitForSeconds(3f);

        isRotate = true;

        var wait = new WaitForSeconds(0.2f);
        for (int i = 0; i < 50; i++)
        {
            rotateSpeed += 0.005f;
            _curve.constant += 0.5f;
            _emission.rateOverTime = _curve;
            yield return wait;
        }

        for (int i = 0; i < 25; i++)
        {
            rotateSpeed += 0.05f;
            _curve.constant += 0.3f;
            _emission.rateOverTime = _curve;
            yield return wait;
        }
        _renderer.material = _changeMats[0];
        for (int i = 0; i < 25; i++)
        {
            rotateSpeed -= (1.5f / 25f);
            _curve.constant -= 1.5f;
            _emission.rateOverTime = _curve;
            yield return wait;
            if (i == 10)
                _renderer.material = _changeMats[1];
            if (i == 20)
                _renderer.material = _changeMats[2];
        }
        rotateSpeed = 0;
        _curve.constant = 0f;
        _emission.rateOverTime = _curve;

        yield return new WaitForSeconds(1.5f);

        var manager = GetComponentInParent<TaskManager>();
        manager.FinishTask(2);

        yield return new WaitForSeconds(5f);

        CoverSphere.SetActive(false);
        gameObject.SetActive(false);

        yield break;
    }
}
