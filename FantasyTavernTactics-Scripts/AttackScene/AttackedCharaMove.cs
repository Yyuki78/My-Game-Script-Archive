using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedCharaMove : MonoBehaviour
{
    [SerializeField] int CharaType;
    [SerializeField] int AttackedCharaRole;
    private bool isFirstSword = true;
    private bool isFinishGame = false;
    private List<int> activeSphereNum = new List<int>();//突撃兵のみ使用

    private HitSphere[] _allSphere = new HitSphere[22];
    private HitSphere[] _swordSphere = new HitSphere[13];
    private HitSphere[] _bulletSphere = new HitSphere[9];
    private HitSphere _shieldBashCylinder;
    private IAttack _attack;

    [SerializeField] ParticleSystem ActiveGunSphereEffect;
    private int gunSphereNumber = 0;
    private int gunSphereNumber2 = 0;

    void Awake()
    {
        _allSphere = GetComponentsInChildren<HitSphere>();
    }

    private void Start()
    {
        int Snum = 0, Bnum = 0;
        for (int i = 0; i < _allSphere.Length; i++)
        {
            if (_allSphere[i].Color)
            {
                _swordSphere[Snum] = _allSphere[i];
                Snum++;
            }
            else
            {
                if (_allSphere[i].Number == 100)
                {
                    _shieldBashCylinder = _allSphere[i];
                }
                else
                {
                    _bulletSphere[Bnum] = _allSphere[i];
                    Bnum++;
                }
            }
        }
    }

    public void SetSphere(int type = 0, int attackedCharaRole = 0)
    {
        CharaType = type;
        AttackedCharaRole = attackedCharaRole;
        int ran = Random.Range(1, 8), num = 0;
        if (type == 5)//唯一剣の球(赤球)を使用しない
        {
            ran = Random.Range(0, 9);
            _bulletSphere[ran].Init();
            gunSphereNumber = ran;
            _attack = GetComponentInParent<IAttack>();
            return;
        }
        while (ran != _swordSphere[num].Number)
            num++;
        _swordSphere[num].Init();
        activeSphereNum.Add(ran);
        gunSphereNumber2 = ran;//type=4の時のみ使用
        if (type != 4)
        {
            if (ran == 7)
                ran = Random.Range(8, 14);
            else
                ran = Random.Range(7, 14);
            num = 0;
            while (ran != _swordSphere[num].Number)
                num++;
            _swordSphere[num].Init();
            activeSphereNum.Add(ran);
        }
        if (type == 1)
        {
            while (ran == activeSphereNum[0] || ran == activeSphereNum[1])
                ran = Random.Range(1, 14);
            num = 0;
            while (ran != _swordSphere[num].Number)
                num++;
            _swordSphere[num].Init();
            activeSphereNum.Add(ran);
        }
        else if (type == 2 || type == 3 || type == 4)
        {
            ran = Random.Range(0, 9);
            _bulletSphere[ran].Init();
            gunSphereNumber = ran;
        }
        _attack = GetComponentInParent<IAttack>();
    }

    public void hitSphere(int number, int type, Vector3 pos, OVRInput.Controller controller, float speed = 0f)
    {
        if (type == 0)
            _attack.Hit(0, pos, speed, controller);
        else if(type == 1)
            _attack.Hit(3, pos, speed, controller);
        int ran = 0, num = 0;
        if (type == 0)
        {
            if (isFirstSword)
            {
                isFirstSword = false;
                activeSphereNum.Remove(number);
                return;
            }
            if (number < 7)
            {
                do
                    ran = Random.Range(7, 14);
                while (activeSphereNum.Contains(ran));
            }
            else if (number == 7)
            {
                do
                    ran = Random.Range(1, 14);
                while (activeSphereNum.Contains(ran) || ran == 7);
            }
            else
            {
                do
                    ran = Random.Range(1, 8);
                while (activeSphereNum.Contains(ran));
            }
            num = 0;
            while (ran != _swordSphere[num].Number)
                num++;
            StartCoroutine(Active(num, true));
            activeSphereNum.Remove(number);
            activeSphereNum.Add(ran);
        }
        else if (type == 1)
        {
            _bulletSphere[gunSphereNumber].Hit();
            do
                ran = Random.Range(0, 9);
            while (ran == gunSphereNumber);
            gunSphereNumber = ran;
            StartCoroutine(Active(ran, false));
            if (CharaType == 3 || CharaType == 5)
            {
                ActiveGunSphereEffect.transform.position = _bulletSphere[ran].transform.position;
                ActiveGunSphereEffect.Play();
            }
        }
        else if (type == 2)
        {
            num = 0;
            while (gunSphereNumber2 != _swordSphere[num].Number)
                num++;
            _swordSphere[num].Hit();

            if (number < 7)
            {
                do
                    ran = Random.Range(7, 14);
                while (ran == gunSphereNumber2);
            }
            else if (number == 7)
            {
                do
                    ran = Random.Range(1, 14);
                while (ran == gunSphereNumber2);
            }
            else
            {
                do
                    ran = Random.Range(1, 8);
                while (ran == gunSphereNumber2);
            }
            gunSphereNumber2 = ran;
            num = 0;
            while (ran != _swordSphere[num].Number)
                num++;
            StartCoroutine(Active(num, true));
        }
    }

    private IEnumerator Active(int num, bool type)
    {
        yield return null;
        if(type)
            _swordSphere[num].Init();
        else
            _bulletSphere[num].Init();
        yield break;
    }

    public void ShieldBashCylinder(int type, float speed = 0f, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        switch (type)
        {
            case 0://Active
                _shieldBashCylinder.Init();
                break;
            case 1://InActive
                _shieldBashCylinder.Hit();
                break;
            case 2://Hit
                _attack.Hit(1, Vector3.zero, speed, controller);
                break;
        }
    }

    public void Finish()
    {
        isFinishGame = true;
        for (int i = 0; i < _allSphere.Length; i++)
        {
            if (_allSphere[i].gameObject.activeSelf)
                _allSphere[i].gameObject.SetActive(false);
        }
    }

    public void HitAttack(int type = 0, OVRInput.Controller controller = OVRInput.Controller.Active)
    {
        if (isFinishGame) return;
        switch (type)
        {
            case 0://普通に攻撃が当たった
                if (_attack == null)
                    _attack = GetComponentInParent<IAttack>();
                if (AttackedCharaRole != 3)
                    _attack.HitEnemyAttack();
                else
                    _attack.HitEnemyAttack(4);
                break;
            case 1://攻撃を弾いた
                _attack.HitEnemyAttack(1);
                break;
            case 2://自分の盾とプレイヤーの剣がぶつかった
                _attack.HitEnemyAttack(3);
                break;
            case 3://自分の剣とプレイヤーの盾がぶつかった
                _attack.HitEnemyAttack(2);
                break;
        }
    }
}
