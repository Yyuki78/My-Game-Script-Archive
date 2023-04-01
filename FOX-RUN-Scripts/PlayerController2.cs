using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3; // �ړ����x
    [SerializeField] private float jumpPower = 3; // �W�����v��
    private CharacterController _characterController; // CharacterController�̃L���b�V��
    private Transform _transform; // Transform�̃L���b�V��
    private Vector3 _moveVelocity; // �L�����̈ړ����x���

    /// <summary>
    /// �ڒn���菈��
    /// </summary>
    private bool IsGrounded
    {
        get
        {
            var ray = new Ray(_transform.position + new Vector3(0, 0.1f), Vector3.down);
            var raycastHits = new RaycastHit[1];
            var hitCount = Physics.RaycastNonAlloc(ray, raycastHits, 0.2f);
            return hitCount >= 1;
        }
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>(); // ���t���[���A�N�Z�X����̂ŁA���ׂ������邽�߂ɃL���b�V�����Ă���
        _transform = transform; // Transform���L���b�V������Ə����������ׂ�������
    }

    private void Update()
    {
        Debug.Log(IsGrounded ? "�n��ɂ��܂�" : "�󒆂ł�");

            // ���͎��ɂ��ړ������i�����𖳎����Ă���̂ŁA�L�r�L�r�����j
            _moveVelocity.x = Input.GetAxis("Horizontal") * moveSpeed;
            _moveVelocity.z = Input.GetAxis("Vertical") * moveSpeed;

            // �ړ������Ɍ���
            _transform.LookAt(_transform.position + new Vector3(_moveVelocity.x, 0, _moveVelocity.z));

        if (IsGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                // �W�����v����
                Debug.Log("�W�����v�I");
                _moveVelocity.y = jumpPower; // �W�����v�̍ۂ͏�����Ɉړ�������
            }
        }
        else
        {
            // �d�͂ɂ�����
            _moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        }

        // �I�u�W�F�N�g�𓮂���
        _characterController.Move(_moveVelocity * Time.deltaTime);
    }
}
