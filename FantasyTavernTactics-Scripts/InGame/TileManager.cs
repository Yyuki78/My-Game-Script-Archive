using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class TileManager : MonoBehaviour
{
    public bool isGameFinish { get; private set; } = false;
    public bool isWinSide { get; private set; } = false;

    private int BoardSize = 0;
    private int pieceDownCount = 0;
    public bool isHalfPieceDown { get; private set; } = false;

    public int CanAttackPieceCount { get; private set; } = 0;

    private float alpha_Sin;
    [SerializeField] Material[] TileMaterial = new Material[2];
    private int currentMaterialNum = 0;

    private GameInfomation _infomation;
    private ExplanationText _explanationText;
    private BattleSceneManagement _battleSceneManagement;
    private TurnManager _turnManager;
    private BuffTileManager _buffTileManager;

    private TileInfomation[] _tiles;
    private PieceInfomation[] _allPieceInfos;
    private PieceInfomation _pieceInfo;
    private PieceAnimation _pieceAnimes;
    private int TileNumber;

    private Vector2[] _beforePosition;
    private PieceAnimation[] _allPieceAnimes;

    public int MysteryBoxTileNumber = -1;//MysteryBoxManagerで変更　強風イベントで使う

    [SerializeField] GameObject[] FinishLights = new GameObject[2];//ゲーム終了時の最後の攻撃の時に使うスポットライト

    private Coroutine _checkCoroutine;

    private WaitAttackEndText _waitText;

    [SerializeField] TextMeshProUGUI canAttackPieceCountText;
    private string attackableCountText;

    void Awake()
    {
        _infomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        _explanationText = GameObject.FindGameObjectWithTag("GameController").GetComponent<ExplanationText>();
        _battleSceneManagement = GetComponentInParent<BattleSceneManagement>();
        _turnManager = GetComponent<TurnManager>();
        _buffTileManager = GetComponent<BuffTileManager>();

        BoardSize = _infomation.BoardSize;
        switch (BoardSize)
        {
            case 5:
                _tiles = new TileInfomation[25];
                _allPieceInfos = new PieceInfomation[14];
                _beforePosition = new Vector2[14];
                _allPieceAnimes = new PieceAnimation[14];
                break;
            case 6:
                _tiles = new TileInfomation[36];
                _allPieceInfos = new PieceInfomation[18];
                _beforePosition = new Vector2[18];
                _allPieceAnimes = new PieceAnimation[18];
                break;
            case 7:
                _tiles = new TileInfomation[49];
                _allPieceInfos = new PieceInfomation[22];
                _beforePosition = new Vector2[22];
                _allPieceAnimes = new PieceAnimation[22];
                break;
        }

        _tiles = GetComponentsInChildren<TileInfomation>();
        _allPieceInfos = GetComponentsInChildren<PieceInfomation>();
        _allPieceAnimes = GetComponentsInChildren<PieceAnimation>();
        FinishLights[0].SetActive(false);
        FinishLights[1].SetActive(false);
        _waitText = GetComponent<WaitAttackEndText>();
        canAttackPieceCountText.gameObject.SetActive(false);
        attackableCountText = LocalizationSettings.StringDatabase.GetLocalizedString(tableReference: "TextData", tableEntryReference: "AttackablePiece");
    }

    private void LateUpdate()
    {
        alpha_Sin = Mathf.Sin(Time.time * 2 * Mathf.PI) / 2 + 0.5f;
        Color _color = TileMaterial[currentMaterialNum].color;
        _color.a = alpha_Sin;
        TileMaterial[currentMaterialNum].color = _color;
    }

    public void CheckMove(PieceInfomation _info)
    {
        if (!_turnManager.canMove) return;
        _pieceInfo = _info;
        Vector2 pos = _info.CurrentPosition;
        int role = _info.Role;
        for (int i = 0; i < _tiles.Length; i++)
        {
            if (_tiles[i].isBlinking)
                _tiles[i].StopBlinking();
        }
        TileNumber = (int)pos.x - 1 + (int)((pos.y - 1) * BoardSize);

        if (pos.y != 1)//上
            _tiles[TileNumber - BoardSize].Blinking();
        if (pos.y != BoardSize)//下
            _tiles[TileNumber + BoardSize].Blinking();
        if (pos.x != BoardSize)//右
            _tiles[TileNumber + 1].Blinking();
        if (pos.x != 1)//左
            _tiles[TileNumber - 1].Blinking();
        if (role == 3 || role == 4 || isHalfPieceDown)
        {
            if (pos.y != 1 && pos.x != BoardSize)//右上
                _tiles[TileNumber - (BoardSize - 1)].Blinking();
            if (pos.y != BoardSize && pos.x != BoardSize)//右下
                _tiles[TileNumber + (BoardSize + 1)].Blinking();
            if (pos.y != 1 && pos.x != 1)//左上
                _tiles[TileNumber - (BoardSize + 1)].Blinking();
            if (pos.y != BoardSize && pos.x != 1)//左下
                _tiles[TileNumber + (BoardSize - 1)].Blinking();
        }
        if (role == 3 || isHalfPieceDown)
        {
            if (pos.y > 2)//上上
                _tiles[TileNumber - (BoardSize * 2)].Blinking();
            if (pos.y < (BoardSize - 1))//下下
                _tiles[TileNumber + (BoardSize * 2)].Blinking();
            if (pos.x < (BoardSize - 1))//右右
                _tiles[TileNumber + 2].Blinking();
            if (pos.x > 2)//左左
                _tiles[TileNumber - 2].Blinking();
        }
    }

    public void MovePiece(PieceInfomation pieceInfo, TileInfomation tileInfo)
    {
        if (_pieceInfo.CurrentPosition != pieceInfo.CurrentPosition) return;
        for (int i = 0; i < _tiles.Length; i++)
        {
            if (_tiles[i].isBlinking)
                _tiles[i].StopBlinking();
        }
        tileInfo.ExistPiece = true;
        _tiles[TileNumber].ExistPiece = false;
        _pieceInfo.GetComponentInChildren<PieceAnimation>().Move(tileInfo.myPosition, false);
        int tileNumber= (int)tileInfo.myPosition.x - 1 + (int)((tileInfo.myPosition.y - 1) * BoardSize);
        CheckBuffTile(_pieceInfo, tileNumber);
        _turnManager.MinusMoveCount();
    }

    public void CheckAttack(PieceInfomation _info)
    {
        if (_checkCoroutine != null)
        {
            StopCoroutine(_checkCoroutine);
            _checkCoroutine = null;
        }
        _checkCoroutine = StartCoroutine(checkAttack(_info));
    }

    public IEnumerator checkAttack(PieceInfomation _info)
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            if (_tiles[i].isColorChange)
                _tiles[i].ColorChangeBefore();
        }
        if(isGameFinish) yield break;
        if (!_info.CanAttack) yield break;
        _pieceInfo = _info;
        Vector2 pos = _info.CurrentPosition;
        int role = _info.Role;
        yield return null;
        TileNumber = (int)pos.x - 1 + (int)((pos.y - 1) * BoardSize);
        if (pos.y != 1)//上
            _tiles[TileNumber - BoardSize].ColorChangeRed();
        if (pos.y != BoardSize)//下
            _tiles[TileNumber + BoardSize].ColorChangeRed();
        if (pos.x != BoardSize)//右
            _tiles[TileNumber + 1].ColorChangeRed();
        if (pos.x != 1)//左
            _tiles[TileNumber - 1].ColorChangeRed();
        if (role == 1 || role == 2 || role == 4)
        {
            if (pos.y != 1 && pos.x != BoardSize)//右上
                _tiles[TileNumber - (BoardSize - 1)].ColorChangeRed();
            if (pos.y != BoardSize && pos.x != BoardSize)//右下
                _tiles[TileNumber + (BoardSize + 1)].ColorChangeRed();
            if (pos.y != 1 && pos.x != 1)//左上
                _tiles[TileNumber - (BoardSize + 1)].ColorChangeRed();
            if (pos.y != BoardSize && pos.x != 1)//左下
                _tiles[TileNumber + (BoardSize - 1)].ColorChangeRed();
        }
        if (role == 2)
        {
            if (pos.y > 2)//上上
                _tiles[TileNumber - (BoardSize * 2)].ColorChangeRed();
            if (pos.y < (BoardSize - 1))//下下
                _tiles[TileNumber + (BoardSize * 2)].ColorChangeRed();
            if (pos.x < (BoardSize - 1))//右右
                _tiles[TileNumber + 2].ColorChangeRed();
            if (pos.x > 2)//左左
                _tiles[TileNumber - 2].ColorChangeRed();
        }
        yield break;
    }

    public void AttackPiece(PieceInfomation _allyInfo, PieceInfomation _enemyInfo)
    {
        //State確認
        if (_turnManager._currentState == TurnManager.GameState.Attack)
        {
            if (_pieceInfo.CurrentPosition != _allyInfo.CurrentPosition) return;
        }
        else if (_turnManager._currentState == TurnManager.GameState.EnemyAttack)
            _pieceInfo = _allyInfo;
        else
            return;
        //位置確認
        Vector2 pos = _enemyInfo.CurrentPosition;
        TileNumber = (int)pos.x - 1 + (int)((pos.y - 1) * BoardSize);
        if (!_tiles[TileNumber].ExistPiece) return;
        if (_turnManager._currentState == TurnManager.GameState.Attack)
            if (!_tiles[TileNumber].isColorChange) return;
        //攻撃可能確認
        if (!_allyInfo.CanAttack) return;
        _allyInfo.ChangeCanAttack(false);
        if (_allyInfo.isDie || _enemyInfo.isDie) return;
        _pieceInfo = _allyInfo;

        if (!_enemyInfo.isCanAttacked)
        {
            AudioManager.Instance.SE(17);
            _waitText.ShowText(_enemyInfo);
            _allyInfo.ChangeCanAttack(true);
            return;
        }
        //ファイター確認
        if (_pieceInfo.isFighter && !_battleSceneManagement.rejectSceneChange && _enemyInfo.Role != 5)
        {
            _battleSceneManagement.AskAttackScene(_pieceInfo, _enemyInfo);
            return;
        }
        if (_battleSceneManagement.rejectSceneChange)
            _battleSceneManagement.rejectSceneChange = false;

        for (int i = 0; i < _tiles.Length; i++)
        {
            if (_tiles[i].isColorChange)
                _tiles[i].ColorChangeBefore();
        }

        //ダメージ計算
        float Attack = _pieceInfo.Attack;
        float Defence = _enemyInfo.Defense;
        if (_pieceInfo.Role == 2)
        {
            Defence = _enemyInfo.MagicDefense;
        }
        float HitNum = 0;
        int AttributeAdvantage = 0;
        switch (_pieceInfo.Attribute)
        {
            case 0:
                if (_enemyInfo.Attribute == 0 || _enemyInfo.Attribute == 3)
                    AttributeAdvantage = 1;
                else if (_enemyInfo.Attribute == 1)
                    AttributeAdvantage = 0;
                else
                    AttributeAdvantage = 2;
                break;
            case 1:
                if (_enemyInfo.Attribute == 0)
                    AttributeAdvantage = 2;
                else if (_enemyInfo.Attribute == 1 || _enemyInfo.Attribute == 3)
                    AttributeAdvantage = 1;
                else
                    AttributeAdvantage = 0;
                break;
            case 2:
                if (_enemyInfo.Attribute == 0)
                    AttributeAdvantage = 0;
                else if (_enemyInfo.Attribute == 1)
                    AttributeAdvantage = 2;
                else
                    AttributeAdvantage = 1;
                break;
            case 3:
                AttributeAdvantage = 1;
                break;
        }
        switch (AttributeAdvantage)
        {
            case 0:
                HitNum = Random.Range(20, 30);//不利
                break;
            case 1:
                HitNum = Random.Range(30, 50);//同じ
                break;
            case 2:
                HitNum = Random.Range(50, 70);//有利
                break;
        }
        HitNum *= (Attack / Defence);

        //攻撃側回転0=上,1=右上,2=右,3=右下,4=下,5=左下,6=左,7=左上
        int rotateNum = -1;
        if (_pieceInfo.CurrentPosition.x < _enemyInfo.CurrentPosition.x)
        {
            if (_pieceInfo.CurrentPosition.y > _enemyInfo.CurrentPosition.y)
                rotateNum = 1;
            else if (_pieceInfo.CurrentPosition.y == _enemyInfo.CurrentPosition.y)
                rotateNum = 2;
            else
                rotateNum = 3;
        }
        else if(_pieceInfo.CurrentPosition.x == _enemyInfo.CurrentPosition.x)
        {
            if (_pieceInfo.CurrentPosition.y > _enemyInfo.CurrentPosition.y)
                rotateNum = 0;
            else
                rotateNum = 4;
        }
        else
        {
            if (_pieceInfo.CurrentPosition.y > _enemyInfo.CurrentPosition.y)
                rotateNum = 7;
            else if (_pieceInfo.CurrentPosition.y == _enemyInfo.CurrentPosition.y)
                rotateNum = 6;
            else
                rotateNum = 5;
        }
        _enemyInfo.GetComponentInChildren<PieceAnimation>().FaceTarget(rotateNum);
        _pieceInfo.GetComponentInChildren<PieceAnimation>().Attacking(AttributeAdvantage, (int)HitNum, rotateNum, _enemyInfo.GetComponentInChildren<PieceAnimation>());
        CheckDown(_enemyInfo, (int)HitNum);
        CalculateCanAttackPieceCount();
    }

    public void CheckDown(PieceInfomation _enemyInfo, int HitNum)
    {
        _enemyInfo.ReduceHP(HitNum);
        if (_enemyInfo.HP <= 0)
        {
            if (_enemyInfo.Role != 5)
                pieceDownCount++;
            _tiles[(int)_enemyInfo.CurrentPosition.x - 1 + (int)((_enemyInfo.CurrentPosition.y - 1) * BoardSize)].ExistPiece = false;
            _enemyInfo.Die();
            if (_enemyInfo.Role == 4)
            {
                isGameFinish = true;
                FinishLightEffect(!_enemyInfo.Side, _enemyInfo);
            }
            if (pieceDownCount == (_allPieceInfos.Length / 2) && _infomation.isHalfwayBoost)
                isHalfPieceDown = true;
        }
    }

    public void CheckBuffTile(PieceInfomation _Info, int tileNumber, bool type = false)
    {
        if (_tiles[tileNumber].ExistBuff)
            _buffTileManager.GetPortion(tileNumber, _Info, type);
    }

    //強風によって全ての駒が上下左右のどれか1マス動かされる
    public void StrongWindEvent(int direction)
    {
        StartCoroutine(strongWindEvent(direction));
    }

    private IEnumerator strongWindEvent(int direction)
    {
        List<PieceInfomation> movePiece = new List<PieceInfomation>();
        List<Vector2> movePos = new List<Vector2>();

        List<float> exhibitTileNumbers = new List<float>();
        List<float> nextExhibitTileNumbers = new List<float>();
        bool cantMove = false;

        Vector2 windVec = new Vector2(0, -1);
        bool reverse = false;
        if (direction >= 2)
            windVec = new Vector2(-1, 0);
        if (direction % 2 != 0)
        {
            reverse = true;
            windVec = -windVec;
        }

        Vector2 mysteryBoxPos = new Vector2(-1, -1);
        if (MysteryBoxTileNumber != -1)
            mysteryBoxPos = new Vector2((MysteryBoxTileNumber % BoardSize) + 1, (MysteryBoxTileNumber / BoardSize) + 1);
        yield return null;
        //盤の行or列ごとに行う
        for (int i = (reverse ? BoardSize : 1); (reverse ? i > 0 : i < BoardSize + 1); i += (reverse ? -1 : 1))
        {
            exhibitTileNumbers = nextExhibitTileNumbers;
            nextExhibitTileNumbers = new List<float>();//List初期化用

            if (mysteryBoxPos[(int)Mathf.Abs(windVec[1])] == i)//MysteryBoxがある
            {
                nextExhibitTileNumbers.Add(mysteryBoxPos[(int)Mathf.Abs(windVec[0])]);
                //Debug.Log("ミステリーボックスがあるので移動できません" + MysteryBoxTileNumber);
            }

            for (int j = 0; j < _allPieceInfos.Length; j++)
            {
                cantMove = false;
                if (_allPieceInfos[j].isDie) continue;
                if (_allPieceInfos[j].CurrentPosition[(int)Mathf.Abs(windVec[1])] == i)
                {
                    if (i == (reverse ? BoardSize : 1))//駒が端にいる
                    {
                        nextExhibitTileNumbers.Add(_allPieceInfos[j].CurrentPosition[(int)Mathf.Abs(windVec[0])]);
                        continue;
                    }
                    for (int k = 0; k < exhibitTileNumbers.Count; k++)
                    {
                        if (_allPieceInfos[j].CurrentPosition[(int)Mathf.Abs(windVec[0])] == exhibitTileNumbers[k])//駒の移動先に駒がいる
                        {
                            nextExhibitTileNumbers.Add(_allPieceInfos[j].CurrentPosition[(int)Mathf.Abs(windVec[0])]);
                            cantMove = true;
                        }
                    }
                    if (cantMove) continue;
                    //どちらもないなら移動できる
                    movePiece.Add(_allPieceInfos[j]);
                    movePos.Add(_allPieceInfos[j].CurrentPosition + windVec);
                    _tiles[(int)_allPieceInfos[j].CurrentPosition.x - 1 + (int)((_allPieceInfos[j].CurrentPosition.y - 1) * BoardSize)].ExistPiece = false;
                }
            }
            yield return null;
        }
        for (int i = 0; i < movePiece.Count; i++)//移動させる
        {
            movePiece[i].GetComponentInChildren<PieceAnimation>().Move(movePos[i], true);
            TileNumber = (int)movePos[i].x - 1 + (int)((movePos[i].y - 1) * BoardSize);
            CheckBuffTile(movePiece[i], TileNumber, true);
            _tiles[TileNumber].ExistPiece = true;
        }
        yield break;
    }

    //どちらかの指揮官が倒れる時のライト演出
    private void FinishLightEffect(bool side, PieceInfomation _enemyInfo)
    {
        AudioManager.Instance.StopBGM(0);
        isWinSide = side;
        _pieceInfo.GetComponentInChildren<PieceAnimation>().LockRotation();
        Vector3 attackPos = new Vector3(0, 0, 0);
        Vector3 attackedPos = new Vector3(0, 0, 0);
        switch (BoardSize)
        {
            case 5:
                attackPos = new Vector3(4.579f + (_pieceInfo.CurrentPosition.y - 1) * 0.2f, 1.65f, 4.097f + (_pieceInfo.CurrentPosition.x - 1) * 0.2f);
                attackedPos = new Vector3(4.579f + (_enemyInfo.CurrentPosition.y - 1) * 0.2f, 1.65f, 4.097f + (_enemyInfo.CurrentPosition.x - 1) * 0.2f);
                break;
            case 6:
                attackPos = new Vector3(4.559f + (_pieceInfo.CurrentPosition.y - 1) * 0.168f, 1.65f, 4.077f + (_pieceInfo.CurrentPosition.x - 1) * 0.168f);
                attackedPos = new Vector3(4.559f + (_enemyInfo.CurrentPosition.y - 1) * 0.168f, 1.65f, 4.077f + (_enemyInfo.CurrentPosition.x - 1) * 0.168f);
                break;
            case 7:
                attackPos = new Vector3(4.55f + (_pieceInfo.CurrentPosition.y - 1) * 0.143f, 1.65f, 4.067f + (_pieceInfo.CurrentPosition.x - 1) * 0.143f);
                attackedPos = new Vector3(4.55f + (_enemyInfo.CurrentPosition.y - 1) * 0.143f, 1.65f, 4.067f + (_enemyInfo.CurrentPosition.x - 1) * 0.143f);
                break;
        }
        FinishLights[0].transform.position = attackPos;
        FinishLights[0].SetActive(true);
        FinishLights[1].transform.position = attackedPos;
        FinishLights[1].SetActive(true);
    }

    //リセット時用に元のポジションを保存する
    public void SavePiecePosition()
    {
        for(int i = 0; i < _allPieceInfos.Length; i++)
        {
            if (_allPieceInfos[i].isDie) continue;
            _beforePosition[i] = _allPieceInfos[i].CurrentPosition;
        }
    }

    public void ResetMove()
    {
        for (int i = 0; i < _tiles.Length; i++)
        {
            if (_tiles[i].ExistPiece && MysteryBoxTileNumber != i)
                _tiles[i].ExistPiece = false;
        }
        for (int i = 0; i < _allPieceInfos.Length; i++)
        {
            if (_allPieceInfos[i].isDie) continue;
            _allPieceAnimes[i].Move(_beforePosition[i], false);
            TileNumber = (int)_beforePosition[i].x - 1 + (int)((_beforePosition[i].y - 1) * BoardSize);
            _tiles[TileNumber].ExistPiece = true;
        }
    }

    public void StopBlinking(int type = 0)
    {
        if (type == 0) //移動フェーズ終了
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                if (_tiles[i].isBlinking)
                    _tiles[i].StopBlinking();
            }
            currentMaterialNum = 1;
            canAttackPieceCountText.gameObject.SetActive(true);
            CalculateCanAttackPieceCount();
        }
        else if (type == 1) //自分のターン終了
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                if (_tiles[i].isColorChange)
                    _tiles[i].ColorChangeBefore();
            }
            for (int i = 0; i < _allPieceInfos.Length; i++)
            {
                if (_allPieceInfos[i].isDie) continue;
                _allPieceInfos[i].ChangeCanAttack(true);
            }
            currentMaterialNum = 0;
            canAttackPieceCountText.gameObject.SetActive(false);
            CanAttackPieceCount = 0;
        }
        else if (type == 2) //戦闘シーン移行選択時
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                if (_tiles[i].isColorChange)
                    _tiles[i].ColorChangeBefore();
            }
        }
    }

    //攻撃可能な駒の数を計算して表示する
    private List<int> _PiecePosition = new List<int>();
    private List<int> _AttackRangePosition = new List<int>();
    private List<PieceInfomation> _CanAttackPiece = new List<PieceInfomation>();
    public void CalculateCanAttackPieceCount()
    {
        _PiecePosition.Clear();
        _CanAttackPiece.Clear();
        //駒の位置を格納する(敵駒のみ)
        for (int i = 0; i < _allPieceInfos.Length; i++)
        {
            if (_allPieceInfos[i].Side) continue;
            if (_allPieceInfos[i].isDie) continue;
            _PiecePosition.Add((int)_allPieceInfos[i].CurrentPosition.x - 1 + (int)((_allPieceInfos[i].CurrentPosition.y - 1) * BoardSize));
        }

        //移動せずに攻撃可能な駒の選別
        for (int i = 0; i < _allPieceInfos.Length; i++)
        {
            if (!_allPieceInfos[i].Side) continue;
            if (_allPieceInfos[i].isDie) continue;
            if (!_allPieceInfos[i].CanAttack) continue;
            _AttackRangePosition.Clear();
            StoreAttackRangePosition(_allPieceInfos[i]);

            List<int> hitList = _PiecePosition.FindAll(_AttackRangePosition.Contains);
            if (hitList.Count != 0)
                _CanAttackPiece.Add(_allPieceInfos[i]);
        }

        canAttackPieceCountText.text = attackableCountText + _CanAttackPiece.Count.ToString();
        CanAttackPieceCount = _CanAttackPiece.Count;
    }

    //攻撃範囲の格納
    private void StoreAttackRangePosition(PieceInfomation _info)
    {
        Vector2 pos = _info.CurrentPosition;
        int role = _info.Role;
        TileNumber = (int)pos.x - 1 + (int)((pos.y - 1) * BoardSize);

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
}
