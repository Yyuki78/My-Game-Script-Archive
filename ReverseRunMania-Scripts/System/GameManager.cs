using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        GameStart,
        GamePlay,
        Result,
    }
    public static GameState currentState;//現在のモード

    public static bool isGameOver = false;

    [SerializeField] GameObject Tutrial;
    [SerializeField] GameObject ResultPanel;
    [SerializeField] GameObject MileageText;
    [SerializeField] GameObject Player;
    private PlayerMove _move;

    void Awake()
    {
        //Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        Application.targetFrameRate = 60;

        isGameOver = false;
        SetCurrentState(GameState.GameStart);

        _move = Player.GetComponent<PlayerMove>();
        _move.isMoving = false;

        StartCoroutine(WaitStart());
    }


    void FixedUpdate()
    {
        if (currentState == GameState.GameStart)
        {
            _move.StartAutoMove();
        }
    }

    public void SetCurrentState(GameState state)
    {
        currentState = state;
        OnGameStateChanged(currentState);
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    // 状態が変わったら何をするか
    void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.GameStart:
                
                break;
            case GameState.GamePlay:
                _move.isMoving = true;
                break;
            case GameState.Result:
                ResultPanel.SetActive(true);
                break;
            default:
                break;
        }
    }

    private IEnumerator WaitStart()
    {
        yield return new WaitForSeconds(4f);
        Tutrial.SetActive(true);
        MileageText.SetActive(true);
        SetCurrentState(GameState.GamePlay);
        AudioManager.instance.BGM(1);
        yield break;
    }
}
