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

    private List<PieceInfomation> _CanMovePiece = new List<PieceInfomation>();//移動させる可能性のある駒
    private List<PieceInfomation> _CanAttackPiece = new List<PieceInfomation>();//その場で攻撃できる駒
    private List<PieceInfomation> _DisAdvPiece = new List<PieceInfomation>();//その場で攻撃できるが相性不利な駒

    private float moveEvaluate;
    private List<float> _MoveEvaluateValue = new List<float>();//移動先の評価値
    private List<PieceInfomation> _MoveEvaluatePiece = new List<PieceInfomation>();//評価値の元(駒情報)
    private List<int> _MoveEvaluatePosition = new List<int>();//評価値の元(移動先)

    //以下は使いまわす
    private List<int> _MoveDestination = new List<int>();
    private List<int> _NextMoveDestination = new List<int>();//二回移動の考慮用
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

        //確率操作をする20%,40%,40%
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

            if (isRandom1)//ほぼランダム移動
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
                //取得した駒からランダムに一つ取得
                _movePieceInfo = _CanMovePiece[UnityEngine.Random.Range(0, _CanMovePiece.Count)];
                yield return null;
                //移動先を全て取得
                StoreMoveDestination();
                yield return null;
                //取得した移動先からランダムに一つ選ぶ
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

            //全ての駒の位置を格納する
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
                StoreAttackRangePosition(_allPieceInfos[i]);
                yield return null;
                List<int> hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
                yield return null;
                if (hitList.Count != 0)//その場で攻撃可能
                {
                    //攻撃対象との相性判定
                    for (int k = 0; k < hitList.Count; k++)
                    {
                        //相性不利でないなら追加
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
            //移動可能駒の選別
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                //AI駒＋死んでない＋その場で攻撃可能だが相性不利orその場で攻撃可能でない
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
            _MoveEvaluateValue.Clear();
            _MoveEvaluatePiece.Clear();
            _MoveEvaluatePosition.Clear();

            //取得した駒ごとに移動先の評価を行い、評価値で分けて格納する
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
                    if (hitList.Count != 0)//移動先で駒が攻撃可能
                    {
                        //攻撃対象との相性判定
                        for (int k = 0; k < hitList.Count; k++)
                        {
                            if (AttributeCompatibilityCheck(_CanMovePiece[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList[k])]) == 1)
                            {
                                moveEvaluate = 3.25f + RoleEvaluation(0, _CanMovePiece[i]);
                                if (_allPieceInfos[_AllPiecePosition.IndexOf(hitList[k])].Role == 4)//対象が指揮官ならその移動になる
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
                        //移動先が後ろに下がるものなら+0.5f、前なら+0.25f
                        if (_CanMovePiece[i].CurrentPosition.y > (_MoveDestination[j] / BoardSize) + 1)
                            moveEvaluate += 0.5f;
                        if (_CanMovePiece[i].CurrentPosition.y < (_MoveDestination[j] / BoardSize) + 1)
                            moveEvaluate += 0.25f;
                    }
                    else//移動先で攻撃可能でない
                    {
                        //もう一度移動した場合に攻撃可能か調べる
                        StoreNextMoveDestination(_CanMovePiece[i], _MoveDestination[j]);
                        for (int l = 0; l < _NextMoveDestination.Count; l++)
                        {
                            _AttackRangePosition.Clear();
                            StoreAttackRangePosition(_CanMovePiece[i], true, _NextMoveDestination[l]);

                            List<int> hitList2 = new List<int>();
                            hitList2 = _PiecePosition.FindAll(_AttackRangePosition.Contains);
                            yield return null;
                            if (hitList2.Count != 0)//移動先で駒が攻撃可能
                            {
                                //攻撃対象との相性判定
                                for (int k = 0; k < hitList2.Count; k++)
                                {
                                    if (AttributeCompatibilityCheck(_CanMovePiece[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList2[k])]) == 1)
                                    {
                                        moveEvaluate = 1.75f + RoleEvaluation(1, _CanMovePiece[i]);
                                        if (_allPieceInfos[_AllPiecePosition.IndexOf(hitList2[k])].Role == 4)//対象が指揮官なら評価UP
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
                    //結果を格納
                    _MoveEvaluateValue.Add(moveEvaluate);
                    _MoveEvaluatePiece.Add(_CanMovePiece[i]);
                    _MoveEvaluatePosition.Add(_MoveDestination[j]);
                    yield return null;
                }
            }
            //評価値をソート
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

            //評価結果から移動させる駒を決める(同値はランダム)
            int sameValueNum = 0;
            while (_MoveEvaluateValue[0] == _MoveEvaluateValue[sameValueNum])
            {
                sameValueNum++;
                if (_MoveEvaluateValue.Count == sameValueNum)
                    break;
            }
            int pieceNumber = UnityEngine.Random.Range(0, sameValueNum);
            /*
            Debug.Log("同じ評価値の数：" + sameValueNum);
            Debug.Log("動かす駒は----------------------------------------------------------------");
            Debug.Log("評価値：" + _MoveEvaluateValue[pieceNumber]);
            Debug.Log("評価駒：" + _MoveEvaluatePiece[pieceNumber]);
            Debug.Log("評価位置：" + _MoveEvaluatePosition[pieceNumber]);
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
            //攻撃可能で攻撃対象が単数の場合はそのまま攻撃
            if (hitList.Count != 0)
            {
                if(hitList.Count == 1)//攻撃対象が単数
                {
                    //Debug.Log("AIが攻撃します");
                    if (!_allPieceInfos[_AllPiecePosition.IndexOf(hitList[0])].isCanAttacked)
                        yield return wait; yield return wait;
                    _tileManager.AttackPiece(_allPieceInfos[i], _allPieceInfos[_AllPiecePosition.IndexOf(hitList[0])]);
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
                    continue;
                }
                _CanAttackPiece.Add(_allPieceInfos[i]);//それ以外の攻撃可能駒
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
                //Debug.Log("AIが攻撃します");
                int attackPosition, enemyNum;
                //攻撃先の中で最も与えるダメージが大きい相手を狙う(駒を倒せる場合は優先)
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
                //指揮官が含まれる場合は最優先で狙う
                for (int k = 0; k < hitList.Count; k++)
                {
                    if (_allPieceInfos[_AllPiecePosition.IndexOf(hitList[k])].Role == 4)//攻撃対象に指揮官が含まれるか
                        attackPosition = hitList[k];
                }
                yield return null;

                //攻撃先の要素番号
                enemyNum = _AllPiecePosition.IndexOf(attackPosition);
                if (!_allPieceInfos[enemyNum].isCanAttacked)
                    yield return wait; yield return wait;
                _tileManager.AttackPiece(_CanAttackPiece[i], _allPieceInfos[enemyNum]);
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

    //現在地からの移動先を格納
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

    //移動先からの移動先を格納(二回移動の考慮用)
    private void StoreNextMoveDestination(PieceInfomation _info, int MovePosNumber)
    {
        if (!_manager.canMove) return;
        Vector2 pos;
        pos.x = (MovePosNumber % BoardSize) + 1;
        pos.y = (MovePosNumber / BoardSize) + 1;
        int role = _info.Role;
        TileNumber = MovePosNumber;

        _NextMoveDestination.Clear();
        if (pos.y != 1 && !_tiles[TileNumber - BoardSize].ExistPiece)//上
            _NextMoveDestination.Add(TileNumber - BoardSize);
        if (pos.y != BoardSize && !_tiles[TileNumber + BoardSize].ExistPiece)//下
            _NextMoveDestination.Add(TileNumber + BoardSize);
        if (pos.x != BoardSize && !_tiles[TileNumber + 1].ExistPiece)//右
            _NextMoveDestination.Add(TileNumber + 1);
        if (pos.x != 1 && !_tiles[TileNumber - 1].ExistPiece)//左
            _NextMoveDestination.Add(TileNumber - 1);
        if (role == 3 || role == 4 || _tileManager.isHalfPieceDown)
        {
            if (pos.y != 1 && pos.x != BoardSize && !_tiles[TileNumber - (BoardSize - 1)].ExistPiece)//右上
                _NextMoveDestination.Add(TileNumber - (BoardSize - 1));
            if (pos.y != BoardSize && pos.x != BoardSize && !_tiles[TileNumber + (BoardSize + 1)].ExistPiece)//右下
                _NextMoveDestination.Add(TileNumber + (BoardSize + 1));
            if (pos.y != 1 && pos.x != 1 && !_tiles[TileNumber - (BoardSize + 1)].ExistPiece)//左上
                _NextMoveDestination.Add(TileNumber - (BoardSize + 1));
            if (pos.y != BoardSize && pos.x != 1 && !_tiles[TileNumber + (BoardSize - 1)].ExistPiece)//左下
                _NextMoveDestination.Add(TileNumber + (BoardSize - 1));
        }
        if (role == 3 || _tileManager.isHalfPieceDown)
        {
            if (pos.y > 2 && !_tiles[TileNumber - (BoardSize * 2)].ExistPiece)//上上
                _NextMoveDestination.Add(TileNumber - (BoardSize * 2));
            if (pos.y < (BoardSize - 1) && !_tiles[TileNumber + (BoardSize * 2)].ExistPiece)//下下
                _NextMoveDestination.Add(TileNumber + (BoardSize * 2));
            if (pos.x < (BoardSize - 1) && !_tiles[TileNumber + 2].ExistPiece)//右右
                _NextMoveDestination.Add(TileNumber + 2);
            if (pos.x > 2 && !_tiles[TileNumber - 2].ExistPiece)//左左
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

    //相性判定
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

    //攻撃予測ダメージ
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
        if (_enemyInfo.HP - HitNum <= 0)//倒せる
            HitNum = 300;
        HitNum += (RoleEvaluation(3, _enemyInfo) * 4);
        return (int)HitNum;
    }

    //役職ポイント
    private float RoleEvaluation(int mode, PieceInfomation _Info)
    {
        if (isRandom2 && mode != 3) return 0.5f;
        if (mode == 0)//移動1
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
        else if (mode == 1)//移動2
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
        else if (mode == 2)//移動3
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
        else//攻撃
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
