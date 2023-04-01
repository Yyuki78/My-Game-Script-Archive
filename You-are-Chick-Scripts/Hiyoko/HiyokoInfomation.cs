using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiyokoInfomation : MonoBehaviour
{
    public enum State
    {
        Normal,
        Pick,
        Put,
        Jump,
        Bound,
        Start,
        Finish
    }
    public State _currentState { private set; get; }
    public bool IsMove { private set; get; }
    public bool IsPick { private set; get; }
    public bool IsJumping { private set; get; }
    public bool IsFinish { private set; get; }

    void Start()
    {
        SetState(State.Normal);
    }

    public void SetState(State state)
    {
        _currentState = state;
        switch (state)
        {
            case State.Normal:
                IsMove = true;
                IsPick = true;
                IsJumping = false;
                break;
            case State.Pick:
                IsMove = false;
                break;
            case State.Put:
                SetState(State.Normal);
                break;
            case State.Jump:
                IsPick = false;
                IsJumping = true;
                break;
            case State.Bound:
                break;
            case State.Start:
                break;
            case State.Finish:
                IsPick = false;
                IsFinish = true;
                break;
        }
    }
}
