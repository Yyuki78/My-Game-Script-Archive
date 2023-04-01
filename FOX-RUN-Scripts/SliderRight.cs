using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderRight : MonoBehaviour
{
    //public
    public float speed = 1.0f;
    //private
    private Image image;
    private float time;
    [SerializeField] private GameObject Fox;
    private EnemyMove _move;
    // Start is called before the first frame update
    void Start()
    {
        image = this.gameObject.GetComponent<Image>();
        _move = Fox.GetComponent<EnemyMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_move.slider.value >= 45 && _move.slider.value < 50)
        {
            Debug.Log("ゲージが9割を超えました");
            //オブジェクトのAlpha値を更新
            image.color = GetAlphaColor(image.color);
        }
        else
        {
            image.color = SetAlphaColor(image.color);
;        }
    }
    
    //Alpha値を更新してColorを返す
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 1f;

        return color;
    }
    Color SetAlphaColor(Color color)
    {
        color.a = 1.5f;

        return color;
    }
}
