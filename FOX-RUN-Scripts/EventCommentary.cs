using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCommentary : MonoBehaviour
{
    [SerializeField] private Button Eventbutton1;
    [SerializeField] private Button Eventbutton2;
    [SerializeField] private Button Eventbutton3;
    [SerializeField] private Button Eventbutton4;
    [SerializeField] private Button Eventbutton5;
    [SerializeField] private GameObject EventPanel1;
    [SerializeField] private GameObject EventPanel2;
    [SerializeField] private GameObject EventPanel3;
    [SerializeField] private GameObject EventPanel4;
    [SerializeField] private GameObject EventPanel5;
    [SerializeField] private Button Resumebutton;
    // Start is called before the first frame update
    void Start()
    {
        EventPanel1.SetActive(false);
        EventPanel2.SetActive(false);
        EventPanel3.SetActive(false);
        EventPanel4.SetActive(false);
        EventPanel5.SetActive(false);
        Eventbutton1.onClick.AddListener(EP1);
        Eventbutton2.onClick.AddListener(EP2);
        Eventbutton3.onClick.AddListener(EP3);
        Eventbutton4.onClick.AddListener(EP4);
        Eventbutton5.onClick.AddListener(EP5);
        Resumebutton.onClick.AddListener(Resume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EP1()
    {
        EventPanel1.SetActive(true);
        EventPanel2.SetActive(false);
        EventPanel3.SetActive(false);
        EventPanel4.SetActive(false);
        EventPanel5.SetActive(false);
    }

    private void EP2()
    {
        EventPanel1.SetActive(false);
        EventPanel2.SetActive(true);
        EventPanel3.SetActive(false);
        EventPanel4.SetActive(false);
        EventPanel5.SetActive(false);
    }

    private void EP3()
    {
        EventPanel1.SetActive(false);
        EventPanel2.SetActive(false);
        EventPanel3.SetActive(true);
        EventPanel4.SetActive(false);
        EventPanel5.SetActive(false);
    }

    private void EP4()
    {
        EventPanel1.SetActive(false);
        EventPanel2.SetActive(false);
        EventPanel3.SetActive(false);
        EventPanel4.SetActive(true);
        EventPanel5.SetActive(false);
    }

    private void EP5()
    {
        EventPanel1.SetActive(false);
        EventPanel2.SetActive(false);
        EventPanel3.SetActive(false);
        EventPanel4.SetActive(false);
        EventPanel5.SetActive(true);
    }

    private void Resume()
    {
        EventPanel1.SetActive(false);
        EventPanel2.SetActive(false);
        EventPanel3.SetActive(false);
        EventPanel4.SetActive(false);
        EventPanel5.SetActive(false);
    }
}
