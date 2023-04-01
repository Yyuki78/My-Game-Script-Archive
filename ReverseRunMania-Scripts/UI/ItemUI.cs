using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] Image firstItemImage;
    [SerializeField] Image secondItemImage;

    [SerializeField] Sprite[] ItemImage;

    private GameObject Player;
    private ItemManager _item;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        _item = Player.GetComponent<ItemManager>();
        firstItemImage.sprite = ItemImage[0];
        secondItemImage.sprite = ItemImage[0];
        StartCoroutine(checkLoop());
    }

    private IEnumerator checkLoop()
    {
        yield return new WaitForSeconds(10f);
        var wait = new WaitForSeconds(1f);
        while (true)
        {
            if (_item.FirstItem != 0 && firstItemImage.sprite == ItemImage[0])
            {
                firstItemImage.sprite = ItemImage[_item.FirstItem];
            }
            yield return wait;
        }
    }
    
    public void changeUI1(int itemNum)
    {
        firstItemImage.sprite = ItemImage[itemNum];
        StartCoroutine(DecisionItemEffect(true));
    }

    public void changeUI2(int itemNum)
    {
        secondItemImage.sprite = ItemImage[itemNum];
        StartCoroutine(DecisionItemEffect(false));
    }

    private IEnumerator DecisionItemEffect(bool first)
    {
        var size = new Vector2(1, 1);
        for(int i = 0; i < 15; i++)
        {
            if (first)
            {
                firstItemImage.rectTransform.sizeDelta += size;
            }
            else
            {
                secondItemImage.rectTransform.sizeDelta += size;
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (first)
            {
                firstItemImage.rectTransform.sizeDelta -= size * 3;
            }
            else
            {
                secondItemImage.rectTransform.sizeDelta -= size * 3;
            }
        }
        yield break;
    }
}
