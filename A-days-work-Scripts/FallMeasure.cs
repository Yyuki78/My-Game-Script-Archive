using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallMeasure : MonoBehaviour
{
    [SerializeField] Fade fade;
    [SerializeField] GameObject XROrigin;
    [SerializeField] Vector3 ComeBackPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player�̈ʒu�����Z�b�g���܂�");
            XROrigin.transform.localPosition = ComeBackPosition;
            fade.changeScene(1);
        }
    }
}
