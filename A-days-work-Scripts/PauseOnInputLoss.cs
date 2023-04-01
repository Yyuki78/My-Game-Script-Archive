using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace OculusSampleFramework
{
    public class PauseOnInputLoss : MonoBehaviour
    {
        [SerializeField] GameObject[] Hands;
        void Start()
        {
            
        }
        private void OnInputFocusLost()
        {
            Time.timeScale = 0.0f;
            Hands[0].SetActive(false);
            Hands[1].SetActive(false);
        }
        private void OnInputFocusAcquired()
        {
            Time.timeScale = 1.0f;
            Hands[0].SetActive(true);
            Hands[1].SetActive(true);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            Pause(!hasFocus);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            Pause(pauseStatus);
        }

        private void Pause(bool pauseStatus)
        {
            if (pauseStatus == true)
            {
                Time.timeScale = 0;
                Hands[0].SetActive(false);
                Hands[1].SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                Hands[0].SetActive(true);
                Hands[1].SetActive(true);
            }
        }
    }
}