using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineVirtualCamera _camera;
    CinemachineTransposer _body;
    CinemachineComposer _aim;
    CinemachineOrbitalTransposer _rotate;

    private AudioManager _audio;

    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _body = _camera.GetCinemachineComponent<CinemachineTransposer>();
        _aim = _camera.GetCinemachineComponent<CinemachineComposer>();
        _rotate = _camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _audio = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        StartCoroutine(StartCamera());
    }

    private IEnumerator StartCamera()
    {
        _body.m_XDamping = 0;
        _body.m_YDamping = 0;
        _body.m_ZDamping = 0;
        _body.m_YawDamping = 0;
        _aim.m_HorizontalDamping = 0;
        _aim.m_VerticalDamping = 0;
        _body.m_FollowOffset.x = -1f;
        _body.m_FollowOffset.y = 1.25f;
        _body.m_FollowOffset.z = 3f;
        yield return new WaitForSeconds(1.3f);
        _body.m_FollowOffset.x = 1f;
        yield return new WaitForSeconds(1f);
        _body.m_FollowOffset.x = 0;
        _body.m_FollowOffset.z = -3f;
        yield return new WaitForSeconds(1f);
        _body.m_XDamping = 1;
        _body.m_YDamping = 1;
        _body.m_ZDamping = 1;
        _body.m_YawDamping = 0.5f;
        _aim.m_HorizontalDamping = 0.5f;
        _aim.m_VerticalDamping = 0.5f;
        yield break;
    }

    public IEnumerator GameOver()
    {
        _audio.SE15();
        int finishNum = 0;
        yield return new WaitForSecondsRealtime(0.1f);
        while (finishNum != 2)
        {
            if (_rotate.m_Heading.m_Bias > -180)
            {
                _rotate.m_Heading.m_Bias -= 6;
                yield return new WaitForSecondsRealtime(0.03f);
            }
            else
            {
                _rotate.m_Heading.m_Bias = 180;
                yield return new WaitForSecondsRealtime(0.03f);
                finishNum++;
            }
        }
        var g = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        g.GameOver(true);
    }
}
