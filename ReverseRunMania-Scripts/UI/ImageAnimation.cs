using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    public bool isGacha = false;

    public Sprite[] sprites;
    [SerializeField] int spritePerFrame = 6;

    private int index = 0;
    private Image image;
    private int frame = 0;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (!isGacha) return;
        if (index == sprites.Length) return;
        frame++;
        if (frame < spritePerFrame) return;
        image.sprite = sprites[index];
        frame = 0;
        index++;
        if (index >= sprites.Length)
        {
            index = 0;
        }
    }
}