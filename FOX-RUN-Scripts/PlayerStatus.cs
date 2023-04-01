using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    /// <summary>
    /// ���C�t�ő�l��Ԃ��܂�
    /// </summary>
    public float LifeMax => lifeMax;

    /// <summary>
    /// ���C�t�̒l��Ԃ��܂�
    /// </summary>
    public float Life => _life;

    //�@LifeGauge�X�N���v�g
    [SerializeField]
    private LifeGauge lifeGauge;

    public float lifeMax = 5; // ���C�t�ő�l
    private float _life; // ���݂̃��C�t�l�i�q�b�g�|�C���g�j

    [SerializeField] GameObject gameOver;//�Q�[���I�[�o�[���
    [SerializeField] GameObject GameOverParticle;//�Q�[���I�[�o�[���̃G�t�F�N�g

    GameObject Fox; //Fox���̂��̂�����ϐ�
    MobAttack _attack; //MobAttack������ϐ�

    protected void Start()
    {
        _life = lifeMax; // ������Ԃ̓��C�t���^��
        //�@�̗̓Q�[�W�ɔ��f
        lifeGauge.SetLifeGauge(_life);

        Fox = GameObject.Find("Fox"); //Fox���I�u�W�F�N�g�̖��O����擾���ĕϐ��Ɋi�[����
        _attack = Fox.GetComponent<MobAttack>(); //Fox�̒��ɂ���MobAttack���擾���ĕϐ��Ɋi�[����
    }

    /// <summary>
    /// �w��l�̃_���[�W���󂯂܂��B
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        var source = GetComponent<Cinemachine.CinemachineImpulseSource>();
        if (_attack.attckone == false) return;
        _attack.attckone = false;
        _life -= damage;
        if (_life >= 0)
        {
            //�J������h�炷
            source.GenerateImpulse();
            lifeGauge.SetLifeGauge(_life);
        }
        if (_life > 0) return;

        //HP��0�ɂȂ����ۂ̓���
        // �Q�[���I�[�o�[��\��
        GameOverParticle.SetActive(true);
        gameOver.SetActive(true);
        GetComponent<UnityStandardAssets.Vehicles.Ball.BallUserControl>().enabled = false;
        _attack.enabled = false;
    }
}
