using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerInformation : MonoBehaviour
{
    public bool isSpike = false;
    [SerializeField] GameObject Explosion;

    void Start()
    {
        
    }

    public void HitEnemy(Collider col)
    {
        if (isSpike)
        {
            isSpike = false;
            return;
        }

        AudioManager.instance.SE(18);

        Time.timeScale = 0f;

        Vector3 a_vector = new Vector3(0, 0, 0);
        float main_player_impact = 15000.0f;
        a_vector.Set(gameObject.transform.position.x - col.transform.position.x, 0.25f, gameObject.transform.position.z - col.transform.position.z);
        a_vector.Normalize();
        a_vector *= main_player_impact;

        int n = Random.Range(0, 2);
        float ran;
        if (n == 0)
            ran = Random.Range(-10f, -14f);
        else
            ran = Random.Range(10f, 14f);

        if (a_vector != Vector3.zero)
        {
            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.AddForce(a_vector, ForceMode.Impulse);
        }

        Instantiate(Explosion, col.ClosestPoint(transform.position), Quaternion.identity);
        
        GameManager.isGameOver = true;

        StartCoroutine(gameOver(ran));
    }

    public void TimeOver()
    {
        if (isSpike)
        {
            isSpike = false;
            return;
        }

        Time.timeScale = 0f;

        Vector3 a_vector = new Vector3(0, 0, 0);
        float main_player_impact = 15000.0f;
        a_vector.Set(0, 0.25f, 0);
        a_vector.Normalize();
        a_vector *= main_player_impact;

        int n = Random.Range(0, 2);
        float ran;
        if (n == 0)
            ran = Random.Range(-10f, -14f);
        else
            ran = Random.Range(10f, 14f);

        if (a_vector != Vector3.zero)
        {
            Rigidbody rigid = GetComponent<Rigidbody>();
            rigid.isKinematic = false;
            rigid.AddForce(a_vector, ForceMode.Impulse);
        }

        Instantiate(Explosion, transform.position, Quaternion.identity);

        GameManager.isGameOver = true;

        StartCoroutine(gameOver(ran));
    }

    private IEnumerator gameOver(float ran)
    {
        GetComponent<Cinemachine.CinemachineImpulseSource>().GenerateImpulse();
        for (int i = 0; i < 30; i++)
        {
            yield return null;
        }
        Time.timeScale = 0.2f;
        GameObject.FindGameObjectWithTag("Tank").GetComponent<PostEffect>().changeScene(0);
        for (int i = 0; i < 60; i++)
        {
            transform.Rotate(ran / 2f, ran, 0);
            yield return null;
        }
        yield break;
    }
}
