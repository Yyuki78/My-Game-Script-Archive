using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortTask : MonoBehaviour
{
    [SerializeField] int taskType = 0;//0�͕��^�ьn(6���),1�̓��C�g�_��,2�̓o�b�e���[�̃Z�b�g,3�̓��C���[�C��,4�̓o���u��

    [SerializeField] GameObject TransparentObj;//�A�N�e�B�u���������
    [SerializeField] GameObject SetOutlineObj;//�A�E�g���C����t�������

    [SerializeField] Material ChangeMat;//�ύX����}�e���A��

    [SerializeField] GameObject[] LightSwitches;//���C�g�̃X�C�b�`�p
    [SerializeField] MeshRenderer[] Lights;

    // Start is called before the first frame update
    void Start()
    {
        //Prepare();
    }

    public void Prepare()
    {
        switch (taskType)
        {
            case 0:
                TransparentObj.SetActive(true);
                SetOutlineObj.AddComponent<Outline>();
                break;
            case 1:
                int ran = Random.Range(0, 3);
                LightSwitches[ran].SetActive(true);
                LightSwitches[ran].AddComponent<Outline>();
                Material[] materials1 = Lights[ran].sharedMaterials;
                materials1[1] = ChangeMat;
                Lights[ran].sharedMaterials = materials1;
                break;
            case 2:
                SetOutlineObj.AddComponent<Outline>();
                var outline = TransparentObj.AddComponent<Outline>();
                outline.OutlineColor = new Color(1, 130f / 255f, 0, 123f / 255f);

                var renderer = TransparentObj.GetComponent<MeshRenderer>();
                Material[] materials2 = renderer.sharedMaterials;
                materials2[1] = ChangeMat;
                renderer.sharedMaterials = materials2;

                renderer = Lights[0].GetComponent<MeshRenderer>();
                materials2 = renderer.sharedMaterials;
                materials2[1] = Lights[1].material;
                renderer.sharedMaterials = materials2;
                break;
            case 3:
                SetOutlineObj.transform.localEulerAngles = new Vector3(0, -90, 0);
                SetOutlineObj.AddComponent<Outline>();
                break;
            case 4:
                SetOutlineObj.AddComponent<Outline>();
                break;
            default:
                Debug.Log("�^�X�N�Z�b�g�~�X�I");
                break;
        }
    }
}
