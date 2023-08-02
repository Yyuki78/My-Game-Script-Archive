using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInfomation : MonoBehaviour
{
    public bool Side;
    public bool isFighter = false;
    public int Role;//0‚Í•à•º,1‚Í‹Rm,2‚Í–‚–@•º,3‚Í“ËŒ‚•º,4‚ÍwŠöŠ¯,5‚ÍMysteriousBox
    public float Attack { get; private set; }
    public float Defense { get; private set; }
    public float MagicDefense { get; private set; }
    public int MaxHP { get; private set; } = 100;
    public int HP { get; private set; } = 100;
    public int Attribute { get; private set; }
    public Vector2 CurrentPosition;
    public bool alreadySetAttribute = false;
    public bool CanAttack { get; private set; } = true;
    public bool isCanAttacked = true;//UŒ‚‚ğó‚¯‚é‚±‚Æ‚ªo—ˆ‚é‚©
    public bool isAIMoving = false;//AIƒ^[ƒ“‚Ì‹îˆÚ“®’†‚©‚Ç‚¤‚©

    public bool isDie { get; private set; } = false;

    [SerializeField] Material[] PieceBaseMaterial;
    [SerializeField] GameObject MyPieceProof;
    private HitEffect _effect;

    private AudioSource _audio;
    [SerializeField] AudioClip[] clip = new AudioClip[6];

    void Start()
    {
        _effect = GetComponentInChildren<HitEffect>();
        _audio = GetComponent<AudioSource>();
        Attribute = -1;
        switch (Role)
        {
            case 0:
                Attack = 2;
                Defense = 2;
                MagicDefense = 2;
                break;
            case 1:
                Attack = 2;
                Defense = 3;
                MagicDefense = 2.5f;
                break;
            case 2:
                Attack = 2;
                Defense = 1.5f;
                MagicDefense = 2;
                break;
            case 3:
                Attack = 3;
                Defense = 2;
                MagicDefense = 1;
                break;
            case 4:
                Attack = 2.5f;
                Defense = 3;
                MagicDefense = 3;
                Attribute = 3;
                if (Side)
                    isFighter = true;
                break;
            case 5:
                Defense = 2;
                MagicDefense = 2;
                MaxHP = 200;
                HP = MaxHP;
                Attribute = 3;
                return;
        }
        if (!Side)
        {
            switch (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameInfomation>().isEnemyHP)
            {
                case 0:
                    MaxHP = 100;
                    break;
                case 1:
                    MaxHP = 50;
                    break;
                case 2:
                    MaxHP = 125;
                    break;
                case 3:
                    MaxHP = 150;
                    break;
            }
            HP = MaxHP;
            if (Role == 4) return;
            int ran = Random.Range(0, 3);
            SetAttribute(ran);
        }
    }

    public void SetAttribute(int AttributeNum)
    {
        if (Attribute == 3) return;
        Attribute = AttributeNum;
        Transform child = gameObject.transform.GetChild(0);
        Renderer renderer = child.GetComponent<Renderer>();
        renderer.material = PieceBaseMaterial[Attribute];
        alreadySetAttribute = true;
    }

    public void ResetAttribute()
    {
        if (Attribute == 3) return;
        Attribute = 0;
        Transform child = gameObject.transform.GetChild(0);
        Renderer renderer = child.GetComponent<Renderer>();
        renderer.material = PieceBaseMaterial[3];
        alreadySetAttribute = false;
    }

    public void SetMyPiece()
    {
        if (Role == 4) return;
        _audio.PlayOneShot(clip[0]);
        MyPieceProof.SetActive(true);
        isFighter = true;
    }

    public void ResetMyPiece()
    {
        if (Role == 4) return;
        MyPieceProof.SetActive(false);
        isFighter = false;
    }

    public void ChangeValue(int type)
    {
        _audio.volume = 0.25f;
        _audio.pitch = 1.0f;
        switch (type)
        {
            case 0://UŒ‚UP
                Attack += 1;
                _audio.PlayOneShot(clip[1]);
                break;
            case 1://HP‰ñ•œ
                HP += 50;
                if (HP > MaxHP)
                    HP = MaxHP;
                _effect.GaugeReduction(-50, 1.5f);
                _audio.PlayOneShot(clip[2]);
                break;
            case 2://UŒ‚UP2
                Attack += 1f;
                _audio.PlayOneShot(clip[1]);
                break;
            case 3://–hŒä—ÍUP
                Defense += 0.5f;
                MagicDefense += 0.5f;
                _audio.PlayOneShot(clip[3]);
                break;
            case 4://Å‘åHPUP
                MaxHP += 25;
                HP += 25;
                _effect.GaugeReduction(-25, 1.5f);
                _audio.PlayOneShot(clip[4]);
                break;
            case 5://ˆÚ“®‰ñ”UP(‰¹‚Ì‚İ)
                _audio.PlayOneShot(clip[5]);
                break;
        }
    }

    public void ChangeCanAttack(bool canAttack)
    {
        CanAttack = canAttack;
    }

    public void ReduceHP(int num)//”»’è‚Ìˆ×‚Éæ‚ÉHP‚ÍŒ¸‚ç‚·
    {
        HP -= num;
    }

    public void Attacked(int num)//‰‰o—p
    {
        _effect.GaugeReduction(num, 1.5f);
    }

    public void Die()
    {
        isDie = true;
    }

    private void OnEnable()
    {
        if (isDie) gameObject.SetActive(false);
    }
}
