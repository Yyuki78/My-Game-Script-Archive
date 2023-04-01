using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTextController : MonoBehaviour
{
    private GetItemText[] _getItemText;

    // Start is called before the first frame update
    void Start()
    {
        _getItemText = GetComponentsInChildren<GetItemText>();
    }

    public void GetItem(int type)
    {
        if (!_getItemText[0].isUsed)
            _getItemText[0].GetItem(type);
        else
        {
            if (!_getItemText[1].isUsed)
                _getItemText[1].GetItem(type);
            else
                _getItemText[2].GetItem(type);
        }
    }
}
