using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{

    public GameObject mainCamera;      //メインカメラ格納用
    public GameObject subCamera1;       //サブカメラ1格納用 
    public GameObject subCamera2;       //サブカメラ2格納用 

    //カメラ切り替えを一度しか行わないためのbool
    private bool once = false; //俯瞰視点かどうか
    private bool once2 = false; //俯瞰視点かどうか

    GameObject Fox; //Foxそのものが入る変数
    EnemyMove _move; //EnemyMoveが入る変数

    //呼び出し時に実行される関数
    void Start()
    {
        //メインカメラとサブカメラをそれぞれ取得
        mainCamera = GameObject.Find("CM vcam1");
        subCamera1 = GameObject.Find("CM vcam2 back");
        subCamera2 = GameObject.Find("CM vcam3 fukan");

        //サブカメラを非アクティブにする
        subCamera1.SetActive(false);
        subCamera2.SetActive(false);

        Fox = GameObject.Find("Fox"); //Foxをオブジェクトの名前から取得して変数に格納する
        _move = Fox.GetComponent<EnemyMove>(); //Foxの中にあるEnemyMoveを取得して変数に格納する
    }


    //単位時間ごとに実行される関数
    void Update()
    {
        if (_move.hukan == true)
        {
            StartCoroutine("ChangeHukan");
            Debug.Log("俯瞰視点");
        }

        if (_move.hukan == false && once2 == false)
        {
            StopCoroutine("ChangeHukan");
            //メインカメラをアクティブに設定
            subCamera2.SetActive(false);
            subCamera1.SetActive(false);
            mainCamera.SetActive(true);
            once = true;
            once2 = true;
        }

        //Nキーが押されている間、サブカメラをアクティブにする
        if (Input.GetKeyDown(KeyCode.N))
        {
            //サブカメラをアクティブに設定
            mainCamera.SetActive(false);
            subCamera1.SetActive(true);
            subCamera2.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            //メインカメラをアクティブに設定
            subCamera2.SetActive(false);
            subCamera1.SetActive(false);
            mainCamera.SetActive(true);
        }

        /*
        if (Input.GetKeyDown(KeyCode.M))
        {
            mainCamera.SetActive(false);
            subCamera1.SetActive(false);
            subCamera2.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            //メインカメラをアクティブに設定
            subCamera2.SetActive(false);
            subCamera1.SetActive(false);
            mainCamera.SetActive(true);
        }
        */
    }

    IEnumerator ChangeHukan()
    {
        once2 = false;
        if (_move.hukan == false) yield break;
        if (once == false) yield break;
        yield return new WaitForSeconds(1);
        if (_move.hukan == false) yield break;
        //俯瞰視点に移行します
        mainCamera.SetActive(false);
        subCamera1.SetActive(false);
        subCamera2.SetActive(true);
        once = false;
    }
}