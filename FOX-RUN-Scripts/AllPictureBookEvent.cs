using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllPictureBookEvent : MonoBehaviour
{
    [SerializeField] private GameObject Event1;
    [SerializeField] private GameObject Event2;
    [SerializeField] private GameObject Event3;
    [SerializeField] private GameObject Event4;
    [SerializeField] private GameObject Event5;
    [SerializeField] private GameObject Event1still;
    [SerializeField] private GameObject Event2still;
    [SerializeField] private GameObject Event3still;
    [SerializeField] private GameObject Event4still;
    [SerializeField] private GameObject Event5still;
    [SerializeField] private GameObject RatEvent1;
    [SerializeField] private GameObject RatEvent2;
    [SerializeField] private GameObject RatEvent3;
    [SerializeField] private GameObject RatEvent4;
    [SerializeField] private GameObject RatEvent5;
    [SerializeField] private GameObject RatEvent6;
    [SerializeField] private GameObject RatEvent7;
    [SerializeField] private GameObject RatEvent8;
    [SerializeField] private GameObject RatEvent9;
    [SerializeField] private GameObject RatEvent10;
    [SerializeField] private GameObject RatEvent1still;
    [SerializeField] private GameObject RatEvent2still;
    [SerializeField] private GameObject RatEvent3still;
    [SerializeField] private GameObject RatEvent4still;
    [SerializeField] private GameObject RatEvent5still;
    [SerializeField] private GameObject RatEvent6still;
    [SerializeField] private GameObject RatEvent7still;
    [SerializeField] private GameObject RatEvent8still;
    [SerializeField] private GameObject RatEvent9still;
    [SerializeField] private GameObject RatEvent10still;
    // Start is called before the first frame update
    void Start()
    {
        //Event1を表示するかしないか
        if (PlayerPrefs.HasKey("Event1"))
        {
            Event1.SetActive(true);
            Event1still.SetActive(false);
        }
        else
        {
            Event1.SetActive(false);
            Event1still.SetActive(true);
        }
        //Event2を表示するかしないか
        if (PlayerPrefs.HasKey("Event2"))
        {
            Event2.SetActive(true);
            Event2still.SetActive(false);
        }
        else
        {
            Event2.SetActive(false);
            Event2still.SetActive(true);
        }
        //Event3を表示するかしないか
        if (PlayerPrefs.HasKey("Event3"))
        {
            Event3.SetActive(true);
            Event3still.SetActive(false);
        }
        else
        {
            Event3.SetActive(false);
            Event3still.SetActive(true);
        }
        //Event4を表示するかしないか
        if (PlayerPrefs.HasKey("Event4"))
        {
            Event4.SetActive(true);
            Event4still.SetActive(false);
        }
        else
        {
            Event4.SetActive(false);
            Event4still.SetActive(true);
        }
        //Event5を表示するかしないか
        if (PlayerPrefs.HasKey("Event5"))
        {
            Event5.SetActive(true);
            Event5still.SetActive(false);
        }
        else
        {
            Event5.SetActive(false);
            Event5still.SetActive(true);
        }
        //RatEvent1を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent1"))
        {
            RatEvent1.SetActive(true);
            RatEvent1still.SetActive(false);
        }
        else
        {
            RatEvent1.SetActive(false);
            RatEvent1still.SetActive(true);
        }
        //RatEvent2を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent2"))
        {
            RatEvent2.SetActive(true);
            RatEvent2still.SetActive(false);
        }
        else
        {
            RatEvent2.SetActive(false);
            RatEvent2still.SetActive(true);
        }
        //RatEvent3を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent3"))
        {
            RatEvent3.SetActive(true);
            RatEvent3still.SetActive(false);
        }
        else
        {
            RatEvent3.SetActive(false);
            RatEvent3still.SetActive(true);
        }
        //RatEvent4を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent4"))
        {
            RatEvent4.SetActive(true);
            RatEvent4still.SetActive(false);
        }
        else
        {
            RatEvent4.SetActive(false);
            RatEvent4still.SetActive(true);
        }
        //RatEvent5を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent5"))
        {
            RatEvent5.SetActive(true);
            RatEvent5still.SetActive(false);
        }
        else
        {
            RatEvent5.SetActive(false);
            RatEvent5still.SetActive(true);
        }
        //RatEvent6を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent6"))
        {
            RatEvent6.SetActive(true);
            RatEvent6still.SetActive(false);
        }
        else
        {
            RatEvent6.SetActive(false);
            RatEvent6still.SetActive(true);
        }
        //RatEvent7を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent7"))
        {
            RatEvent7.SetActive(true);
            RatEvent7still.SetActive(false);
        }
        else
        {
            RatEvent7.SetActive(false);
            RatEvent7still.SetActive(true);
        }
        //RatEvent8を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent8"))
        {
            RatEvent8.SetActive(true);
            RatEvent8still.SetActive(false);
        }
        else
        {
            RatEvent8.SetActive(false);
            RatEvent8still.SetActive(true);
        }
        //RatEvent9を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent9"))
        {
            RatEvent9.SetActive(true);
            RatEvent9still.SetActive(false);
        }
        else
        {
            RatEvent9.SetActive(false);
            RatEvent9still.SetActive(true);
        }
        //RatEvent10を表示するかしないか
        if (PlayerPrefs.HasKey("RatEvent10"))
        {
            RatEvent10.SetActive(true);
            RatEvent10still.SetActive(false);
        }
        else
        {
            RatEvent10.SetActive(false);
            RatEvent10still.SetActive(true);
        }
    }
}
