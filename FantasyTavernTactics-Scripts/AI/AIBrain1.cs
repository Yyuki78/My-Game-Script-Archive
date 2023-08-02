using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AIBrain1 : MonoBehaviour, IAI
{
    private int BoardSize = 0;

    private MeshRenderer CupRenderer;
    private TextMeshProUGUI _moveNum;

    private GameInfomation _infomation;
    private TurnManager _manager;
    private TileManager _tileManager;

    private TileInfomation[] _tiles;
    private PieceInfomation[] _allPieceInfos;
    private PieceInfomation _movePieceInfo;
    private PieceAnimation[] _allPieceAnimes;
    private int TileNumber;

    private List<PieceInfomation> _CanMovePiece = new List<PieceInfomation>();
    private List<PieceInfomation> _CanAttackPiece = new List<PieceInfomation>();
    private List<int> _MoveDestination = new List<int>();
    private List<int> _AllPiecePosition = new List<int>();
    private List<int> _PiecePosition = new List<int>();
    private List<int> _AttackRangePosition = new List<int>();

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

        int ran = Random.Range(1, 4);
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
                yield return null;
                StoreAttackRangePosition(_allPieceInfos[i]);
                yield return null;

                List<int> hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
                yield return null;
                if (hitList.Count != 0)
                {
                    _CanAttackPiece.Add(_allPieceInfos[i]);
                }
            }
            _CanMovePiece.Clear();
            //�ړ��\��̑I��
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                if (_allPieceInfos[i].Side) continue;
                if (_allPieceInfos[i].isDie) continue;
                if(_CanAttackPiece.Contains(_allPieceInfos[i])) continue;
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
            _movePieceInfo = _CanMovePiece[Random.Range(0, _CanMovePiece.Count)];
            yield return null;

            //�ړ����S�Ď擾+�]��
            StoreMoveDestination();
            yield return null;

            //�擾�����ړ��悩�烉���_���Ɉ�I��
            int moveDest = _MoveDestination[Random.Range(0, _MoveDestination.Count)];
            yield return null;

            int beforePos = (int)_movePieceInfo.CurrentPosition.x - 1 + (int)((_movePieceInfo.CurrentPosition.y - 1) * BoardSize);
            _tiles[beforePos].ExistPiece = false;
            _tiles[moveDest].ExistPiece = true;
            _tileManager.CheckBuffTile(_movePieceInfo, moveDest, true);
            _movePieceInfo.GetComponentInChildren<PieceAnimation>().Move(_tiles[moveDest].myPosition, true);
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
            if (hitList.Count != 0)
            {
                //Debug.Log("AI���U�����܂�");
                //�U����������_���ɑI��
                int attackPosition = hitList[Random.Range(0, hitList.Count)];
                //�U����̗v�f�ԍ�
                int enemyNum = _AllPiecePosition.IndexOf(attackPosition);
                if(!_allPieceInfos[enemyNum].isCanAttacked)
                    yield return wait; yield return wait;
                _tileManager.AttackPiece(_allPieceInfos[i], _allPieceInfos[enemyNum]);
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

    private void StoreAttackRangePosition(PieceInfomation _info)
    {
        Vector2 pos = _info.CurrentPosition;
        int role = _info.Role;
        TileNumber = (int)pos.x - 1 + (int)((pos.y - 1) * BoardSize);

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
}
