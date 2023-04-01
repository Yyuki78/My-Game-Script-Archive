using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [SerializeField] GameObject gameReady;  // GameReady �Q�[���I�u�W�F�N�g�Q��
 //   [SerializeField] RemainTimer timer;     // RemainTimer �R���|�[�l���g�Q��
    [SerializeField] GameObject gameStart;  // GameStart �Q�[���I�u�W�F�N�g�Q��
    [SerializeField] GameObject gameOver;   // GameOver �Q�[���I�u�W�F�N�g�Q��
    [SerializeField] GameObject result;     // Result �Q�[���I�u�W�F�N�g�Q��
    [SerializeField] GameObject player;     // PlayerGun �Q�[���I�u�W�F�N�g�Q��
    [SerializeField] GameObject spawners;   // Spawner �Q�[���I�u�W�F�N�g�Q��

    private PlayerStatus _status;

    // �X�e�[�g�x�[�X�N���X
    abstract class BaseState
    {
        public GameStateController Controller { get; set; }

        public enum StateAction
        {
            STATE_ACTION_WAIT,
            STATE_ACTION_NEXT
        }

        public BaseState(GameStateController c) { Controller = c; }

        public virtual void OnEnter() { }
        public virtual StateAction OnUpdate() { return StateAction.STATE_ACTION_NEXT; }
        public virtual void OnExit() { }
    }

    // �Q�[���J�n�����X�e�[�g
    class ReadyState : BaseState
    {
        float timer;

        public ReadyState(GameStateController c) : base(c) { }
        public override void OnEnter()
        {
            // ready�������\��
            Controller.gameReady.SetActive(true);
        }
        public override StateAction OnUpdate()
        {
            timer += Time.deltaTime;
            // 5�b��Ɏ���
            if (timer > 5.0f)
            {
                return StateAction.STATE_ACTION_NEXT;
            }
            return StateAction.STATE_ACTION_WAIT;
        }
        public override void OnExit()
        {
            // ready��������\��
            Controller.gameReady.SetActive(false);
        }
    }

    // �Q�[���J�n�\���X�e�[�g
    class StartState : BaseState
    {
        float timer;

        public StartState(GameStateController c) : base(c) { }
        /*
        public override void OnEnter()
        {
            // �^�C�}�[��\��
            Controller.timer.gameObject.SetActive(true);

            // start�������\��
            Controller.gameStart.SetActive(true);

            // player��\��
            Controller.player.SetActive(true);

            // spawners��\��
            Controller.spawners.SetActive(true);
        }
        */
        public override StateAction OnUpdate()
        {
            timer += Time.deltaTime;
            // 1�b��Ɏ���
            if (timer > 1.0f)
            {
                return StateAction.STATE_ACTION_NEXT;
            }
            return StateAction.STATE_ACTION_WAIT;
        }
        public override void OnExit()
        {
            // Start��������\��
            Controller.gameStart.SetActive(false);
        }
    }

    /*
    // �Q�[�����X�e�[�g
    class PlayingState : BaseState
    {
        public PlayingState(GameStateController c) : base(c) { }
        public override StateAction OnUpdate()
        {
            // Life��0�ɂȂ�����Q�[���I�[�o�[��
            if (_status._life <= 0)
            {
                return StateAction.STATE_ACTION_NEXT;
            }
            return StateAction.STATE_ACTION_WAIT;
        }

        public override void OnExit()
        {
            // �v���C���[���\��
            Controller.player.SetActive(false);

            // �G�̔������~�߂�
            Controller.spawners.SetActive(false);
        }
    }

    // �Q�[���I�[�o�[�\���X�e�[�g
    class GameOverState : BaseState
    {
        float timer;
        public GameOverState(GameStateController c) : base(c) { }
        public override void OnEnter()
        {
            // �Q�[���I�[�o�[��\��
            Controller.gameOver.SetActive(true);
        }
        public override StateAction OnUpdate()
        {
            timer += Time.deltaTime;
            // 2�b��Ɏ���
            if (timer > 2.0f)
            {
                return StateAction.STATE_ACTION_NEXT;
            }
            return StateAction.STATE_ACTION_WAIT;
        }
        public override void OnExit()
        {
            // �Q�[���I�[�o�[���\��
            Controller.gameOver.SetActive(false);
        }
    }

    // ���U���g�\���X�e�[�g
    class ResultState : BaseState
    {
        public ResultState(GameStateController c) : base(c) { }
        public override void OnEnter()
        {
            // ���U���g�\��
            Controller.result.SetActive(true);
        }
        public override StateAction OnUpdate() { return StateAction.STATE_ACTION_WAIT; }
    }
    */

    // �Q�[���̏�ԃ��X�g
    List<BaseState> state;

    // �J�����g�̃Q�[�����
    int currentState;

    void Start()
    {
        // �Q�[���̏�Ԃ����Ԃɓo�^
        state = new List<BaseState> {
            new ReadyState(this),
            new StartState(this),
            /*
            new PlayingState(this),
            new GameOverState(this),
            new ResultState(this),
            */
        };

        // �ŏ��̏�Ԃ̊J�n����
        state[currentState].OnEnter();

        _status = GetComponent<PlayerStatus>();
    }

    void Update()
    {
        // ��Ԃ��X�V
        var stateAction = state[currentState].OnUpdate();

        // ���̏�Ԃ֑J�ڂ��邩����
        if (stateAction == BaseState.StateAction.STATE_ACTION_NEXT)
        {
            // ��Ԃ̏I������
            state[currentState].OnExit();
            // ���̏�Ԃ�
            ++currentState;
            // ��Ԃ̊J�n����
            state[currentState].OnEnter();
        }
    }
}