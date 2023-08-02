using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrowEffect : MonoBehaviour
{
    [SerializeField] Vector3 movement;

    private Hashtable moveUpHash;
    private Hashtable moveDownHash;

    void Start()
    {
        moveUpHash = iTween.Hash(
            "position", gameObject.transform.position + movement,
            "time", 1.0f,
            "delay", 0f,
            "easetype", iTween.EaseType.easeInOutQuad,
            "oncomplete", "MovingDown",
            "oncompletetarget", this.gameObject
        );
        moveDownHash = iTween.Hash(
            "position", gameObject.transform.position - movement,
            "time", 1.0f,
            "delay", 0f,
            "easetype", iTween.EaseType.easeInOutQuad,
            "oncomplete", "MovingUp",
            "oncompletetarget", this.gameObject
        );
        MovingUp();
    }

    private void MovingUp()
    {
        iTween.MoveTo(gameObject, moveUpHash);
    }

    private void MovingDown()
    {
        iTween.MoveTo(gameObject, moveDownHash);
    }
}
