using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BallMove : MonoBehaviour
{
    private Rigidbody2D _rigid;

    public Sprite[] sprites;
    [SerializeField] int spritePerFrame = 6;

    private int index = 0;
    private SpriteRenderer _renderer;
    private int frame = 0;

    void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        index = Random.Range(0, sprites.Length);
        _renderer.sprite = sprites[index];
    }

    void Update()
    {
        if (_rigid.velocity.magnitude <= 0.1f) return;
        if (index == sprites.Length) return;
        frame++;
        if (frame < spritePerFrame) return;
        spritePerFrame = 50 / Mathf.Max(1, (int)_rigid.velocity.magnitude);
        _renderer.sprite = sprites[index];
        frame = 0;
        index++;
        if (index >= sprites.Length)
        {
            index = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.instance.SE(15);
            float boundsPower = 0.1f;

            Vector3 hitPos = collision.contacts[0].point;

            Vector3 boundVec = this.transform.position - hitPos;

            Vector3 forceDir = boundsPower * boundVec.normalized;
            _rigid.AddForce(forceDir, ForceMode2D.Impulse);
        }
        if (collision.gameObject.tag == "SoccerGoal")
        {
            AudioManager.instance.SE(5);
            _rigid.velocity = Vector3.zero;
            _rigid.angularVelocity = 0f;
        }
    }
}