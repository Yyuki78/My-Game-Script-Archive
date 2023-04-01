using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private WheelCollider[] m_WheelColliders = new WheelCollider[4];
    [SerializeField] private GameObject[] m_WheelMeshes = new GameObject[4];
    [SerializeField] private Vector3 m_CentreOfMassOffset;
    [SerializeField] private float m_MaximumSteerAngle;
    [Range(0, 1)] [SerializeField] private float m_TractionControl; // 0 is no traction control, 1 is full interference
    [SerializeField] private float m_FullTorqueOverAllWheels;
    [SerializeField] private float m_ReverseTorque;
    [SerializeField] private float m_BrakeTorque;

    private Quaternion[] m_WheelMeshLocalRotations;
    private float m_SteerAngle;
    [SerializeField] float m_CurrentTorque;
    private Rigidbody m_Rigidbody;

    public float m_Topspeed2 = 23;
    public bool holdSpeed = false;//CarParticleSystemでも使用

    public float BrakeInput { get; private set; }
    public float CurrentSteerAngle { get { return m_SteerAngle; } }
    public float CurrentSpeed { get { return m_CurrentTorque; } }
    public float MaxSpeed { get { return m_FullTorqueOverAllWheels; } }
    public float AccelInput { get; private set; }
    public float CurrentMotorTorque { get { return m_WheelColliders[0].motorTorque; } }

    CarState _state;

    public float BoostQuantity = 30;

    public bool isMoving = true;
    private bool isFreeze = false;
    private Coroutine coroutine = null;
    [SerializeField] GameObject[] iceParticle;

    [SerializeField] GameObject Tutrial;

    // Use this for initialization
    private void Start()
    {
        m_WheelMeshLocalRotations = new Quaternion[4];
        for (int i = 0; i < 4; i++)
        {
            m_WheelMeshLocalRotations[i] = m_WheelMeshes[i].transform.localRotation;
        }
        m_WheelColliders[0].attachedRigidbody.centerOfMass = m_CentreOfMassOffset;

        m_Rigidbody = GetComponent<Rigidbody>();
        m_CurrentTorque = m_FullTorqueOverAllWheels - (m_TractionControl * m_FullTorqueOverAllWheels);

        _state = GetComponent<CarState>();
    }

    private void FixedUpdate()
    {
        if (!isMoving) return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float handbrake = 0;
        if (_state.IsDrive)
        {
            Move(h, v, v, handbrake);
        }
    }

    public void StartAutoMove()
    {
        //タイヤのメッシュを動きに追従させる
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            m_WheelColliders[i].GetWorldPose(out position, out quat);
            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = quat;
            m_WheelMeshes[i].transform.rotation = m_WheelMeshes[i].transform.rotation * new Quaternion(90, 90, 0, 0);
        }

        m_WheelColliders[0].motorTorque = m_CurrentTorque / 4f;
        m_WheelColliders[1].motorTorque = m_CurrentTorque / 4f;
        m_WheelColliders[3].motorTorque = m_CurrentTorque / 4f;
        m_WheelColliders[2].motorTorque = m_CurrentTorque / 4f;
    }

    private void Move(float steering, float accel, float footbrake, float handbrake)
    {
        //タイヤのメッシュを動きに追従させる
        for (int i = 0; i < 4; i++)
        {
            Quaternion quat;
            Vector3 position;
            m_WheelColliders[i].GetWorldPose(out position, out quat);
            m_WheelMeshes[i].transform.position = position;
            m_WheelMeshes[i].transform.rotation = quat;
            m_WheelMeshes[i].transform.rotation = m_WheelMeshes[i].transform.rotation * new Quaternion(90, 90, 0, 0);
        }

        //左右入力がされた最初の一回でチュートリアルを消す
        if (Tutrial.activeSelf && steering != 0f)
            Tutrial.SetActive(false);

        //入力量を-1,1の範囲に収める
        steering = Mathf.Clamp(steering, -1, 1);
        AccelInput = accel = Mathf.Clamp(accel, 0, 1);
        BrakeInput = footbrake = -1 * Mathf.Clamp(footbrake, -1, 0);
        handbrake = Mathf.Clamp(handbrake, 0, 1);

        //自動で前に進む
        AutoMove();

        //進ませる
        ApplyDrive(accel, footbrake);

        //現在のスピードに合わせて曲がる強さを変える
        if (m_MaximumSteerAngle > 9.5f)
            m_MaximumSteerAngle = 16.5f - m_CurrentTorque / (5000f / 3.5f);
        if (isFreeze)
        {
            m_MaximumSteerAngle += 3.0f;
            AchievementDetection.freezingTime += Time.deltaTime;
        }
        else
            AchievementDetection.freezingTime = 0f;

        //曲げる
        Steering(steering);

        //ブレーキ
        //ApplyBrake(BrakeInput);

        //最大速度に到達するとその速度で走るようになる
        //HoldTopSpeed();

        //スタックする問題の解消
        //ResolveStack();
    }

    private void ApplyDrive(float accel, float footbrake)
    {
        if (accel == 0) return;
        float thrustTorque;

        //前に進ませる
        if (m_CurrentTorque < 9500)
        {
            thrustTorque = 2000;
            for (int i = 0; i < 4; i++)
            {
                m_WheelColliders[i].motorTorque = (m_CurrentTorque + thrustTorque) / 4f;
            }
        }
        else
        {
            thrustTorque = 11500;
            for (int i = 0; i < 4; i++)
            {
                m_WheelColliders[i].motorTorque = thrustTorque / 4f;
            }
        }

        /*
        thrustTorque = accel * (m_CurrentTorque / 4f);
        for (int i = 0; i < 4; i++)
        {
            m_WheelColliders[i].motorTorque = thrustTorque;
        }
        
        for (int i = 0; i < 4; i++)
        {
            if (CurrentSpeed > 5 && Vector3.Angle(transform.forward, m_Rigidbody.velocity) < 50f)
            {
                m_WheelColliders[i].brakeTorque = m_BrakeTorque * footbrake;
            }
            else if (footbrake > 0)
            {
                m_WheelColliders[i].brakeTorque = 0f;
                m_WheelColliders[i].motorTorque = -m_ReverseTorque * footbrake;
            }
        }*/
    }

    private void ApplyBrake(float footbrake)
    {
        //Set the handbrake.
        //Assuming that wheels 2 and 3 are the rear wheels.
        if (footbrake > 0f)
        {
            var fbTorque = -footbrake * m_BrakeTorque / 4;
            for (int i = 0; i < 4; i++)
            {
                m_WheelColliders[i].motorTorque = fbTorque;
            }
        }
    }

    private void HoldTopSpeed()
    {
        if (m_Rigidbody.velocity.magnitude + 0.5f >= m_Topspeed2)
        {
            if (!_state.IsDrive) return;
            holdSpeed = true;
            Debug.Log("最高速度");
        }
        else
        {
            holdSpeed = false;
        }

        if (holdSpeed)
        {
            if (m_Rigidbody.velocity.magnitude >= m_Topspeed2) return;
            for (int i = 0; i < 4; i++)
            {
                m_WheelColliders[i].motorTorque = m_FullTorqueOverAllWheels / 2f;
            }
        }
    }

    private void Steering(float steering)
    {
        if (steering != 0f)
            AudioManager.instance.SE(2);
        else
            AudioManager.instance.StopSE(2);

        WheelFrictionCurve sFriction = m_WheelColliders[2].sidewaysFriction;
        JointSpring Spring = m_WheelColliders[2].suspensionSpring;

        //入力量に応じて左右に曲がる
        m_SteerAngle = m_MaximumSteerAngle;

        m_SteerAngle -= ((m_Rigidbody.velocity.magnitude) / 3f);
        m_SteerAngle *= steering;

        m_WheelColliders[0].steerAngle = m_SteerAngle;
        m_WheelColliders[1].steerAngle = m_SteerAngle;
        m_WheelColliders[2].steerAngle = 0;
        m_WheelColliders[3].steerAngle = 0;

        sFriction.extremumSlip = 1f;
        sFriction.extremumValue = 30f;

        Spring.spring = 11500;
        /*
        m_WheelColliders[2].sidewaysFriction = sFriction;
        m_WheelColliders[3].sidewaysFriction = sFriction;
        m_WheelColliders[2].suspensionSpring = Spring;
        m_WheelColliders[3].suspensionSpring = Spring;
        */
    }

    private void ResolveStack()
    {
        if (!_state.IsDrive) return;
        if (Mathf.Abs(m_Rigidbody.velocity.magnitude) <= 1)
        {
            m_Rigidbody.AddForce(transform.up * 1f);
        }
    }

    private void AutoMove()
    {
        if (m_CurrentTorque < m_FullTorqueOverAllWheels)
            m_CurrentTorque += Time.deltaTime * 7.5f;

        m_WheelColliders[0].motorTorque = m_CurrentTorque / 4f;
        m_WheelColliders[1].motorTorque = m_CurrentTorque / 4f;
        m_WheelColliders[3].motorTorque = m_CurrentTorque / 4f;
        m_WheelColliders[2].motorTorque = m_CurrentTorque / 4f;
    }

    public void Respown()
    {
        Move(0, 0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            m_WheelColliders[i].motorTorque = 0;
        }
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        BoostQuantity = 33f;
    }

    public void FreezeWheel()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine("freezeWheel");
    }

    private IEnumerator freezeWheel()
    {
        isFreeze = true;
        AudioManager.instance.SE(15);

        for (int i=0;i< iceParticle.Length; i++)
        {
            iceParticle[i].SetActive(true);
        }

        yield return new WaitForSeconds(10f);

        for (int i = 0; i < iceParticle.Length; i++)
        {
            iceParticle[i].SetActive(false);
        }

        isFreeze = false;
        AudioManager.instance.StopSE(15);
        yield break;
    }
}