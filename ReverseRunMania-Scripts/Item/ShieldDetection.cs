using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDetection : MonoBehaviour
{
    private AudioSource _audio;
    [SerializeField] AudioClip[] _clip;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.PlayOneShot(_clip[0]);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.layer = 12;
            other.gameObject.GetComponentInParent<EnemyMove>().ChangeSpeed(0);
            other.gameObject.GetComponentInParent<EnemyMove>().HitItem(0.05f);
            _audio.volume = 0.35f;
            _audio.PlayOneShot(_clip[1]);
            StartCoroutine(destroy());
        }
    }

    private IEnumerator destroy()
    {
        GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
