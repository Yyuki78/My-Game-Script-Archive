using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class PictureBookButton : MonoBehaviour
{
    [SerializeField] private Button Nextbutton1;
    [SerializeField] private Button Nextbutton2;
    [SerializeField] private Button Backbutton1;
    [SerializeField] private Button Backbutton2;
    [SerializeField] private GameObject BookPanel1;
    [SerializeField] private GameObject BookPanel2;
    [SerializeField] private GameObject BookPanel3;
    // Start is called before the first frame update
    void Start()
    {
        BookPanel1.SetActive(true);
        BookPanel2.SetActive(false);
        BookPanel3.SetActive(false);
        Nextbutton1.onClick.AddListener(GoPage2);
        Nextbutton2.onClick.AddListener(GoPage3);
        Backbutton1.onClick.AddListener(GoPage1);
        Backbutton2.onClick.AddListener(GoPage2);
    }

    private void GoPage1()
    {
        BookPanel1.SetActive(true);
        BookPanel2.SetActive(false);
        BookPanel3.SetActive(false);
    }

    private void GoPage2()
    {
        BookPanel1.SetActive(false);
        BookPanel2.SetActive(true);
        BookPanel3.SetActive(false);
    }

    private void GoPage3()
    {
        BookPanel1.SetActive(false);
        BookPanel2.SetActive(false);
        BookPanel3.SetActive(true);
    }
}
