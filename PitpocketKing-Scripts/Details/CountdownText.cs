using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownText : MonoBehaviour
{
    private GameObject gameManager;
    private GameManager _manager;

    private GameObject Player;
    private PlayerMove _player;

    private TextMeshProUGUI timeLabel;

    private AudioManager _audio;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        _manager = gameManager.GetComponent<GameManager>();
        Player = GameObject.FindGameObjectWithTag("Player");
        _player = Player.GetComponent<PlayerMove>();
        timeLabel = GetComponent<TextMeshProUGUI>();
        timeLabel.text = " ";
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();

        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        _manager.waitStart = true;
        _player.EnableMove = false;
        timeLabel.text = "3";
        yield return new WaitForSeconds(0.4f);
        _audio.SE13();
        yield return new WaitForSeconds(0.9f);
        timeLabel.text = "2";
        yield return new WaitForSeconds(1.0f);
        timeLabel.text = "1";
        yield return new WaitForSeconds(1.0f);
        timeLabel.text = "スタート!";
        _manager.waitStart = false;
        _player.EnableMove = true;
        _audio.BGM2();
        yield return new WaitForSeconds(1.0f);
        timeLabel.text = " ";
        yield break;
    }
}