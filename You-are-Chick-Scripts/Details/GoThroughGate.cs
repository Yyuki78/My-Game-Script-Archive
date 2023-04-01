using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoThroughGate : MonoBehaviour
{
    [SerializeField] GameObject Rock;
    [SerializeField] SpriteRenderer[] Gate;
    private bool once = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && once)
        {
            once = false;
            AudioManager.instance.SE(1);
            Rock.SetActive(false);
            Gate[0].color = new Color(1, 1, 1, 0.5f);
            Gate[1].color = new Color(1, 1, 1, 0.5f);
        }
    }
}
