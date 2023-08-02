using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackInfomation : MonoBehaviour
{
    public PieceInfomation AttackPiece;
    public PieceInfomation AttackedPiece;

    private BattleSceneManagement _manager;
    private BattlePracticeManager _practice;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameParent") != null)
            _manager = GameObject.FindGameObjectWithTag("GameParent").GetComponent<BattleSceneManagement>();
        if (_manager != null)
        {
            if (_manager.isPractice)
            {
                _practice = _manager.gameObject.GetComponent<BattlePracticeManager>();
                AttackPiece.Role = _practice.AttackInfoRole;
                AttackedPiece.Role = _practice.AttackedInfoRole;
            }
            else
            {
                AttackPiece = _manager.AttackInfo;
                AttackedPiece = _manager.AttackedInfo;
            }
        }
    }

    public void ReleaseReference()
    {
        _manager = null;
        AttackPiece = null;
        AttackedPiece = null;
    }
}
