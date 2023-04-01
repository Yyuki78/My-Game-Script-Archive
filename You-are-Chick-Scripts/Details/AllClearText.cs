using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllClearText : MonoBehaviour
{
    [SerializeField] GameObject HeaderText;
    [SerializeField] GameObject allClearText;
    void Start()
    {
        if (GameManager.isClearStage[9])
        {
            HeaderText.SetActive(false);
            allClearText.SetActive(true);
        }
    }
}
