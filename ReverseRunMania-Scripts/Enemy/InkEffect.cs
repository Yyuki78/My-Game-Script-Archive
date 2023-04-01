using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkEffect : MonoBehaviour
{
    [SerializeField] Image[] _image;
    [SerializeField] Sprite[] inkImage;

    private Coroutine coroutine;

    void Start()
    {
        for (int i = 0; i < _image.Length; i++)
        {
            _image[i].gameObject.SetActive(false);
        }
    }

    public void InkSplash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine("inkSplash");
    }

    private IEnumerator inkSplash()
    {
        int activeRan = Random.Range(3, 6);
        float[] beforeSize = new float[6];
        Vector3[] scale = new Vector3[6];
        Vector3 rotation = new Vector3(0, 0, 1);
        for (int i = 0; i < activeRan; i++)
        {
            float posX = Random.Range(-Screen.width / 2f + 50, Screen.width / 2f - 50);
            float posY = Random.Range(-Screen.height / 2f + 50, Screen.height / 2f - 50);
            _image[i].gameObject.transform.localPosition = new Vector3(posX, posY, 0);

            float angle = Random.Range(0, 360);
            _image[i].gameObject.transform.rotation = Quaternion.AngleAxis(angle, rotation);

            int ran = Random.Range(0, 30);
            _image[i].sprite = inkImage[ran];
            _image[i].SetNativeSize();
            scale[i] = new Vector3(_image[i].transform.localScale.x / 30f, _image[i].transform.localScale.y / 30f, _image[i].transform.localScale.z / 30f);

            _image[i].gameObject.transform.localScale = new Vector3(0, 0, 0);
            _image[i].color = new Color(1, 1, 1, 1);
            _image[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < 30; i++)
        {
            for (int j = 0; j < activeRan; j++)
            {
                _image[j].gameObject.transform.localScale += scale[j];
            }
            yield return null;
        }
        AudioManager.instance.SE(13);

        yield return new WaitForSeconds(5f);

        Color color = new Color(0, 0, 0, 1f / 60f);
        Vector3 pos = new Vector3(0, 1f, 0);
        for (int i = 0; i < 60; i++)
        {
            for (int j = 0; j < activeRan; j++)
            {
                _image[j].color -= color;
                _image[j].gameObject.transform.localPosition -= pos;
            }
            yield return null;
        }

        for (int i = 0; i < _image.Length; i++)
        {
            _image[i].gameObject.SetActive(false);
        }

        yield break;
    }
}
