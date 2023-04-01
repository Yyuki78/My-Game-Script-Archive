using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TrashTaskDetection : MonoBehaviour
{
    [SerializeField] int TaskType = 0;//0はゴミ,1はドラム缶
    [SerializeField] string TagNum;
    [SerializeField] Material ChangeMat;

    private MeshRenderer _mesh;
    private int trashNum = 0;

    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        _audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagNum))
        {
            var outline = other.gameObject.GetComponent<Outline>();
            if (outline == null)
                return;

            switch (TaskType)
            {
                case 0:
                    Destroy(other.gameObject.GetComponent<Outline>());

                    Destroy(other.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.gameObject.GetComponent<Rigidbody>());

                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;

                    StartCoroutine(DestroyTrash(other.gameObject));

                    trashNum++;

                    _audio.PlayOneShot(_audio.clip);

                    if (trashNum == 3)
                    {
                        Material[] materials = _mesh.sharedMaterials;
                        materials[0] = ChangeMat;
                        _mesh.sharedMaterials = materials;

                        var _manager = GetComponentInParent<TaskManager>();
                        _manager.FinishTask(1);
                    }
                    break;
                case 1:
                    var manager = GetComponentInParent<BarrelTask>();
                    manager.SetBarrel();

                    Destroy(other.gameObject.GetComponent<Outline>());

                    Destroy(other.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.gameObject.GetComponent<Rigidbody>());

                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;

                    this.gameObject.SetActive(false);
                    break;
                default:
                    Debug.Log("タスクタイプが違います");
                    break;
            }
        }
    }

    private IEnumerator DestroyTrash(GameObject other)
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 scale = new Vector3(0, 0.05f, 0);
        var wait = new WaitForSeconds(0.1f);
        for (int i = 0; i < 20; i++)
        {
            other.transform.localScale -= scale;
            yield return wait;
        }
        other.transform.localScale = new Vector3(0, 0, 0);

        if (trashNum == 3)
        {
            yield return new WaitForSeconds(0.5f);
            this.gameObject.GetComponent<MeshRenderer>().enabled = false;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        yield break;
    }
}