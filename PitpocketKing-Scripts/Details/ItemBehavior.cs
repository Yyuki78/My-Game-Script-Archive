using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehavior : MonoBehaviour
{
    [SerializeField] int ItemMode = 0;//0で獲得金額アップ,1で人数増加,2で超絶お金持ち出現

    private float elapsedTime;

    private float RotateSpped;
    private Rigidbody _rigid;

    private GameObject gameManager;
    private GameManager _manager;

    // Start is called before the first frame update
    void Start()
    {
        RotateSpped = Random.Range(1.5f, 2.5f);
        _rigid = GetComponent<Rigidbody>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _manager = gameManager.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.isEnd) return;
        transform.Rotate(0, RotateSpped, 0);

        elapsedTime += Time.deltaTime;
        if (elapsedTime > 20f)
        {
            _manager.Repop(gameObject.transform.position);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            _rigid.isKinematic = true;
        }
        if(other.gameObject.layer == 12)
        {
            switch (ItemMode)
            {
                case 0:
                    other.gameObject.GetComponent<PlayerMove>().isUpGetMoney = true;
                    other.gameObject.GetComponent<PlayerMove>().already = true;
                    break;
                default:
                    break;
            }
            _manager.getItem(ItemMode);

            Destroy(gameObject);
        }
    }
}
