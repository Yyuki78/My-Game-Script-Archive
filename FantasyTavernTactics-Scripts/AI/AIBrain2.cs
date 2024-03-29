using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class AIBrain2 : MonoBehaviour, IAI
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
    private PieceAnimation _pieceAnimes;
    private int TileNumber;

    private List<PieceInfomation> _CanMovePiece = new List<PieceInfomation>();
    private List<PieceInfomation> _CanAttackPiece = new List<PieceInfomation>();
    private List<int> _MoveDestination = new List<int>();
    private List<int> _MoveEvaluate5 = new List<int>();
    private List<int> _MoveEvaluate2 = new List<int>();
    private List<int> _MoveEvaluate1 = new List<int>();
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

        //確率操作をする30%,40%,30%
        int ran = Random.Range(0, 10);
        if (ran < 3)
            ran = 1;
        else if (ran < 7)
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
            _PiecePosition.Clear();
            _CanAttackPiece.Clear();
            yield return null;
            //駒の位置を格納する(AIから見て敵駒のみ)
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                if (!_allPieceInfos[i].Side) continue;
                if (_allPieceInfos[i].isDie) continue;
                _PiecePosition.Add((int)_allPieceInfos[i].CurrentPosition.x - 1 + (int)((_allPieceInfos[i].CurrentPosition.y - 1) * BoardSize));
                yield return null;
            }

            //移動せずに攻撃可能な駒の選別
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
                    _CanAttackPiece.Add(_allPieceInfos[i]);
            }
            _CanMovePiece.Clear();
            //移動可能駒の選別
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                if (_allPieceInfos[i].Side) continue;
                if (_allPieceInfos[i].isDie) continue;
                if (_CanAttackPiece.Contains(_allPieceInfos[i])) continue;
                if (IsMovePossible(_allPieceInfos[i]))
                    _CanMovePiece.Add(_allPieceInfos[i]);
                yield return null;
            }

            //移動可能駒が一つもないなら移動フェーズ終了
            if (_CanMovePiece.Count == 0)
            {
                _manager.OnClickGoAttack();
                StartCoroutine(AIAttack());
                yield break;
            }
            _MoveEvaluate1.Clear();
            _MoveEvaluate2.Clear();
            _MoveEvaluate5.Clear();

            //取得した駒ごとに移動先の評価を行い、評価値(1,2,5)で分けて格納する
            for (int i = 0; i < _CanMovePiece.Count; i++)
            {
                _movePieceInfo = _CanMovePiece[i];
                StoreMoveDestination();
                int pieceNumber = 0;
                for (int j = 0; j < _MoveDestination.Count; j++)
                {
                    pieceNumber = i * 100 + j;
                    _AttackRangePosition.Clear();
                    StoreAttackRangePosition(_CanMovePiece[i], true, _MoveDestination[j]);
                    yield return null;

                    List<int> hitList = new List<int>();
                    hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
                    yield return null;
                    if (hitList.Count != 0)
                    {
                        //Debug.Log("移動先でAIが攻撃可能です");
                        _MoveEvaluate5.Add(pieceNumber);
                    }
                    else
                    {
                        if (_CanMovePiece[i].CurrentPosition.y < (_MoveDestination[j] / BoardSize) + 1)
                            _MoveEvaluate2.Add(pieceNumber);
                        else
                            _MoveEvaluate1.Add(pieceNumber);
                    }
                    yield return null;
                }
            }

            //評価結果から移動させる駒を決める
            int movePosition = 0;
            if (_MoveEvaluate5.Count != 0)
                movePosition = _MoveEvaluate5[Random.Range(0, _MoveEvaluate5.Count)];
            else
            {
                if (_MoveEvaluate2.Count != 0)
                {
                    //Debug.Log("評価値2の移動をします" + movePosition);
                    movePosition = _MoveEvaluate2[Random.Range(0, _MoveEvaluate2.Count)];
                }
                else
                {
                    //Debug.Log("評価値1の移動をします" + movePosition);
                    movePosition = _MoveEvaluate1[Random.Range(0, _MoveEvaluate1.Count)];
                }
            }
            
            _movePieceInfo = _CanMovePiece[movePosition / 100];
            StoreMoveDestination();
            int moveDest = _MoveDestination[movePosition % 100];
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
        //駒の位置を格納する(AIから見て敵駒のみ)
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
        //攻撃可能駒の選別
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
                //Debug.Log("AIが攻撃します");
                int attackPosition, enemyNum;
                if (hitList.Count == 1)
                    attackPosition = hitList[0];
                else
                {
                    //攻撃先の中で最も与えるダメージが大きい相手を狙う
                    _DamagePredictionList.Clear();
                    for (int k = 0; k < hitList.Count; k++)
                    {
                        enemyNum = _AllPiecePosition.IndexOf(hitList[k]);
                        _DamagePredictionList.Add(DamagePrediction(_allPieceInfos[i], _allPieceInfos[enemyNum]));
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
                }
                yield return null;

                //攻撃先の要素番号
                enemyNum = _AllPiecePosition.IndexOf(attackPosition);
                if (!_allPieceInfos[enemyNum].isCanAttacked)
                    yield return wait; yield return wait;
                _tileManager.AttackPiece(_allPieceInfos[i], _allPieceInfos[enemyNum]);
                yield return wait;
                //駒の位置を再度格納する(倒した可能性があるので)
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

        if (pos.y != 1)//上
            if (!_tiles[TileNumber - BoardSize].ExistPiece) return (true);
        if (pos.y != BoardSize)//下
            if (!_tiles[TileNumber + BoardSize].ExistPiece) return (true);
        if (pos.x != BoardSize)//右
            if (!_tiles[TileNumber + 1].ExistPiece) return (true);
        if (pos.x != 1)//左
            if (!_tiles[TileNumber - 1].ExistPiece) return (true);

        if (role == 3 || role == 4 || _tileManager.isHalfPieceDown)
        {
            if (pos.y != 1 && pos.x != BoardSize)//右上
                if (!_tiles[TileNumber - (BoardSize - 1)].ExistPiece) return (true);
            if (pos.y != BoardSize && pos.x != BoardSize)//右下
                if (!_tiles[TileNumber + (BoardSize + 1)].ExistPiece) return (true);
            if (pos.y != 1 && pos.x != 1)//左上
                if (!_tiles[TileNumber - (BoardSize + 1)].ExistPiece) return (true);
            if (pos.y != BoardSize && pos.x != 1)//左下
                if (!_tiles[TileNumber + (BoardSize - 1)].ExistPiece) return (true);
        }

        if (role == 3 || _tileManager.isHalfPieceDown)
        {
            if (pos.y > 2)//上上
                if (!_tiles[TileNumber - (BoardSize * 2)].ExistPiece) return (true);
            if (pos.y < (BoardSize - 1))//下下
                if (!_tiles[TileNumber + (BoardSize * 2)].ExistPiece) return (true);
            if (pos.x < (BoardSize - 1))//右右
                if (!_tiles[TileNumber + 2].ExistPiece) return (true);
            if (pos.x > 2)//左左
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
        if (pos.y != 1 && !_tiles[TileNumber - BoardSize].ExistPiece)//上
            _MoveDestination.Add(TileNumber - BoardSize);
        if (pos.y != BoardSize && !_tiles[TileNumber + BoardSize].ExistPiece)//下
            _MoveDestination.Add(TileNumber + BoardSize);
        if (pos.x != BoardSize && !_tiles[TileNumber + 1].ExistPiece)//右
            _MoveDestination.Add(TileNumber + 1);
        if (pos.x != 1 && !_tiles[TileNumber - 1].ExistPiece)//左
            _MoveDestination.Add(TileNumber - 1);
        if (role == 3 || role == 4 || _tileManager.isHalfPieceDown)
        {
            if (pos.y != 1 && pos.x != BoardSize && !_tiles[TileNumber - (BoardSize - 1)].ExistPiece)//右上
                _MoveDestination.Add(TileNumber - (BoardSize - 1));
            if (pos.y != BoardSize && pos.x != BoardSize && !_tiles[TileNumber + (BoardSize + 1)].ExistPiece)//右下
                _MoveDestination.Add(TileNumber + (BoardSize + 1));
            if (pos.y != 1 && pos.x != 1 && !_tiles[TileNumber - (BoardSize + 1)].ExistPiece)//左上
                _MoveDestination.Add(TileNumber - (BoardSize + 1));
            if (pos.y != BoardSize && pos.x != 1 && !_tiles[TileNumber + (BoardSize - 1)].ExistPiece)//左下
                _MoveDestination.Add(TileNumber + (BoardSize - 1));
        }
        if (role == 3 || _tileManager.isHalfPieceDown)
        {
            if (pos.y > 2 && !_tiles[TileNumber - (BoardSize * 2)].ExistPiece)//上上
                _MoveDestination.Add(TileNumber - (BoardSize * 2));
            if (pos.y < (BoardSize - 1) && !_tiles[TileNumber + (BoardSize * 2)].ExistPiece)//下下
                _MoveDestination.Add(TileNumber + (BoardSize * 2));
            if (pos.x < (BoardSize - 1) && !_tiles[TileNumber + 2].ExistPiece)//右右
                _MoveDestination.Add(TileNumber + 2);
            if (pos.x > 2 && !_tiles[TileNumber - 2].ExistPiece)//左左
                _MoveDestination.Add(TileNumber - 2);
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
            pos.x = tileNumber % BoardSize + 1;
            pos.y = tileNumber / BoardSize + 1;
        }
        int role = _info.Role;

        if (pos.y != 1)//上
            _AttackRangePosition.Add(TileNumber - BoardSize);
        if (pos.y != BoardSize)//下
            _AttackRangePosition.Add(TileNumber + BoardSize);
        if (pos.x != BoardSize)//右
            _AttackRangePosition.Add(TileNumber + 1);
        if (pos.x != 1)//左
            _AttackRangePosition.Add(TileNumber - 1);

        if (role == 1 || role == 2 || role == 4)
        {
            if (pos.y != 1 && pos.x != BoardSize)//右上
                _AttackRangePosition.Add(TileNumber - (BoardSize - 1));
            if (pos.y != BoardSize && pos.x != BoardSize)//右下
                _AttackRangePosition.Add(TileNumber + (BoardSize + 1));
            if (pos.y != 1 && pos.x != 1)//左上
                _AttackRangePosition.Add(TileNumber - (BoardSize + 1));
            if (pos.y != BoardSize && pos.x != 1)//左下
                _AttackRangePosition.Add(TileNumber + (BoardSize - 1));
        }
        if (role == 2)
        {
            if (pos.y > 2)//上上
                _AttackRangePosition.Add(TileNumber - (BoardSize * 2));
            if (pos.y < (BoardSize - 1))//下下
                _AttackRangePosition.Add(TileNumber + (BoardSize * 2));
            if (pos.x < (BoardSize - 1))//右右
                _AttackRangePosition.Add(TileNumber + 2);
            if (pos.x > 2)//左左
                _AttackRangePosition.Add(TileNumber - 2);
        }
        return;
    }

    private int DamagePrediction(PieceInfomation _allyInfo,PieceInfomation _enemyInfo)
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
        return (int)HitNum;
    }
}
