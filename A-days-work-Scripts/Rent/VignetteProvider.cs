using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.XR.Interaction.Toolkit;

public class VignetteProvider : MonoBehaviour
{
    [SerializeField] private float intensity = 0.75f;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private Volume volume = null;
    [SerializeField] private LocomotionProvider locomotionProvider = null;

    private Vignette _vignette;

    private void Awake()
    {
        if (volume.profile.TryGet(out Vignette vignette))
        {
            _vignette = vignette;
        }
    }

    private void OnEnable()
    {
        locomotionProvider.beginLocomotion += FadeIn;
        locomotionProvider.endLocomotion += FadeOut;
    }

    private void OnDisable()
    {
        locomotionProvider.beginLocomotion -= FadeIn;
        locomotionProvider.endLocomotion -= FadeOut;
    }

    private void FadeIn(LocomotionSystem locomotionSystem) => StartCoroutine(Fade(0, intensity));

    private void FadeOut(LocomotionSystem locomotionSystem) => StartCoroutine(Fade(intensity, 0));

    private IEnumerator Fade(float beginValue, float endValue)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime <= duration)
        {
            float blend = Mathf.Clamp01(elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            float _intensity = Mathf.Lerp(beginValue, endValue, blend);
            ApplyValue(_intensity);

            // タイミングに注意．フレーム終了のタイミングまで待つ．
            yield return new WaitForEndOfFrame();
        }
    }

    private void ApplyValue(float value) => _vignette.intensity.Override(value);
}