using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TaskObjectDetection : MonoBehaviour
{
    [SerializeField] int TaskType = 0;//0は何もなし,1はバッテリーの色付け,2はワイヤ修理,3はライト点灯
    [SerializeField] int TaskDifficult = 0;//0=short,1=middle,2=long
    [SerializeField] int TaskNum = 0;//Taskごとの番号
    [SerializeField] string TagNum;

    [SerializeField] GameObject otherEffectObj;//タスク完了で何か変わるもの用
    [SerializeField] Material ChangeMat;

    private AudioSource _audio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("hit!");
        if (other.tag == TagNum)
        {
            var manager = GetComponentInParent<TaskManager>();
            switch (TaskType)
            {
                case 0:
                    manager.FinishTask(TaskDifficult);
                    manager.FinishTaskNumber(TaskNum);

                    Destroy(other.gameObject.GetComponent<Outline>());

                    Destroy(other.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.gameObject.GetComponent<Rigidbody>());

                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;
                    break;
                case 1:
                    var outline = other.gameObject.GetComponent<Outline>();
                    if (outline == null)
                        return;

                    manager.FinishTask(TaskDifficult);
                    manager.FinishTaskNumber(TaskNum);

                    Destroy(other.gameObject.GetComponent<Outline>());

                    Destroy(other.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.gameObject.GetComponent<Rigidbody>());


                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;

                    var renderer = otherEffectObj.GetComponent<MeshRenderer>();
                    Material[] materials = renderer.sharedMaterials;
                    materials[1] = ChangeMat;
                    renderer.sharedMaterials = materials;
                    break;
                case 2:
                    var outline2 = GetComponent<Outline>();
                    if (outline2 == null)
                        return;

                    manager.FinishTask(TaskDifficult);
                    manager.FinishTaskNumber(TaskNum);

                    Destroy(other.gameObject.GetComponent<Outline>());

                    transform.localEulerAngles = new Vector3(0, -180, 0);
                    Destroy(GetComponent<Outline>());

                    _audio = GetComponent<AudioSource>();
                    if (_audio != null)
                        _audio.Play();
                    _audio = null;
                    return;
                case 3:
                    manager.FinishTask(TaskDifficult);
                    manager.FinishTaskNumber(TaskNum);

                    Destroy(other.gameObject.GetComponent<Outline>());

                    var renderer2 = otherEffectObj.GetComponent<MeshRenderer>();
                    Material[] materials2 = renderer2.sharedMaterials;
                    materials2[1] = ChangeMat;
                    renderer2.sharedMaterials = materials2;

                    _audio = GetComponent<AudioSource>();
                    if (_audio != null)
                        _audio.Play();
                    _audio = null;
                    break;
                case 4:
                    var outline3 = other.gameObject.GetComponent<Outline>();
                    if (outline3 == null)
                        return;

                    manager.FinishTask(TaskDifficult);

                    Destroy(other.gameObject.GetComponent<Outline>());

                    Destroy(other.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.gameObject.GetComponent<Rigidbody>());
                    
                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;

                    otherEffectObj.SetActive(true);
                    break;
                case 5:
                    Destroy(other.gameObject.GetComponent<Outline>());
                    Destroy(other.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.gameObject.GetComponent<Rigidbody>());

                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;

                    var b = GetComponentInParent<BarrelTask>();
                    b.SetBarrel();
                    break;
                case 6:
                    Destroy(other.gameObject.GetComponent<Outline>());
                    Destroy(other.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.gameObject.GetComponent<Rigidbody>());

                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;

                    otherEffectObj.SetActive(true);

                    var t = GetComponentInParent<ArtTask>();
                    t.SetArt();
                    break;
                case 7:
                    Destroy(other.gameObject.GetComponent<Outline>());
                    other.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                    other.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;

                    otherEffectObj.SetActive(true);

                    var k = GetComponentInParent<KeyOrChargeTask>();
                    k.SetSomething();
                    break;
                case 8:
                    Destroy(other.gameObject.GetComponent<Outline>());
                    Destroy(other.gameObject.GetComponent<XRGrabInteractable>());
                    Destroy(other.gameObject.GetComponent<Rigidbody>());

                    other.gameObject.transform.position = transform.position;
                    other.gameObject.transform.rotation = transform.rotation;

                    otherEffectObj.SetActive(true);

                    var earth = other.gameObject.GetComponent<EarthEffect>();
                    earth.finish();
                    break;
                case 9:
                    otherEffectObj.SetActive(true);
                    break;
                default:
                    Debug.Log("タスクタイプが違います");
                    break;
            }

            _audio = other.gameObject.GetComponent<AudioSource>();
            if (_audio != null)
                _audio.Play();
            _audio = null;

            this.gameObject.SetActive(false);
        }
    }
}
