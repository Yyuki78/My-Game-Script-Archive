using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyStatus))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] private LayerMask raycastLayerMask; // ���C���[�}�X�N

    private NavMeshAgent _agent;
    private RaycastHit[] _raycastHits = new RaycastHit[10];
    private EnemyStatus _status;
    [SerializeField] GameObject Ball;
    Rigidbody _rigidbody;

    //����
    public AudioClip sound1;
    AudioSource _audioSource;

    //�����Q�[�W
    public bool StartGauge = false;
    public bool reduce = false;
    public Slider slider;
    public float fill;
    //�����Q�[�W�������Ă��邩�ǂ�����bool
    public bool plus = false;
    [SerializeField] GameObject _foxGauge;

    [SerializeField] GameObject gameClear;//�Q�[���N���A���
    [SerializeField] GameObject GameClearParticle;//�Q�[���N���A���̃G�t�F�N�g
    public GameObject ball;//Fox��Collider

    public bool hukan = false;//�J�����̘��Վ��_�ϊ��p

    public bool clear = false;//�Q�[���N���A���ɑ��̃X�N���v�g���~�߂�悤��bool

    private FootCollisionDetector _footcol;//���Ք���p�̃X�N���v�g

    //�y�ǃC�x���g�p�̕ϐ�
    [SerializeField] GameObject _DokanDetect;
    private DokanEvent _DokanEve;
    public bool DokanTrue = false;//�|�b�v�A�b�v�e�L�X�g��\�����邽�߂̊֐�
    private bool once = false;

    //�l�Y�~�C�x���g�p�̕ϐ�
    [SerializeField] GameObject Rat;
    private RatEvent RatEve;
    public bool RatTrue = false;//�|�b�v�A�b�v�e�L�X�g��\�����邽�߂̊֐�
    private bool once2 = false;

    //�_��C�x���g�p�̕ϐ�
    [SerializeField] ObstacleEvent ObsEve;
    public bool ObsTrue = false;
    private bool once3 = false;

    //�U���������ɃQ�[�W���㏸�����邽�߂̕ϐ�
    //BGMControl�ɂĎ���(fill���������Ă�)

    private bool Sitonce = false;//���邽�߂�bool

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>(); // NavMeshAgent��ێ����Ă���
        _status = GetComponent<EnemyStatus>();
        _rigidbody = Ball.GetComponent<Rigidbody>();

        _audioSource = GetComponent<AudioSource>();
        fill = 0;
        slider.value = 0;

        _footcol = Ball.GetComponent<FootCollisionDetector>();

        _DokanEve = _DokanDetect.GetComponent<DokanEvent>();

        RatEve = Rat.GetComponent<RatEvent>();
    }

    private void Update()
    {
        if (fill < 0)
        {
            fill = 0;
        }
        
        //�͈͊O�̎��Ɏ~�܂�
        if (_agent.remainingDistance >= 5f)
        {
            _agent.isStopped = true;
        }

        if (reduce == true && fill > 0 && plus == false)
        {
            //�͈͊O�Ȃ̂ŃQ�[�W�����炷
            fill -= 0.1f;
        }

        //�����𖞂���������Վ��_�ɂȂ�
        if (StartGauge == true && _agent.velocity.magnitude == 0 && Input.anyKey == false && _rigidbody.velocity.magnitude <= 2 && _status.IsSittable)
        {
            //CameraControll
            hukan = true;
        }
        else
        {
            hukan = false;
        }

        //3�b�ȏ�L�c�l�������Ă��Ȃ��Ȃ����
        if (_agent.velocity.magnitude == 0)
        {
            if (Sitonce == false)
            {
                StartCoroutine(SitCoroutine());
                Sitonce = true;
            }
        }

        /*
        if (_foxGauge.GetComponent<FoxGauge>().reduse == true)
        {
           fill -= 0.1f;
        }*/

        //�y�ǃC�x���g�����������Ƃ��ɃQ�[�W���㏸������
        if (_DokanEve.DokanEventBool == true && once == false)
        {
            //DokanTrue = true;
            once = true;
            fill += 50;
        }
        /*
        if (once == true)
        {
            DokanTrue = false;
        }*/

        //�l�Y�~�C�x���g�����������Ƃ��ɃQ�[�W���㏸������
        if (once2 == true)
        {
            RatTrue = false;
            once2 = false;
        }
        if (RatEve.Rat1Eve == true || RatEve.Rat2Eve == true || RatEve.Rat3Eve == true || RatEve.Rat4Eve == true || RatEve.Rat5Eve == true || RatEve.Rat6Eve == true || RatEve.Rat7Eve == true || RatEve.Rat8Eve == true || RatEve.Rat9Eve == true || RatEve.Rat10Eve == true)
        {
            if (once2 == false)
            {
                RatTrue = true;
                once2 = true;
                fill += 10;
                Debug.Log("10�_�����Z����܂�");
            }
        }

        //�_��C�x���g�����������Ƃ��ɃQ�[�W���㏸������
        if (ObsEve.ObstacleEve == true && once3 == false)
        {
            ObsTrue = true;
            once = true;
            fill += 100;
        }

        //�Q�[�W�̕\��
        slider.value = fill * 0.1f;
    }

    //SoundDetector��onTriggerEnter�ɃZ�b�g���A�Փ˔�����󂯎�郁�\�b�h
    public void OnReduceOffObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //�v���C���[�����m�͈͓��ɓ������̂ŃQ�[�W�������~�߂�
            reduce = false;
        }
    }

    //SoundDetector��onTriggerStay�ɃZ�b�g���A�Փ˔�����󂯎�郁�\�b�h
    public void OnSoundObject(Collider collider)
    {
        // ���m�����I�u�W�F�N�g�ɁuPlayer�v�̃^�O�����Ă���+�L�c�l�̌��m�͈͓��Ɉ�x�ł������Ă���{�L�c�l�������Ă���+���Ղ̏�łȂ��Ȃ�Q�[�W�𑝂₷
        if (collider.CompareTag("Player") && StartGauge == true && _agent.velocity.magnitude != 0 && _footcol.FootGaugeCol == false)
        {
            //�͈͓��Ȃ̂ŃQ�[�W�𑝂₷
            fill += 0.1f;
            plus = true;
        }
        else
        {
           plus = false;
        }
    }

    //SoundDetector��onTriggerExit�ɃZ�b�g���A�Փ˔�����󂯎�郁�\�b�h
    public void OnReduceObject(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            //�v���C���[�����m�͈͓��Ɍ��m�͈͊O�ɏo���̂ŃQ�[�W�����炷
            reduce = true;
            plus = false;
        }
    }

    // CollisionDetector��onTriggerStay�ɃZ�b�g���A�Փ˔�����󂯎�郁�\�b�h
    public void OnDetectObject(Collider collider)
    {
        StartGauge = true;
        if (slider.value >= 50)
        {
            if (Ball.GetComponent<PlayerStatus>().Life <= 0) return;
            //�����Q�[�W���ő�ɂȂ����ۂ̓���
            // �Q�[���N���A��\��
            gameClear.SetActive(true);
            GameClearParticle.SetActive(true);
            GetComponent<MobAttack>().enabled = false;
            GetComponent<EnemyStatus>().enabled = false;
            ball.SetActive(false);
            _status.Clear();
            clear = true;
            GetComponent<EnemyMove>().enabled = false;
        }
        if (collider.CompareTag("Player"))
        {
            if (_agent.isStopped == true)
            {
                Sitonce = false;
                _status.StandUp();
            }
        }

        if (!_status.IsMovable)
        {
            _agent.isStopped = true;
            return;
        }

        // ���m�����I�u�W�F�N�g�ɁuPlayer�v�̃^�O�����Ă���΁A���̃I�u�W�F�N�g��ǂ�������
        if (collider.CompareTag("Player"))
        {
            var positionDiff = collider.transform.position - transform.position; // ���g�ƃv���C���[�̍��W�������v�Z
            var distance = positionDiff.magnitude; // �v���C���[�Ƃ̋������v�Z
            var direction = positionDiff.normalized; // �v���C���[�ւ̕���

            // _raycastHits�ɁA�q�b�g����Collider����W���Ȃǂ��i�[�����
            // RaycastAll��RaycastNonAlloc�͓����̋@�\�����ARaycastNonAlloc���ƃ������ɃS�~���c��Ȃ��̂ł�����𐄏�
            var hitCount = Physics.RaycastNonAlloc(transform.position, direction, _raycastHits, distance, raycastLayerMask);
        //    Debug.Log("hitCount: " + hitCount);
            if (hitCount == 0)
            {
                // �{��̃v���C���[��CharacterController���g���Ă��āACollider�͎g���Ă��Ȃ��̂�Raycast�̓q�b�g���Ȃ�
                // �܂�A�q�b�g����0�ł���΃v���C���[�Ƃ̊Ԃɏ�Q���������Ƃ������ƂɂȂ�
                _agent.isStopped = false;
                _agent.destination = collider.transform.position;
            }
            else
            {
                // �����������~����
                _agent.isStopped = true;
            }
        }
    }

    private IEnumerator SitCoroutine()
    {
        yield return new WaitForSeconds(3);
        _status.Sit();
        Sitonce = false;
    }
}