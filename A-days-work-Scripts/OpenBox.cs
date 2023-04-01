using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBox : MonoBehaviour
{
    [SerializeField] GameObject InputPassObj;
    [SerializeField] GameObject InsideObj;
    [SerializeField] GameObject TransparentObj;

    [SerializeField] int password;

    private Animation _anime;
    private InputPassword _pass;

    private bool isOpen = false;
    private bool coolTime = false;

    private AudioSource _audio;
    [SerializeField] AudioClip[] _clip;

    // Start is called before the first frame update
    void Start()
    {
        _anime = GetComponent<Animation>();
        _pass = GetComponentInChildren<InputPassword>();
        _audio = GetComponent<AudioSource>();
        InputPassObj.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        var outline = GetComponent<Outline>();
        if (other.CompareTag("Player") && outline != null)
        {
            if (!isOpen)
            {
                if (coolTime) return;
                isOpen = true;
                InputPassObj.SetActive(true);
                StartCoroutine(coolDown());
            }
            else
            {
                if (coolTime) return;
                isOpen = false;
                InputPassObj.SetActive(false);
                StartCoroutine(coolDown());
            }
        }
    }

    public void JudgePassword(int inputNum)
    {
        if (inputNum == password)
        {
            //Debug.Log("開錠");
            StartCoroutine(CorrectPassword());
            _audio.PlayOneShot(_clip[0]);
        }
        else
        {
            //Debug.Log("パスワードが違います");
            _pass.ResetPass();
            _audio.PlayOneShot(_clip[1]);
        }
    }

    private IEnumerator CorrectPassword()
    {
        InputPassObj.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject.GetComponent<Outline>());

        TransparentObj.SetActive(true);
        InsideObj.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        InsideObj.AddComponent<Outline>();

        _anime.Play();

        _audio.PlayOneShot(_clip[2]);

        yield break;
    }

    private IEnumerator coolDown()
    {
        if (coolTime) yield break;
        coolTime = true;
        yield return new WaitForSeconds(0.2f);
        coolTime = false;

        yield break;
    }
}