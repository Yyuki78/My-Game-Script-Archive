using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipWeapon : MonoBehaviour
{
    [SerializeField] GameObject Gun;
    [SerializeField] GameObject Sword;

    [SerializeField] Transform spine;
    [SerializeField] Transform rightHand;
    [SerializeField] Transform leftHand;

    private bool isSwordEquipped = false;

    private Vector3 gunOnPosition;
    private Vector3 gunOnRotation;
    private Vector3 swordOnPosition;
    private Vector3 swordOnRotation;

    protected Vector3 equipGunPosition1 = new Vector3(-0.003f, 0.065f, 0.008f);
    protected Vector3 equipGunRotation1 = new Vector3(-75, 270f, -16.5f);
    protected Vector3 equipGunPosition2 = new Vector3(-0.0028f, 0.0759f, 0.0066f);
    protected Vector3 equipGunRotation2 = new Vector3(-67.169f, 43.457f, 43.36f);
    protected Vector3 equipSwordPosition = new Vector3(0.0082f, 0.0647f, 0.0214f);
    protected Vector3 equipSwordRotation = new Vector3(-116.458f, 263.815f, 186.408f);

    void Start()
    {
        gunOnPosition = Gun.transform.localPosition;
        gunOnRotation = Gun.transform.localEulerAngles;
        swordOnPosition = Sword.transform.localPosition;
        swordOnRotation = Sword.transform.localEulerAngles;

        Gun.gameObject.transform.parent = spine;
        Sword.gameObject.transform.parent = spine;
    }
    
    public void ToggleWeapon(int num)
    {
        switch (num)
        {
            case 0://èeÇ≈çUåÇ
                Gun.gameObject.transform.parent = leftHand;
                
                Gun.gameObject.transform.localPosition = equipGunPosition1;
                Gun.gameObject.transform.localEulerAngles = equipGunRotation1;
                break;
            case 2://èeÇí≠ÇﬂÇÈ
                Gun.gameObject.transform.parent = rightHand;

                Gun.gameObject.transform.localPosition = equipGunPosition2;
                Gun.gameObject.transform.localEulerAngles = equipGunRotation2;
                break;
            case 1://åïÇ≈çUåÇ
                if (isSwordEquipped)
                {
                    isSwordEquipped = false;
                    Sword.gameObject.transform.parent = spine;

                    Sword.gameObject.transform.localPosition = swordOnPosition;
                    Sword.gameObject.transform.localEulerAngles = swordOnRotation;
                }
                else
                {
                    isSwordEquipped = true;
                    Sword.gameObject.transform.parent = rightHand;

                    Sword.gameObject.transform.localPosition = equipSwordPosition;
                    Sword.gameObject.transform.localEulerAngles = equipSwordRotation;
                }
                break;
        }
    }

    public void WeaponDetachment()
    {
        Gun.gameObject.transform.parent = spine;
        Sword.gameObject.transform.parent = spine;
        Gun.gameObject.transform.localPosition = gunOnPosition;
        Gun.gameObject.transform.localEulerAngles = gunOnRotation;
        Sword.gameObject.transform.localPosition = swordOnPosition;
        Sword.gameObject.transform.localEulerAngles = swordOnRotation;
    }
}
