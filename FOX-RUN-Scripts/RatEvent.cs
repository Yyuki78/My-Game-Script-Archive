using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatEvent : MonoBehaviour
{
    [SerializeField] GameObject Rat1;
    [SerializeField] GameObject Rat2;
    [SerializeField] GameObject Rat3;
    [SerializeField] GameObject Rat4;
    [SerializeField] GameObject Rat5;
    [SerializeField] GameObject Rat6;
    [SerializeField] GameObject Rat7;
    [SerializeField] GameObject Rat8;
    [SerializeField] GameObject Rat9;
    [SerializeField] GameObject Rat10;
    private RatDetect R1;
    private RatDetect R2;
    private RatDetect R3;
    private RatDetect R4;
    private RatDetect R5;
    private RatDetect R6;
    private RatDetect R7;
    private RatDetect R8;
    private RatDetect R9;
    private RatDetect R10;
    public bool Rat1Eve = false;
    public bool Rat2Eve = false;
    public bool Rat3Eve = false;
    public bool Rat4Eve = false;
    public bool Rat5Eve = false;
    public bool Rat6Eve = false;
    public bool Rat7Eve = false;
    public bool Rat8Eve = false;
    public bool Rat9Eve = false;
    public bool Rat10Eve = false;
    private bool once = false;
    // Start is called before the first frame update
    void Start()
    {
        R1 = Rat1.GetComponent<RatDetect>();
        R2 = Rat2.GetComponent<RatDetect>();
        R3 = Rat3.GetComponent<RatDetect>();
        R4 = Rat4.GetComponent<RatDetect>();
        R5 = Rat5.GetComponent<RatDetect>();
        R6 = Rat6.GetComponent<RatDetect>();
        R7 = Rat7.GetComponent<RatDetect>();
        R8 = Rat8.GetComponent<RatDetect>();
        R9 = Rat9.GetComponent<RatDetect>();
        R10 = Rat10.GetComponent<RatDetect>();
    }

    // Update is called once per frame
    void Update()
    {
        //ÇªÇÍÇºÇÍÇÃRatéÊìæÇ…âûÇ∂ÇƒPlayerprefsÇ≈ê}ä”ñÑÇﬂÇÇ∑ÇÈ
        if (once == true)
        {
            once = false;
            Rat1Eve = false;
            Rat2Eve = false;
            Rat3Eve = false;
            Rat4Eve = false;
            Rat5Eve = false;
            Rat6Eve = false;
            Rat7Eve = false;
            Rat8Eve = false;
            Rat9Eve = false;
            Rat10Eve = false;
        }

        if (R1.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent1", 1);
            Rat1.SetActive(false);
            R1.RatPass = false;
            Rat1Eve = true;
            once = true;
        }
        if (R2.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent2", 1);
            Rat2.SetActive(false);
            R2.RatPass = false;
            Rat2Eve = true;
            once = true;
        }
        if (R3.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent3", 1);
            Rat3.SetActive(false);
            R3.RatPass = false;
            Rat3Eve = true;
            once = true;
        }
        if (R4.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent4", 1);
            Rat4.SetActive(false);
            R4.RatPass = false;
            Rat4Eve = true;
            once = true;
        }
        if (R5.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent5", 1);
            Rat5.SetActive(false);
            R5.RatPass = false;
            Rat5Eve = true;
            once = true;
        }
        if (R6.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent6", 1);
            Rat6.SetActive(false);
            R6.RatPass = false;
            Rat6Eve = true;
            once = true;
        }
        if (R7.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent7", 1);
            Rat7.SetActive(false);
            R7.RatPass = false;
            Rat7Eve = true;
            once = true;
        }
        if (R8.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent8", 1);
            Rat8.SetActive(false);
            R8.RatPass = false;
            Rat8Eve = true;
            once = true;
        }
        if (R9.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent9", 1);
            Rat9.SetActive(false);
            R9.RatPass = false;
            Rat9Eve = true;
            once = true;
        }
        if (R10.RatPass == true && once == false)
        {
            PlayerPrefs.SetInt("RatEvent10", 1);
            Rat10.SetActive(false);
            R10.RatPass = false;
            Rat10Eve = true;
            once = true;
        }
    }
}
