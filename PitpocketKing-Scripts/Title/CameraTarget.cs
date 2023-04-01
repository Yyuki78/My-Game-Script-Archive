using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTarget : MonoBehaviour
{
    private GameObject[] NPCs;

    private CinemachineVirtualCamera virtualCamera;

    // Start is called before the first frame update
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        StartCoroutine(changeTarget());
    }

    private IEnumerator changeTarget()
    {
        yield return new WaitForSeconds(0.1f);
        NPCs = GameObject.FindGameObjectsWithTag("NPC");

        while (true)
        {
            int ran = Random.Range(0, NPCs.Length);
            virtualCamera.Follow = NPCs[ran].transform; // 追従対象
            virtualCamera.LookAt = NPCs[ran].transform; // 照準合わせ対象
            yield return new WaitForSeconds(10f);
        }
    }
}
