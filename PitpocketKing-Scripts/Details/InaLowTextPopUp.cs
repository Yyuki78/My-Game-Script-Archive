using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InaLowTextPopUp : MonoBehaviour
{
    private TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(popUp());
        int ranX = Random.Range(200, 400);
        int ranY = Random.Range(400, 500);
        transform.localPosition = new Vector3(ranX, ranY, 0);
    }

    private IEnumerator popUp()
    {
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 100; i++)
        {
            transform.localPosition += new Vector3(0, 0.5f, 0);
            _text.color -= new Color(0, 0, 0, 0.01f);
            yield return new WaitForSeconds(0.02f);
        }

        Destroy(gameObject);
    }
}
