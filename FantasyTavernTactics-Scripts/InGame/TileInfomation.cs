using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfomation : MonoBehaviour
{
    [SerializeField] Vector2 MyPosition;
    public Vector2 myPosition => MyPosition;
    public bool ExistPiece;
    public bool ExistBuff;
    public bool isBlinking { get; private set; } = false;
    public bool isColorChange { get; private set; } = false;

    private TileManager _manager;
    private MeshRenderer _renderer;
    [SerializeField] Material[] TileMaterial = new Material[2];

    private float alpha_Sin;

    void Start()
    {
        _manager = GetComponentInParent<TileManager>();
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = TileMaterial[0];
        _renderer.enabled = false;
    }

    public void Blinking()
    {
        if (ExistPiece) return;
        if (isBlinking) return;
        isBlinking = true;
        _renderer.enabled = true;
    }

    public void StopBlinking()
    {
        if (!isBlinking) return;
        isBlinking = false;
        _renderer.enabled = false;
    }

    public void ColorChangeRed()
    {
        if (isBlinking) return;
        if (isColorChange) return;
        isBlinking = true;
        isColorChange = true;
        _renderer.enabled = true;
        _renderer.material = TileMaterial[1];
    }

    public void ColorChangeBefore()
    {
        if (!isBlinking) return;
        if (!isColorChange) return;
        isBlinking = false;
        isColorChange = false;
        _renderer.material = TileMaterial[0];
        _renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Piece") return;
        if (!isBlinking) return;
        if (isColorChange) return;
        PieceInfomation info = other.GetComponent<PieceInfomation>();
        _manager.MovePiece(info, this);
    }
}
