using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    /*
    [SerializeField] private WheelCollider forwardLeft;
    [SerializeField] private WheelCollider forwardRight;
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider backRight;


    [SerializeField] private float strength = 20000;
    [SerializeField] private float maxTurn = 20;
    
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            var torque = strength * Time.deltaTime;
            forwardLeft.motorTorque = torque;
            forwardRight.motorTorque = torque;
            backLeft.motorTorque = torque;
            backRight.motorTorque = torque;
        }
        if (Input.GetKey(KeyCode.S))
        {
            var torque = - strength * Time.deltaTime;
            forwardLeft.motorTorque = torque;
            forwardRight.motorTorque = torque;
            backLeft.motorTorque = torque;
            backRight.motorTorque = torque;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            forwardLeft.steerAngle = maxTurn;
            forwardRight.steerAngle = maxTurn;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            forwardLeft.steerAngle = -maxTurn;
            forwardRight.steerAngle = -maxTurn;
        }
    }
    */

    private bool stop;
    public float stopForce;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        var locVel = transform.InverseTransformDirection(rigidbody.velocity);
        Debug.Log(locVel);
        stop = Input.GetAxis("Vertical") < 0 && (locVel.z > 0);
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;

        if (stop)
        {
            //            rigidbody.AddForce(transform.forward * stopForce);
        }
        
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce;
        frontPassengerW.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public WheelCollider frontDriverW, frontPassengerW;
    public WheelCollider rearDriverW, rearPassengerW;
    public Transform frontDriverT, frontPassengerT;
    public Transform rearDriverT, rearPassengerT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;

    private Rigidbody rigidbody;

}
