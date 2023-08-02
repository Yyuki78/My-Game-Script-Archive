using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MysteryBoxManager : MonoBehaviour
{
    private int BoardSize = 0;

    private bool isActiveBox = false;
    private int DestroyLimitNum = 4;

    [SerializeField] GameObject MysteryBox;
    [SerializeField] PieceInfomation _boxInfomation;
    [SerializeField] PieceAnimation _boxAnimation;
    private TextMeshProUGUI _limitTurnText;
    private TextMeshProUGUI _remainingHPText;
    private AudioSource _audio;

    [SerializeField] GameObject BoxMesh;
    [SerializeField] GameObject DestroyEffect;
    [SerializeField] GameObject[] DestroyParticle = new GameObject[2];

    [SerializeField] BuffEffectManager _effectManager;

    [SerializeField] AudioClip[] clip = new AudioClip[3];

    private TileInfomation[] _tiles;
    private PieceInfomation[] _allPieceInfos;
    private PieceInfomation _pieceInfo;
    private PieceAnimation _pieceAnimes;
    private int TileNumber;

    private GameInfomation _infomation;
    private TileManager _tileManager;

    void Awake()
    {
        var texts = MysteryBox.GetComponentsInChildren<TextMeshProUGUI>();
        _limitTurnText = texts[0];
        _remainingHPText = texts[1];
        _remainingHPText.gameObject.SetActive(false);
        _audio = MysteryBox.GetComponent<AudioSource>();

        MysteryBox.SetActive(false);
        _infomation = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>();
        _tileManager = GetComponent<TileManager>();

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

    //出現
    public void SetMysteryBox()
    {
        StartCoroutine(setMysteryBox());
    }

    private IEnumerator setMysteryBox()
    {
        isActiveBox = true;
        MysteryBox.SetActive(true);
        int halfY = 4;
        if (BoardSize == 5)
            halfY = 3;
        List<int> canSetPos = new List<int>();
        yield return null;
        _audio.PlayOneShot(clip[0]);

        do
        {
            for (int i = 0; i < BoardSize; i++)
            {
                if (_tiles[i + (halfY - 1) * BoardSize].ExistPiece || _tiles[i + (halfY - 1) * BoardSize].ExistBuff) continue;
                canSetPos.Add(i + (halfY - 1) * BoardSize);
            }
            switch (halfY)
            {
                case 1:
                    halfY = 6;
                    break;
                case 2:
                    halfY = 5;
                    if (BoardSize == 7)
                        halfY = 7;
                    break;
                case 3:
                    halfY = 4;
                    if (BoardSize == 7)
                        halfY = 6;
                    break;
                case 4:
                    halfY = 2;
                    if (BoardSize == 7)
                        halfY = 5;
                    break;
                case 5:
                    halfY = 1;
                    if (BoardSize == 7)
                        halfY = 3;
                    break;
                case 6:
                    halfY = 2;
                    break;
                case 7:
                    halfY = 1;
                    break;
            }
        } while (canSetPos.Count == 0);
        yield return null;

        int setPos = canSetPos[Random.Range(0, canSetPos.Count)];
        Vector2 generatePos = new Vector2((setPos % BoardSize) + 1, (setPos / BoardSize) + 1);
        _boxInfomation.CurrentPosition = generatePos;
        _boxAnimation.Move(generatePos, false);
        _tiles[setPos].ExistPiece = true;
        _tileManager.MysteryBoxTileNumber = setPos;

        _limitTurnText.text = DestroyLimitNum.ToString();
        yield break;
    }

    //自壊までのカウントダウン
    public void MinusCountDown()
    {
        if (isActiveBox)
        {
            DestroyLimitNum--;
            if (DestroyLimitNum == 0)
            {
                DestroyMysteryBox(false);
                _limitTurnText.gameObject.SetActive(false);
                return;
            }
            _limitTurnText.text = DestroyLimitNum.ToString();
        }
    }

    //箱が攻撃された
    public void AttackedBox()
    {
        if (!_remainingHPText.gameObject.activeSelf)
            _remainingHPText.gameObject.SetActive(true);
        _remainingHPText.text = _boxInfomation.HP.ToString() + "/250";
    }

    //破壊or自壊
    public void DestroyMysteryBox(bool type)
    {
        if (!isActiveBox) return;
        isActiveBox = false;
        _tileManager.MysteryBoxTileNumber = -1;
        if (type)
        {
            BuffPiece(true);
        }
        else
        {
            _audio.PlayOneShot(clip[2]);
            BuffPiece(false);
            _tiles[(int)_boxInfomation.CurrentPosition.x - 1 + ((int)_boxInfomation.CurrentPosition.y - 1) * BoardSize].ExistPiece = false;
            _boxInfomation.ReduceHP(300);
            _boxInfomation.Die();
            _boxAnimation.Attacked(300);
        }
    }

    private IEnumerator boxDestroyEffect()
    {
        MysteryBox.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(2f);
        yield break;
    }

    //駒へバフをかける
    public void BuffPiece(bool Side)
    {
        StartCoroutine(buffEffect(Side));
    }

    private IEnumerator buffEffect(bool Side)
    {
        BoxMesh.SetActive(false);
        DestroyEffect.SetActive(true);
        MysteryBox.GetComponentInChildren<ParticleSystem>().Stop();
        yield return new WaitForSeconds(0.5f);
        if (Side)
        {
            _audio.PlayOneShot(clip[1]);
            DestroyParticle[0].SetActive(true);
        }
        else
            DestroyParticle[1].SetActive(true);
        yield return new WaitForSeconds(1f);
        int ran;
        for (int i = 0; i < _allPieceInfos.Length; i++)
        {
            if (_allPieceInfos[i].Side != Side) continue;
            if (_allPieceInfos[i].isDie) continue;
            ran = Random.Range(0, 3);
            _allPieceInfos[i].ChangeValue(ran + 2);
            if (ran == 0)
                _effectManager.Active(_allPieceInfos[i].CurrentPosition, 1);
            else if (ran == 1)
                _effectManager.Active(_allPieceInfos[i].CurrentPosition, 3);
            else
                _effectManager.Active(_allPieceInfos[i].CurrentPosition, 2);
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        MysteryBox.SetActive(false);
        yield break;
    }
}
