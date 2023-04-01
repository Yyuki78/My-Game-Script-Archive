using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] GameObject Explosion;
    private MeshRenderer _mesh;
    private Rigidbody _rigid;

    void Start()
    {
        _mesh = GetComponentInChildren<MeshRenderer>();
        _rigid = GetComponent<Rigidbody>();
        _rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
        Invoke("Delete", 10f);
    }

    private void FixedUpdate()
    {
        _rigid.AddForce(transform.forward * 15, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.GetComponent<BoxCollider>().enabled = false;
            other.gameObject.layer = 12;
            StartCoroutine(hitEffect(other.gameObject));
        }
    }

    private IEnumerator hitEffect(GameObject car)
    {
        car.GetComponentInParent<EnemyMove>().ChangeSpeed(0);
        _mesh.enabled = false;
        //爆発
        Instantiate(Explosion, transform.position, Quaternion.identity);

        //車を吹っ飛ばす
        car.GetComponentInParent<EnemyMove>().HitItem(0.05f);

        yield return null;

        Delete();
        yield break;
    }

    private void Delete()
    {
        Destroy(gameObject);
    }
}
