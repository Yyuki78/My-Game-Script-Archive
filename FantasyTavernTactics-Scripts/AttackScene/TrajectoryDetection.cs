using System.Linq;
using UnityEditor;
using UnityEngine;

public class TrajectoryDetection : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private CommanderAttack _attack;
    private Material _material;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _attack = GetComponentInParent<CommanderAttack>();
    }

    public void SetLine(Vector3 firstPos,Vector3 lastPos)
    {
        // Vector�̊p�x�擾
        Vector3 distVec = lastPos - firstPos;
        var axis = Vector3.Cross(Vector3.forward, distVec);
        var angle = Vector3.Angle(Vector3.forward, distVec) * (axis.y < 0 ? -1 : 1);

        //  Line�I�u�W�F�N�g�̌�����ݒ肷��
        transform.eulerAngles = new Vector3(0.0f, angle, 0.0f);

        // ���_�����w�肷��
        _lineRenderer.positionCount = 2;

        // LineRenderer�ɍ��W���w��
        _lineRenderer.SetPosition(0, firstPos);
        _lineRenderer.SetPosition(1, lastPos);
    }

    public void FinishAttack(Material material)
    {
        if (_lineRenderer.positionCount != 2) return;
        _material = material;
        var col = gameObject.AddComponent<BoxCollider>();
        Vector3 colSize = col.size;
        col.size = new Vector3(0.02f, colSize.y, colSize.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AttackedPieceBody"&& other.isTrigger)
        {
            //�Ō�̍U������������
            _attack.HitFinalAttack();
            _lineRenderer.material = _material;
        }
    }
}
