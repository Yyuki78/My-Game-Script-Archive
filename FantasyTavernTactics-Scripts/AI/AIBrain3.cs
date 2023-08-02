using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AIBrain3 : MonoBehaviour, IAI
{
    private int BoardSize = 0;

    private MeshRenderer CupRenderer;
    private TextMeshProUGUI _moveNum;

    private bool isRandom1 = false;
    private bool isRandom2 = false;

    private GameInfomation _infomation;
    private TurnManager _manager;
    private TileManager _tileManager;

    private TileInfomation[] _tiles;
    private PieceInfomation[] _allPieceInfos;
    private PieceInfomation _movePieceInfo;
    private PieceAnimation[] _allPieceAnimes;
    private PieceAnimation _pieceAnimes;
    private int TileNumber;

    private List<PieceInfomation> _CanMovePiece = new List<PieceInfomation>();//�ړ�������\���̂����
    private List<PieceInfomation> _CanAttackPiece = new List<PieceInfomation>();//���̏�ōU���ł����
    private List<PieceInfomation> _DisAdvPiece = new List<PieceInfomation>();//���̏�ōU���ł��邪�����s���ȋ�

    private float moveEvaluate;
    private List<float> _MoveEvaluateValue = new List<float>();//�ړ���̕]���l
    private List<PieceInfomation> _MoveEvaluatePiece = new List<PieceInfomation>();//�]���l�̌�(����)
    private List<int> _MoveEvaluatePosition = new List<int>();//�]���l�̌�(�ړ���)

    //�ȉ��͎g���܂킷
    private List<int> _MoveDestination = new List<int>();
    private List<int> _NextMoveDestination = new List<int>();//���ړ��̍l���p
    private List<int> _AllPiecePosition = new List<int>();
    private List<int> _PiecePosition = new List<int>();
    private List<int> _AttackRangePosition = new List<int>();
    private List<int> _DamagePredictionList = new List<int>();

    void Awake()
    {
        _infomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        _manager = GetComponent<TurnManager>();
        _tileManager = GetComponent<TileManager>();

        BoardSize = _infomation.BoardSize;
        switch (BoardSize)
        {
            case 5:
                _tiles = new TileInfomation[25];
                _allPieceInfos = new PieceInfomation[14];
                _allPieceAnimes = new PieceAnimation[14];
                break;
            case 6:
                _tiles = new TileInfomation[36];
                _allPieceInfos = new PieceInfomation[18];
                _allPieceAnimes = new PieceAnimation[18];
                break;
            case 7:
                _tiles = new TileInfomation[49];
                _allPieceInfos = new PieceInfomation[22];
                _allPieceAnimes = new PieceAnimation[22];
                break;
        }
        _tiles = GetComponentsInChildren<TileInfomation>();
        _allPieceInfos = GetComponentsInChildren<PieceInfomation>();
        _allPieceAnimes = GetComponentsInChildren<PieceAnimation>();

        CupRenderer = GameObject.FindGameObjectWithTag("AIcup").GetComponent<MeshRenderer>();
        _moveNum = GameObject.FindGameObjectWithTag("AImovetext").GetComponent<TextMeshProUGUI>();
        CupRenderer.enabled = false;
        _moveNum.gameObject.SetActive(false);
    }

    public void StartMoveCountSelection()
    {
        StartCoroutine(DecideTurnMoveNum());
    }

    Vector3 rotate = new Vector3(1800f, 0, 90f);
    WaitForSeconds wait = new WaitForSeconds(1.5f);
    private IEnumerator DecideTurnMoveNum()
    {
        CupRenderer.enabled = true;
        CupRenderer.transform.DOLocalRotate(rotate, 1.5f, RotateMode.FastBeyond360).SetEase(Ease.InOutQuint);
        yield return wait;

        //�m�����������20%,40%,40%
        int ran = UnityEngine.Random.Range(0, 10);
        if (ran < 2)
            ran = 1;
        else if (ran < 6)
            ran = 2;
        else
            ran = 3;
        _moveNum.text = ran.ToString();
        _moveNum.gameObject.SetActive(true);

        yield return null;
        CupRenderer.enabled = false;
        yield return wait;
        _moveNum.gameObject.SetActive(false);
        _moveNum.text = " ";

        _manager.DecideMoveNum(ran);
        StartCoroutine(AIMovement());
        yield break;
    }

    private IEnumerator AIMovement()
    {
        while (_manager.MoveCount > 0)
        {
            int ran = UnityEngine.Random.Range(0, 30);
            if (ran == 0 || ran == 1)
                isRandom1 = true;
            else
                isRandom1 = false;
            if (ran == 1 || ran == 2 || ran == 3)
                isRandom2 = true;
            else
                isRandom2 = false;
            yield return null;

            if (isRandom1)//�قڃ����_���ړ�
            {
                _PiecePosition.Clear();
                _CanAttackPiece.Clear();
                yield return null;
                //��̈ʒu���i�[����(AI���猩�ēG��̂�)
                for (int i = 0; i < _allPieceInfos.Length; i++)
                {
                    if (!_allPieceInfos[i].Side) continue;
                    if (_allPieceInfos[i].isDie) continue;
                    _PiecePosition.Add((int)_allPieceInfos[i].CurrentPosition.x - 1 + (int)((_allPieceInfos[i].CurrentPosition.y - 1) * BoardSize));
                    yield return null;
                }
                //�ړ������ɍU���\�ȋ�̑I��
                for (int i = 0; i < _allPieceInfos.Length; i++)
                {
                    if (_allPieceInfos[i].Side) continue;
                    if (_allPieceInfos[i].isDie) continue;
                    _AttackRangePosition.Clear();
                    StoreAttackRangePosition(_allPieceInfos[i]);
                    yield return null;

                    List<int> hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
                    yield return null;
                    if (hitList.Count != 0)
                        _CanAttackPiece.Add(_allPieceInfos[i]);
                }
                _CanMovePiece.Clear();
                //�ړ��\��̑I��
                for (int i = 0; i < _allPieceInfos.Length; i++)
                {
                    if (_allPieceInfos[i].Side) continue;
                    if (_allPieceInfos[i].isDie) continue;
                    if (_CanAttackPiece.Contains(_allPieceInfos[i])) continue;
                    if (IsMovePossible(_allPieceInfos[i]))
                        _CanMovePiece.Add(_allPieceInfos[i]);
                    yield return null;
                }
                //�ړ��\�����Ȃ��Ȃ�ړ��t�F�[�Y�I��
                if (_CanMovePiece.Count == 0)
                {
                    _manager.OnClickGoAttack();
                    StartCoroutine(AIAttack());
                    yield break;
                }
                //�擾������烉���_���Ɉ�擾
                _movePieceInfo = _CanMovePiece[UnityEngine.Random.Range(0, _CanMovePiece.Count)];
                yield return null;
                //�ړ����S�Ď擾
                StoreMoveDestination();
                yield return null;
                //�擾�����ړ��悩�烉���_���Ɉ�I��
                int moveDest = _MoveDestination[UnityEngine.Random.Range(0, _MoveDestination.Count)];
                yield return null;
                int beforePosition = (int)_movePieceInfo.CurrentPosition.x - 1 + (int)((_movePieceInfo.CurrentPosition.y - 1) * BoardSize);
                _tiles[beforePosition].ExistPiece = false;
                _tiles[moveDest].ExistPiece = true;
                _tileManager.CheckBuffTile(_movePieceInfo, moveDest, true);
                _movePieceInfo.GetComponentInChildren<PieceAnimation>().Move(_tiles[moveDest].myPosition, true);
                yield return wait;
                _manager.MinusMoveCount();
                continue;
            }

            //�S�Ă̋�̈ʒu���i�[����
            _AllPiecePosition.Clear();
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                if (_allPieceInfos[i].isDie)
                {
                    _AllPiecePosition.Add(-1);
                    continue;
                }
                else
                    _AllPiecePosition.Add((int)_allPieceInfos[i].CurrentPosition.x - 1 + (int)((_allPieceInfos[i].CurrentPosition.y - 1) * BoardSize));
                yield return null;
            }
            _PiecePosition.Clear();
            _CanAttackPiece.Clear();
            yield return null;
            //��̈ʒu���i�[����(AI���猩�ēG��̂�)
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                if (!_allPieceInfos[i].Side) continue;
                if (_allPieceInfos[i].isDie) continue;
                _PiecePosition.Add((int)_allPieceInfos[i].CurrentPosition.x - 1 + (int)((_allPieceInfos[i].CurrentPosition.y - 1) * BoardSize));
                yield return null;
            }
            //�ړ������ɍU���\�ȋ�̑I��
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                if (_allPieceInfos[i].Side) continue;
                if (_allPieceInfos[i].isDie) continue;
                _AttackRangePosition.Clear();
                StoreAttackRangePosition(_allPieceInfos[i]);
                yield return null;
                List<int> hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
                yield return null;
                if (hitList.Count != 0)//���̏�ōU���\
                {
                    //�U���ΏۂƂ̑�������
                    for (int k = 0; k < hitList.Count; k++)
                    {
                        //�����s���łȂ��Ȃ�ǉ�
                        if (AttributeCompatibilityCheck(_allPieceInfos[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList[k])]) != 0)
                        {
                            _CanAttackPiece.Add(_allPieceInfos[i]);
                            break;
                        }
                        yield return null;
                    }
                }
            }
            _CanMovePiece.Clear();
            //�ړ��\��̑I��
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                //AI��{����łȂ��{���̏�ōU���\���������s��or���̏�ōU���\�łȂ�
                if (_allPieceInfos[i].Side) continue;
                if (_allPieceInfos[i].isDie) continue;
                if (_CanAttackPiece.Contains(_allPieceInfos[i])) continue;
                if (IsMovePossible(_allPieceInfos[i]))
                    _CanMovePiece.Add(_allPieceInfos[i]);
                yield return null;
            }

            //�ړ��\�����Ȃ��Ȃ�ړ��t�F�[�Y�I��
            if (_CanMovePiece.Count == 0)
            {
                _manager.OnClickGoAttack();
                StartCoroutine(AIAttack());
                yield break;
            }
            _MoveEvaluateValue.Clear();
            _MoveEvaluatePiece.Clear();
            _MoveEvaluatePosition.Clear();

            //�擾������ƂɈړ���̕]�����s���A�]���l�ŕ����Ċi�[����
            for (int i = 0; i < _CanMovePiece.Count; i++)
            {
                _movePieceInfo = _CanMovePiece[i];
                StoreMoveDestination();
                for (int j = 0; j < _MoveDestination.Count; j++)
                {
                    moveEvaluate = 0f;
                    _AttackRangePosition.Clear();
                    StoreAttackRangePosition(_CanMovePiece[i], true, _MoveDestination[j]);
                    yield return null;

                    List<int> hitList = new List<int>();
                    hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
                    yield return null;
                    if (hitList.Count != 0)//�ړ���ŋ�U���\
                    {
                        //�U���ΏۂƂ̑�������
                        for (int k = 0; k < hitList.Count; k++)
                        {
                            if (AttributeCompatibilityCheck(_CanMovePiece[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList[k])]) == 1)
                            {
                                moveEvaluate = 3.25f + RoleEvaluation(0, _CanMovePiece[i]);
                                if (_allPieceInfos[_AllPiecePosition.IndexOf(hitList[k])].Role == 4)//�Ώۂ��w�����Ȃ炻�̈ړ��ɂȂ�
                                    moveEvaluate = 10f + RoleEvaluation(0, _CanMovePiece[i]);
                                break;
                            }
                            else if (AttributeCompatibilityCheck(_CanMovePiece[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList[k])]) == 2)
                            {
                                moveEvaluate = 5f + RoleEvaluation(0, _CanMovePiece[i]);
                                break;
                            }
                            else
                                moveEvaluate = 1.5f + RoleEvaluation(0, _CanMovePiece[i]);
                            yield return null;
                        }
                        //�ړ��悪���ɉ�������̂Ȃ�+0.5f�A�O�Ȃ�+0.25f
                        if (_CanMovePiece[i].CurrentPosition.y > (_MoveDestination[j] / BoardSize) + 1)
                            moveEvaluate += 0.5f;
                        if (_CanMovePiece[i].CurrentPosition.y < (_MoveDestination[j] / BoardSize) + 1)
                            moveEvaluate += 0.25f;
                    }
                    else//�ړ���ōU���\�łȂ�
                    {
                        //������x�ړ������ꍇ�ɍU���\�����ׂ�
                        StoreNextMoveDestination(_CanMovePiece[i], _MoveDestination[j]);
                        for (int l = 0; l < _NextMoveDestination.Count; l++)
                        {
                            _AttackRangePosition.Clear();
                            StoreAttackRangePosition(_CanMovePiece[i], true, _NextMoveDestination[l]);

                            List<int> hitList2 = new List<int>();
                            hitList2 = _PiecePosition.FindAll(_AttackRangePosition.Contains);
                            yield return null;
                            if (hitList2.Count != 0)//�ړ���ŋ�U���\
                            {
                                //�U���ΏۂƂ̑�������
                                for (int k = 0; k < hitList2.Count; k++)
                                {
                                    if (AttributeCompatibilityCheck(_CanMovePiece[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList2[k])]) == 1)
                                    {
                                        moveEvaluate = 1.75f + RoleEvaluation(1, _CanMovePiece[i]);
                                        if (_allPieceInfos[_AllPiecePosition.IndexOf(hitList2[k])].Role == 4)//�Ώۂ��w�����Ȃ�]��UP
                                            moveEvaluate = 3f + RoleEvaluation(0, _CanMovePiece[i]);
                                        break;
                                    }
                                    else if (AttributeCompatibilityCheck(_CanMovePiece[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList2[k])]) == 2)
                                    {
                                        moveEvaluate = 2.75f + RoleEvaluation(1, _CanMovePiece[i]);
                                        break;
                                    }
                                    else
                                        moveEvaluate = 0.75f + RoleEvaluation(1, _CanMovePiece[i]);
                                    yield return null;
                                }
                            }
                            else
                            {
                                moveEvaluate = 1f + RoleEvaluation(2, _CanMovePiece[i]);
                                if (_CanMovePiece[i].CurrentPosition.y < (_MoveDestination[j] / BoardSize) + 1)
                                    moveEvaluate += 0.25f;
                                if (_CanMovePiece[i].CurrentPosition.y + 1 < (_MoveDestination[j] / BoardSize) + 1)
                                    moveEvaluate += 0.1f;
                            }
                            if (moveEvaluate >= 3f) break;
                        }
                    }
                    //���ʂ��i�[
                    _MoveEvaluateValue.Add(moveEvaluate);
                    _MoveEvaluatePiece.Add(_CanMovePiece[i]);
                    _MoveEvaluatePosition.Add(_MoveDestination[j]);
                    yield return null;
                }
            }
            //�]���l���\�[�g
            List<Tuple<float, PieceInfomation, int>> combinedList = new List<Tuple<float, PieceInfomation, int>>();
            for (int i = 0; i < _MoveEvaluateValue.Count; i++)
                combinedList.Add(new Tuple<float, PieceInfomation, int>(_MoveEvaluateValue[i], _MoveEvaluatePiece[i], _MoveEvaluatePosition[i]));
            yield return null;
            _MoveEvaluateValue.Sort((x, y) => y.CompareTo(x));
            combinedList = combinedList.OrderBy(t => _MoveEvaluateValue.IndexOf(t.Item1)).ToList();
            _MoveEvaluateValue = combinedList.Select(t => t.Item1).ToList();
            _MoveEvaluatePiece = combinedList.Select(t => t.Item2).ToList();
            _MoveEvaluatePosition = combinedList.Select(t => t.Item3).ToList();
            yield return null;

            //�]�����ʂ���ړ������������߂�(���l�̓����_��)
            int sameValueNum = 0;
            while (_MoveEvaluateValue[0] == _MoveEvaluateValue[sameValueNum])
            {
                sameValueNum++;
                if (_MoveEvaluateValue.Count == sameValueNum)
                    break;
            }
            int pieceNumber = UnityEngine.Random.Range(0, sameValueNum);
            /*
            Debug.Log("�����]���l�̐��F" + sameValueNum);
            Debug.Log("���������----------------------------------------------------------------");
            Debug.Log("�]���l�F" + _MoveEvaluateValue[pieceNumber]);
            Debug.Log("�]����F" + _MoveEvaluatePiece[pieceNumber]);
            Debug.Log("�]���ʒu�F" + _MoveEvaluatePosition[pieceNumber]);
            */
            _movePieceInfo = _MoveEvaluatePiece[pieceNumber];
            yield return null;
            int beforePos = (int)_movePieceInfo.CurrentPosition.x - 1 + (int)((_movePieceInfo.CurrentPosition.y - 1) * BoardSize);
            _tiles[beforePos].ExistPiece = false;
            _tiles[_MoveEvaluatePosition[pieceNumber]].ExistPiece = true;
            _tileManager.CheckBuffTile(_movePieceInfo, _MoveEvaluatePosition[pieceNumber], true);
            _movePieceInfo.GetComponentInChildren<PieceAnimation>().Move(_tiles[_MoveEvaluatePosition[pieceNumber]].myPosition, true);
            yield return wait;

            _manager.MinusMoveCount();
        }
        _manager.OnClickGoAttack();
        StartCoroutine(AIAttack());
        yield break;
    }

    private IEnumerator AIAttack()
    {
        _AllPiecePosition.Clear();
        _PiecePosition.Clear();
        _CanAttackPiece.Clear();
        //��̈ʒu���i�[����(AI���猩�ēG��̂�)
        for (int i = 0; i < _allPieceInfos.Length; i++)
        {
            if (_allPieceInfos[i].isDie)
            {
                _AllPiecePosition.Add(-1);
                continue;
            }
            else
                _AllPiecePosition.Add((int)_allPieceInfos[i].CurrentPosition.x - 1 + (int)((_allPieceInfos[i].CurrentPosition.y - 1) * BoardSize));
            if (!_allPieceInfos[i].Side) continue;
            _PiecePosition.Add((int)_allPieceInfos[i].CurrentPosition.x - 1 + (int)((_allPieceInfos[i].CurrentPosition.y - 1) * BoardSize));
            yield return null;
        }
        //�U���\��̑I��
        for (int i = 0; i < _allPieceInfos.Length; i++)
        {
            if (_allPieceInfos[i].Side) continue;
            if (_allPieceInfos[i].isDie) continue;

            _AttackRangePosition.Clear();
            yield return null;
            StoreAttackRangePosition(_allPieceInfos[i]);
            yield return null;

            List<int> hitList = new List<int>();
            hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
            yield return null;
            //�U���\�ōU���Ώۂ��P���̏ꍇ�͂��̂܂܍U��
            if (hitList.Count != 0)
            {
                if(hitList.Count == 1)//�U���Ώۂ��P��
                {
                    //Debug.Log("AI���U�����܂�");
                    if (!_allPieceInfos[_AllPiecePosition.IndexOf(hitList[0])].isCanAttacked)
                        yield return wait; yield return wait;
                    _tileManager.AttackPiece(_allPieceInfos[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList[0])]);
                    yield return wait;
                    //��̈ʒu���ēx�i�[����(�|�����\��������̂�)
                    _PiecePosition.Clear();
                    for (int j = 0; j < _allPieceInfos.Length; j++)
                    {
                        if (!_allPieceInfos[j].Side) continue;
                        if (_allPieceInfos[j].isDie) continue;
                        _PiecePosition.Add((int)_allPieceInfos[j].CurrentPosition.x - 1 + (int)((_allPieceInfos[j].CurrentPosition.y - 1) * BoardSize));
                        yield return null;
                    }
                    continue;
                }
                _CanAttackPiece.Add(_allPieceInfos[i]);//����ȊO�̍U���\��
            }
        }

        if (_CanAttackPiece.Count == 0)
        {
            _manager.OnClickTurnFinish(true);
            yield break;
        }

        for (int i = 0; i < _CanAttackPiece.Count; i++)
        {
            _AttackRangePosition.Clear();
            yield return null;
            StoreAttackRangePosition(_CanAttackPiece[i]);
            yield return null;

            List<int> hitList = new List<int>();
            hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
            yield return null;
            if (hitList.Count != 0)
            {
                //Debug.Log("AI���U�����܂�");
                int attackPosition, enemyNum;
                //�U����̒��ōł��^����_���[�W���傫�������_��(���|����ꍇ�͗D��)
                _DamagePredictionList.Clear();
                for (int k = 0; k < hitList.Count; k++)
                {
                    enemyNum = _AllPiecePosition.IndexOf(hitList[k]);
                    _DamagePredictionList.Add(DamagePrediction(_CanAttackPiece[i], _allPieceInfos[enemyNum]));
                    yield return null;
                }
                int maxDamageNumber = 0;
                for (int k = 0; k < hitList.Count - 1; k++)
                {
                    if (_DamagePredictionList[maxDamageNumber] < _DamagePredictionList[k + 1])
                        maxDamageNumber = k + 1;
                    yield return null;
                }
                attackPosition = hitList[maxDamageNumber];
                //�w�������܂܂��ꍇ�͍ŗD��ő_��
                for (int k = 0; k < hitList.Count; k++)
                {
                    if (_allPieceInfos[_AllPiecePosition.IndexOf(hitList[k])].Role == 4)//�U���ΏۂɎw�������܂܂�邩
                        attackPosition = hitList[k];
                }
                yield return null;

                //�U����̗v�f�ԍ�
                enemyNum = _AllPiecePosition.IndexOf(attackPosition);
                if (!_allPieceInfos[enemyNum].isCanAttacked)
                    yield return wait; yield return wait;
                _tileManager.AttackPiece(_CanAttackPiece[i], _allPieceInfos[enemyNum]);
                yield return wait;
                //��̈ʒu���ēx�i�[����(�|�����\��������̂�)
                _PiecePosition.Clear();
                for (int j = 0; j < _allPieceInfos.Length; j++)
                {
                    if (!_allPieceInfos[j].Side) continue;
                    if (_allPieceInfos[j].isDie) continue;
                    _PiecePosition.Add((int)_allPieceInfos[j].CurrentPosition.x - 1 + (int)((_allPieceInfos[j].CurrentPosition.y - 1) * BoardSize));
                    yield return null;
                }
            }
        }
        _manager.OnClickTurnFinish(true);
        yield break;
    }

    private bool IsMovePossible(PieceInfomation _info)
    {
        if (!_manager.canMove) return (false);
        Vector2 pos = _info.CurrentPosition;
        int role = _info.Role;
        TileNumber = (int)pos.x - 1 + (int)((pos.y - 1) * BoardSize);

        if (pos.y != 1)//��
            if (!_tiles[TileNumber - BoardSize].ExistPiece) return (true);
        if (pos.y != BoardSize)//��
            if (!_tiles[TileNumber + BoardSize].ExistPiece) return (true);
        if (pos.x != BoardSize)//�E
            if (!_tiles[TileNumber + 1].ExistPiece) return (true);
        if (pos.x != 1)//��
            if (!_tiles[TileNumber - 1].ExistPiece) return (true);

        if (role == 3 || role == 4 || _tileManager.isHalfPieceDown)
        {
            if (pos.y != 1 && pos.x != BoardSize)//�E��
                if (!_tiles[TileNumber - (BoardSize - 1)].ExistPiece) return (true);
            if (pos.y != BoardSize && pos.x != BoardSize)//�E��
                if (!_tiles[TileNumber + (BoardSize + 1)].ExistPiece) return (true);
            if (pos.y != 1 && pos.x != 1)//����
                if (!_tiles[TileNumber - (BoardSize + 1)].ExistPiece) return (true);
            if (pos.y != BoardSize && pos.x != 1)//����
                if (!_tiles[TileNumber + (BoardSize - 1)].ExistPiece) return (true);
        }

        if (role == 3 || _tileManager.isHalfPieceDown)
        {
            if (pos.y > 2)//���
                if (!_tiles[TileNumber - (BoardSize * 2)].ExistPiece) return (true);
            if (pos.y < (BoardSize - 1))//����
                if (!_tiles[TileNumber + (BoardSize * 2)].ExistPiece) return (true);
            if (pos.x < (BoardSize - 1))//�E�E
                if (!_tiles[TileNumber + 2].ExistPiece) return (true);
            if (pos.x > 2)//����
                if (!_tiles[TileNumber - 2].ExistPiece) return (true);
        }
        return (false);
    }

    //���ݒn����̈ړ�����i�[
    private void StoreMoveDestination()
    {
        if (!_manager.canMove) return;
        Vector2 pos = _movePieceInfo.CurrentPosition;
        int role = _movePieceInfo.Role;
        TileNumber = (int)pos.x - 1 + (int)((pos.y - 1) * BoardSize);

        _MoveDestination.Clear();
        if (pos.y != 1 && !_tiles[TileNumber - BoardSize].ExistPiece)//��
            _MoveDestination.Add(TileNumber - BoardSize);
        if (pos.y != BoardSize && !_tiles[TileNumber + BoardSize].ExistPiece)//��
            _MoveDestination.Add(TileNumber + BoardSize);
        if (pos.x != BoardSize && !_tiles[TileNumber + 1].ExistPiece)//�E
            _MoveDestination.Add(TileNumber + 1);
        if (pos.x != 1 && !_tiles[TileNumber - 1].ExistPiece)//��
            _MoveDestination.Add(TileNumber - 1);
        if (role == 3 || role == 4 || _tileManager.isHalfPieceDown)
        {
            if (pos.y != 1 && pos.x != BoardSize && !_tiles[TileNumber - (BoardSize - 1)].ExistPiece)//�E��
                _MoveDestination.Add(TileNumber - (BoardSize - 1));
            if (pos.y != BoardSize && pos.x != BoardSize && !_tiles[TileNumber + (BoardSize + 1)].ExistPiece)//�E��
                _MoveDestination.Add(TileNumber + (BoardSize + 1));
            if (pos.y != 1 && pos.x != 1 && !_tiles[TileNumber - (BoardSize + 1)].ExistPiece)//����
                _MoveDestination.Add(TileNumber - (BoardSize + 1));
            if (pos.y != BoardSize && pos.x != 1 && !_tiles[TileNumber + (BoardSize - 1)].ExistPiece)//����
                _MoveDestination.Add(TileNumber + (BoardSize - 1));
        }
        if (role == 3 || _tileManager.isHalfPieceDown)
        {
            if (pos.y > 2 && !_tiles[TileNumber - (BoardSize * 2)].ExistPiece)//���
                _MoveDestination.Add(TileNumber - (BoardSize * 2));
            if (pos.y < (BoardSize - 1) && !_tiles[TileNumber + (BoardSize * 2)].ExistPiece)//����
                _MoveDestination.Add(TileNumber + (BoardSize * 2));
            if (pos.x < (BoardSize - 1) && !_tiles[TileNumber + 2].ExistPiece)//�E�E
                _MoveDestination.Add(TileNumber + 2);
            if (pos.x > 2 && !_tiles[TileNumber - 2].ExistPiece)//����
                _MoveDestination.Add(TileNumber - 2);
        }
    }

    //�ړ��悩��̈ړ�����i�[(���ړ��̍l���p)
    private void StoreNextMoveDestination(PieceInfomation _info, int MovePosNumber)
    {
        if (!_manager.canMove) return;
        Vector2 pos;
        pos.x = (MovePosNumber % BoardSize) + 1;
        pos.y = (MovePosNumber / BoardSize) + 1;
        int role = _info.Role;
        TileNumber = MovePosNumber;

        _NextMoveDestination.Clear();
        if (pos.y != 1 && !_tiles[TileNumber - BoardSize].ExistPiece)//��
            _NextMoveDestination.Add(TileNumber - BoardSize);
        if (pos.y != BoardSize && !_tiles[TileNumber + BoardSize].ExistPiece)//��
            _NextMoveDestination.Add(TileNumber + BoardSize);
        if (pos.x != BoardSize && !_tiles[TileNumber + 1].ExistPiece)//�E
            _NextMoveDestination.Add(TileNumber + 1);
        if (pos.x != 1 && !_tiles[TileNumber - 1].ExistPiece)//��
            _NextMoveDestination.Add(TileNumber - 1);
        if (role == 3 || role == 4 || _tileManager.isHalfPieceDown)
        {
            if (pos.y != 1 && pos.x != BoardSize && !_tiles[TileNumber - (BoardSize - 1)].ExistPiece)//�E��
                _NextMoveDestination.Add(TileNumber - (BoardSize - 1));
            if (pos.y != BoardSize && pos.x != BoardSize && !_tiles[TileNumber + (BoardSize + 1)].ExistPiece)//�E��
                _NextMoveDestination.Add(TileNumber + (BoardSize + 1));
            if (pos.y != 1 && pos.x != 1 && !_tiles[TileNumber - (BoardSize + 1)].ExistPiece)//����
                _NextMoveDestination.Add(TileNumber - (BoardSize + 1));
            if (pos.y != BoardSize && pos.x != 1 && !_tiles[TileNumber + (BoardSize - 1)].ExistPiece)//����
                _NextMoveDestination.Add(TileNumber + (BoardSize - 1));
        }
        if (role == 3 || _tileManager.isHalfPieceDown)
        {
            if (pos.y > 2 && !_tiles[TileNumber - (BoardSize * 2)].ExistPiece)//���
                _NextMoveDestination.Add(TileNumber - (BoardSize * 2));
            if (pos.y < (BoardSize - 1) && !_tiles[TileNumber + (BoardSize * 2)].ExistPiece)//����
                _NextMoveDestination.Add(TileNumber + (BoardSize * 2));
            if (pos.x < (BoardSize - 1) && !_tiles[TileNumber + 2].ExistPiece)//�E�E
                _NextMoveDestination.Add(TileNumber + 2);
            if (pos.x > 2 && !_tiles[TileNumber - 2].ExistPiece)//����
                _NextMoveDestination.Add(TileNumber - 2);
        }
    }

    private void StoreAttackRangePosition(PieceInfomation _info, bool type = false, int tileNumber = 0)
    {
        Vector2 pos;
        if (!type)
        {
            pos = _info.CurrentPosition;
            TileNumber = (int)pos.x - 1 + (int)((pos.y - 1) * BoardSize);
        }
        else
        {
            TileNumber = tileNumber;
            pos.x = (tileNumber % BoardSize) + 1;
            pos.y = (tileNumber / BoardSize) + 1;
        }
        int role = _info.Role;

        if (pos.y != 1)//��
            _AttackRangePosition.Add(TileNumber - BoardSize);
        if (pos.y != BoardSize)//��
            _AttackRangePosition.Add(TileNumber + BoardSize);
        if (pos.x != BoardSize)//�E
            _AttackRangePosition.Add(TileNumber + 1);
        if (pos.x != 1)//��
            _AttackRangePosition.Add(TileNumber - 1);

        if (role == 1 || role == 2 || role == 4)
        {
            if (pos.y != 1 && pos.x != BoardSize)//�E��
                _AttackRangePosition.Add(TileNumber - (BoardSize - 1));
            if (pos.y != BoardSize && pos.x != BoardSize)//�E��
                _AttackRangePosition.Add(TileNumber + (BoardSize + 1));
            if (pos.y != 1 && pos.x != 1)//����
                _AttackRangePosition.Add(TileNumber - (BoardSize + 1));
            if (pos.y != BoardSize && pos.x != 1)//����
                _AttackRangePosition.Add(TileNumber + (BoardSize - 1));
        }
        if (role == 2)
        {
            if (pos.y > 2)//���
                _AttackRangePosition.Add(TileNumber - (BoardSize * 2));
            if (pos.y < (BoardSize - 1))//����
                _AttackRangePosition.Add(TileNumber + (BoardSize * 2));
            if (pos.x < (BoardSize - 1))//�E�E
                _AttackRangePosition.Add(TileNumber + 2);
            if (pos.x > 2)//����
                _AttackRangePosition.Add(TileNumber - 2);
        }
        return;
    }

    //��������
    private int AttributeCompatibilityCheck(PieceInfomation _allyInfo, PieceInfomation _enemyInfo)
    {
        switch (_allyInfo.Attribute)
        {
            case 0:
                if (_enemyInfo.Attribute == 0 || _enemyInfo.Attribute == 3)
                    return 1;
                else if (_enemyInfo.Attribute == 1)
                    return 0;
                else
                    return 2;
            case 1:
                if (_enemyInfo.Attribute == 0)
                    return 2;
                else if (_enemyInfo.Attribute == 1 || _enemyInfo.Attribute == 3)
                    return 1;
                else
                    return 0;
            case 2:
                if (_enemyInfo.Attribute == 0)
                    return 0;
                else if (_enemyInfo.Attribute == 1)
                    return 2;
                else
                    return 1;
            case 3:
                return 1;
            default:
                return 0;
        }
    }

    //�U���\���_���[�W
    private int DamagePrediction(PieceInfomation _allyInfo, PieceInfomation _enemyInfo)
    {
        float Attack = _allyInfo.Attack; float Defence = _enemyInfo.Defense; float HitNum = 0;
        if (_allyInfo.Role == 2)
            Defence = _enemyInfo.MagicDefense;
        switch (_allyInfo.Attribute)
        {
            case 0:
                if (_enemyInfo.Attribute == 0 || _enemyInfo.Attribute == 3)
                    HitNum = 40;
                else if (_enemyInfo.Attribute == 1)
                    HitNum = 25;
                else
                    HitNum = 60;
                break;
            case 1:
                if (_enemyInfo.Attribute == 0)
                    HitNum = 60;
                else if (_enemyInfo.Attribute == 1 || _enemyInfo.Attribute == 3)
                    HitNum = 40;
                else
                    HitNum = 25;
                break;
            case 2:
                if (_enemyInfo.Attribute == 0)
                    HitNum = 25;
                else if (_enemyInfo.Attribute == 1)
                    HitNum = 60;
                else
                    HitNum = 40;
                break;
            case 3:
                HitNum = 40;
                break;
        }
        HitNum *= (Attack / Defence);
        if (_enemyInfo.HP - HitNum <= 0)//�|����
            HitNum = 300;
        HitNum += (RoleEvaluation(3, _enemyInfo) * 4);
        return (int)HitNum;
    }

    //��E�|�C���g
    private float RoleEvaluation(int mode, PieceInfomation _Info)
    {
        if (isRandom2 && mode != 3) return 0.5f;
        if (mode == 0)//�ړ�1
        {
            switch (_Info.Role)
            {
                case 0:
                    return 0.25f;
                case 1:
                case 2:
                    return 0.75f;
                case 3:
                    return 0.5f;
                case 4:
                default:
                    return 0f;
            }
        }
        else if (mode == 1)//�ړ�2
        {
            switch (_Info.Role)
            {
                case 0:
                    return 0.5f;
                case 1:
                case 2:
                    return 0.75f;
                case 3:
                    return 0.25f;
                case 4:
                default:
                    return 0f;
            }
        }
        else if (mode == 2)//�ړ�3
        {
            switch (_Info.Role)
            {
                case 1:
                case 2:
                    return 0.5f;
                case 3:
                    return 0f;
                case 0:
                case 4:
                default:
                    return 0.25f;
            }
        }
        else//�U��
        {
            switch (_Info.Role)
            {
                case 0:
                    return 0f;
                case 1:
                    return 0.25f;
                case 2:
                    return 0.5f;
                case 3:
                    return 0.75f;
                case 4:
                default:
                    return 1f;
            }
        }
    }
}
