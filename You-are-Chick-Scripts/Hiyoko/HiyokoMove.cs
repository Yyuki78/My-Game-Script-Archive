using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiyokoMove : MonoBehaviour
{
    private HiyokoInfomation _info;
    private Animator _anime;
    private SpriteRenderer _spriteRenderer;

    private float speed = 0.05f;
    private float firstSpeed;

    void Start()
    {
        _info = GetComponent<HiyokoInfomation>();
        _anime = GetComponent<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        firstSpeed = speed;
    }
    
    void Update()
    {
        if (!_info.IsMove) return;
        Vector2 position = transform.position;

        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        if (moveX > 0)
            _spriteRenderer.flipX = true;
        if (moveX < 0)
            _spriteRenderer.flipX = false;

        if (_info.IsJumping)
        {
            speed = firstSpeed / 1.5f;
            moveY = 0f;
        }else
            speed = firstSpeed;

        position += new Vector2(moveX, moveY).normalized * speed;

        if ((Vector2)transform.position != position)
        {
            if (!_anime.GetBool("isMoving"))
                _anime.SetBool("isMoving", true);
        }
        else
        {
            _anime.SetBool("isMoving", false);
        }

        transform.position = position;
    }
}
