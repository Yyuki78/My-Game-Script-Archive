using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;

public class PieceGrabBehavior : MonoBehaviour
{
    public bool isGrabbed { private set; get; } = false;
    private bool isFirstGrab = false;

    [SerializeField] float moveSpeed = 5f;
    private Vector3 initialLocalPosition;
    private Vector3 initialLocalRotation;
    private Transform initialParent;
    private Transform _transform;

    private bool finalSet = true;

    private PieceInfomation _info;
    private OVRGrabbableExtended grabbable;
    private AudioSource _audio;
    [SerializeField] AudioClip clip;

    private GameSetupManager _setupManager;
    private AttributeDetermination _attributeDetermination;
    [SerializeField] MyPieceDetermination _myPieceDetermination;

    private GameInfomation _gameInfomation;
    private TurnManager _turnManager;
    private TileManager _tileManager;

    private PieceInfoDisplay _display;
    private ExplanationText _explanationText;

    private void Awake()
    {
        _info = GetComponent<PieceInfomation>();
        grabbable = GetComponent<OVRGrabbableExtended>();
        _audio = GetComponent<AudioSource>();
        _setupManager = GetComponentInParent<GameSetupManager>();
        _attributeDetermination = GetComponentInParent<AttributeDetermination>();
        _gameInfomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        _turnManager = GetComponentInParent<TurnManager>();
        _tileManager = GetComponentInParent<TileManager>();
        _display = GetComponentInParent<PieceInfoDisplay>();
        _explanationText = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplanationText>();
        _transform = GetComponent<Transform>();
        initialLocalPosition = _transform.localPosition;
        //initialLocalPosition.y = 1.432f;
        initialLocalRotation = _transform.localRotation.eulerAngles;
        initialParent = _transform.parent;
    }

    private void Update()
    {
        if (!isFirstGrab || isGrabbed || _info.isAIMoving) return;
        if (Vector3.Distance(_transform.position, initialLocalPosition) < 0.001f) return;
        if (Vector3.Distance(_transform.position, initialLocalPosition) > 0.005f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, initialLocalPosition, moveSpeed * Time.deltaTime);
            _transform.localEulerAngles = initialLocalRotation;
            if (!finalSet)
                finalSet = true;
        }
        if (Vector3.Distance(_transform.position, initialLocalPosition) < 0.005f && Vector3.Distance(_transform.position, initialLocalPosition) > 0.001f && finalSet)
        {
            finalSet = false;
            _transform.position = initialLocalPosition;
            _transform.localEulerAngles = initialLocalRotation;
        }

    }

    private void OnEnable()
    {
        // listen for grabs
        grabbable.OnGrabBegin.AddListener(OnGrabbed);
        grabbable.OnGrabEnd.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        // stop listening for grabs
        grabbable.OnGrabBegin.RemoveListener(OnGrabbed);
        grabbable.OnGrabEnd.RemoveListener(OnReleased);
    }

    private void OnReleased()
    {
        isGrabbed = false;
        _transform.SetParent(initialParent);
        _transform.localEulerAngles = initialLocalRotation;

        switch (_setupManager._currentState)
        {
            case GameSetupManager.PhaseState.DiceRoll:
                //None
                break;
            case GameSetupManager.PhaseState.AttributeDetermination:
                //ëÆê´Çïtó^Ç∑ÇÈ
                if (!_info.Side) return;
                if (_info.Role == 4)
                    _explanationText.ChangeText(2);
                break;
            case GameSetupManager.PhaseState.MyPieceDetermination:
                //é©ï™Ç≈êÌÇ§ãÓÇÃê›íË
                break;
            case GameSetupManager.PhaseState.Finish:
                //InGame
                break;
        }
    }

    private void OnGrabbed()
    {
        if (_gameInfomation.isShowAttackMessage)
        {
            StartCoroutine(cantGrab());
            return;
        }

        _audio.volume = 0.3f + Random.Range(-0.1f, 0.1f);
        _audio.pitch = 1.0f + Random.Range(-0.1f, 0.1f);
        _audio.PlayOneShot(clip);

        isGrabbed = true;
        isFirstGrab = true;

        _display.ShowPieceInfo(_info);

        switch (_setupManager._currentState)
        {
            case GameSetupManager.PhaseState.DiceRoll:
                //None
                break;
            case GameSetupManager.PhaseState.AttributeDetermination:
                //ëÆê´Çïtó^Ç∑ÇÈ
                if (!_info.Side) return;
                if (_info.alreadySetAttribute) return;
                if (_info.Role == 4)
                    _explanationText.ChangeText(3);
                else
                    _attributeDetermination.GrabPiece(_info);
                break;
            case GameSetupManager.PhaseState.ReducePiece:
                //None
                break;
            case GameSetupManager.PhaseState.MyPieceDetermination:
                //é©ï™Ç≈êÌÇ§ãÓÇÃê›íË
                if (!_info.Side) return;
                if (_info.isDie) return;
                _myPieceDetermination.GrabPiece(_info);
                break;
            case GameSetupManager.PhaseState.Finish:
                //InGame
                switch (_turnManager._currentState)
                {
                    case TurnManager.GameState.Move:
                        if (!_info.Side) return;
                        _tileManager.CheckMove(_info);
                        break;
                    case TurnManager.GameState.Attack:
                        if (!_info.Side) return;
                        _tileManager.CheckAttack(_info);
                        break;
                    default:
                        break;
                }
                break;
        }
    }

    private IEnumerator cantGrab()
    {
        yield return null;
        if (grabbable.isGrabbed)
            grabbable.grabbedBy.ForceRelease(grabbable);
        OnReleased();
        yield break;
    }

    public void UpdateReturnPosition(Vector3 pos)
    {
        initialLocalPosition = pos;
        initialLocalRotation = Vector3.zero;
        if (grabbable.isGrabbed)
        {
            grabbable.grabbedBy.ForceRelease(grabbable);
            OnReleased();
        }
    }

    public void PieceDie()
    {
        if (grabbable.isGrabbed)
        {
            grabbable.grabbedBy.ForceRelease(grabbable);
            OnReleased();
        }
        Destroy(grabbable);
    }
}
