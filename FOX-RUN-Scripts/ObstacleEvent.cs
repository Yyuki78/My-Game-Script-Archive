using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleEvent : MonoBehaviour
{
    [SerializeField] GameObject Obstacle1;
    [SerializeField] GameObject Obstacle2;
    [SerializeField] GameObject Obstacle3;
    private ObstacleDetect _ob1;
    private ObstacleDetect _ob2;
    private ObstacleDetect _ob3;
    public bool ObstacleEve = false;
    // Start is called before the first frame update
    void Start()
    {
        _ob1 = Obstacle1.GetComponent<ObstacleDetect>();
        _ob2 = Obstacle2.GetComponent<ObstacleDetect>();
        _ob3 = Obstacle3.GetComponent<ObstacleDetect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_ob1.ObstaclePass == true && _ob2.ObstaclePass == true && _ob3.ObstaclePass == true)
        {
            Debug.Log("î_ãÔÉCÉxÉìÉgÇ™ê¨å˜ÇµÇ‹ÇµÇΩ");
            ObstacleEve = true;
            GetComponent<ObstacleEvent>().enabled = false;
        }
    }
}
