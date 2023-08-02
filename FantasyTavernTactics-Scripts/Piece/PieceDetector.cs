using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceDetector : MonoBehaviour
{
    private PieceInfomation _info;
    private OVRGrabbableExtended grabbable;

    private TurnManager _turnManager;
    private TileManager _tileManager;

    void Awake()
    {
        _info = GetComponent<PieceInfomation>();
        grabbable = GetComponent<OVRGrabbableExtended>();

        _turnManager = GetComponentInParent<TurnManager>();
        _tileManager = GetComponentInParent<TileManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Piece") return;
        if (_turnManager._currentState != TurnManager.GameState.Attack) return;
        if (!_info.Side) return;
        if (!_info.CanAttack) return;
        if (!grabbable.isGrabbed) return;
        if (_info.isDie) return;
        PieceInfomation info = other.GetComponent<PieceInfomation>();
        if (info.Side) return;
        _tileManager.AttackPiece(_info, info);
        grabbable.grabbedBy.ForceRelease(grabbable);
    }
}
