using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuffTileManager : MonoBehaviour
{
    private int BoardSize = 0;
    private bool isExistMovePlusTile = false;
    private int isAttackBuffTileCount = 0;
    private int isHealingTileCount = 0;

    [SerializeField] GameObject[] BuffPortions;
    [SerializeField] BuffEffectManager _effectManager;
    private TurnManager _turnManager;
    private PieceInfoDisplay _display;

    private Dictionary<int, GameObject> Portions = new Dictionary<int, GameObject>();

    private TileInfomation[] _tiles;
    private PieceInfomation[] _allPieceInfos;
    private PieceInfomation _pieceInfo;
    private PieceAnimation _pieceAnimes;
    private int TileNumber;

    private GameInfomation _infomation;

    void Start()
    {
        _turnManager = GetComponent<TurnManager>();
        _display = GetComponent<PieceInfoDisplay>();
        _infomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();

        BoardSize = _infomation.BoardSize;
        switch (BoardSize)
        {
            case 5:
                _tiles = new TileInfomation[25];
                _allPieceInfos = new PieceInfomation[14];
                break;
            case 6:
                _tiles = new TileInfomation[36];
                _allPieceInfos = new PieceInfomation[18];
                break;
            case 7:
                _tiles = new TileInfomation[49];
                _allPieceInfos = new PieceInfomation[22];
                break;
        }

        _tiles = GetComponentsInChildren<TileInfomation>();
        _allPieceInfos = GetComponentsInChildren<PieceInfomation>();
    }

    public void SetBuffTile(bool type, int num)
    {
        switch (num)
        {
            case 0:
                if (isExistMovePlusTile) return;
                isExistMovePlusTile = true;
                break;
            case 1:
                if (isAttackBuffTileCount >= 3) return;
                isAttackBuffTileCount++;
                break;
            case 2:
                if (isHealingTileCount >= 5) return;
                isHealingTileCount++;
                break;
        }
        List<int> canSetPos = new List<int>();
        if (type)
        {
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (_tiles[i + j * BoardSize].ExistPiece || _tiles[i + j * BoardSize].ExistBuff) continue;
                    canSetPos.Add(i + j * BoardSize);
                }
            }
        }
        else
        {
            int halfY = 4;
            if (BoardSize == 5)
                halfY = 3;

            for (int i = 0; i < BoardSize; i++)
            {
                if (_tiles[i + (halfY - 1) * BoardSize].ExistPiece || _tiles[i + (halfY - 1) * BoardSize].ExistBuff) continue;
                canSetPos.Add(i + (halfY - 1) * BoardSize);
            }
        }
        int setPos = canSetPos[Random.Range(0, canSetPos.Count)];

        float fontSize = 0f;
        Vector3 generatePos = new Vector3(0, 0, 0);
        switch (BoardSize)
        {
            case 5:
                fontSize = 0.045f;
                generatePos = new Vector3(4.579f + (setPos / BoardSize) * 0.2f, 0.83f, 4.097f + (setPos % BoardSize) * 0.2f);
                break;
            case 6:
                fontSize = 0.0375f;
                generatePos = new Vector3(4.559f + (setPos / BoardSize) * 0.168f, 0.83f, 4.077f + (setPos % BoardSize) * 0.168f);
                break;
            case 7:
                fontSize = 0.0325f;
                generatePos = new Vector3(4.55f + (setPos / BoardSize) * 0.143f, 0.83f, 4.067f + (setPos % BoardSize) * 0.143f);
                break;
        }
        _tiles[setPos].ExistBuff = true;
        var g = Instantiate(BuffPortions[num], generatePos, Quaternion.identity, transform);
        g.GetComponentInChildren<TextMeshProUGUI>().fontSize = fontSize;
        setPos += num * 100;
        Portions.Add(setPos, g);
    }

    //ポーション取得時
    public void GetPortion(int TileNumber, PieceInfomation _info, bool type = false)
    {
        StartCoroutine(portionEffect(TileNumber, _info, type));
    }

    WaitForSeconds wait = new WaitForSeconds(1.45f);
    private System.Collections.IEnumerator portionEffect(int TileNumber, PieceInfomation _info, bool type = false)
    {
        _tiles[TileNumber].ExistBuff = false;
        if (type)
            yield return wait;

        int portionNum = 0;
        //ポーションを消す
        for (int i = 0; i < Portions.Count; i++)
        {
            KeyValuePair<int, GameObject> pair = Portions.ElementAt(i);
            if ((pair.Key % 100) == TileNumber)
            {
                portionNum = pair.Key / 100;
                Portions.Remove(pair.Key);
                Destroy(pair.Value);
                break;
            }
        }
        yield return null;
        switch (portionNum)
        {
            case 0://移動回数
                isExistMovePlusTile = false;
                _turnManager.GetPlusMoveTile(_info.Side);
                _info.ChangeValue(5);
                break;
            case 1://攻撃UP
                isAttackBuffTileCount--;
                _info.ChangeValue(0);
                break;
            case 2://HP回復
                isHealingTileCount--;
                _info.ChangeValue(1);
                break;
        }
        yield return null;
        //駒の情報ディスプレイにも反映させる
        _display.ChangePieceInfo(_info);
        yield return null;
        //エフェクトの生成位置を決める
        Vector2 generatePos = new Vector3((TileNumber % BoardSize) + 1, (TileNumber / BoardSize) + 1);

        //エフェクトを出させる
        _effectManager.Active(generatePos, portionNum);
        yield break;
    }
}
