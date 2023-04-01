using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DokanHitDetect : MonoBehaviour
{
    public bool DokanEnemyPass = false;
    void OnTriggerStay(Collider other)
    {
        //キツネが通ったかの判定を行い、どちらもTrueの場合にゲージ上昇をする。
        if (other.gameObject.tag == "Enemy")
        {
            DokanEnemyPass = true;
            Debug.Log("土管にキツネが入りました。");
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("土管にPlayer!?");
        }
    }
}
