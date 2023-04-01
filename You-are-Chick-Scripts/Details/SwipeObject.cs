using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeObject : MonoBehaviour
{
    [SerializeField] int mode = 0;
    [SerializeField] BroomEffect _broom;

    public void SwipThis()
    {
        _broom.effect(this.gameObject, mode);
    }

    private void OnMouseDown()
    {
        _broom.effect(this.gameObject, mode);
    }
}
