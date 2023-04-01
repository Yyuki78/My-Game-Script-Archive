using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour
{
    [SerializeField] private Button Menubutton;
    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private Button Resumebutton;
    [SerializeField] private Button Resumebutton2;
    [SerializeField] private Button Helpbutton;
    [SerializeField] private GameObject HelpPanel;
    [SerializeField] private Button HelpResumebutton;
    private bool pauseGame = false;
    private void Start()
    {
        MenuPanel.SetActive(false);
        Menubutton.onClick.AddListener(Pause);
        Resumebutton.onClick.AddListener(Resume);
        Resumebutton2.onClick.AddListener(Resume);
        HelpPanel.SetActive(false);
        Helpbutton.onClick.AddListener(HelpPause);
        HelpResumebutton.onClick.AddListener(HelpResume);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame = !pauseGame;

            if (pauseGame == true)
            {
                Pause();
            }
            else
            {
                Resume();
                HelpResume();
            }
        }
    }

    private void Pause()
    {
        Time.timeScale = 0;
        MenuPanel.SetActive(true);
    }

    private void Resume()
    {
        Time.timeScale = 1;
        MenuPanel.SetActive(false);
    }

    private void HelpPause()
    {
        HelpPanel.SetActive(true);
    }

    private void HelpResume()
    {
        HelpPanel.SetActive(false);
    }
}
