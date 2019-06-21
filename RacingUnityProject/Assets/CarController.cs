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

    public bool stop;
    public float stopForce;
    private Vector3 locVelocity;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        locVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        stop = Input.GetAxis("Vertical") < 0 && (locVelocity.z > 0);
        if (!stop)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                stop = true;
            }
        }
        
        blTrail.emitting = false;
        brTrail.emitting = false;
        flTrail.emitting = false;
        frTrail.emitting = false;
        
        if (stop)
        {
            rearDriverW.brakeTorque = stopForce;
            rearPassengerW.brakeTorque = stopForce;
            //            rigidbody.AddForce(transform.forward * stopForce);
            blTrail.emitting = true;
            brTrail.emitting = true;
        }
        else
        {
            rearDriverW.brakeTorque = 0;
            rearPassengerW.brakeTorque = 0;
        }

        if (rigidbody.velocity.magnitude > 1 && Vector3.Angle(transform.forward, rigidbody.velocity) > 20)
        {
            blTrail.emitting = true;
            brTrail.emitting = true;
            flTrail.emitting = true;
            frTrail.emitting = true;
        }

    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
        frontDriverW.steerAngle = m_steeringAngle;
        frontPassengerW.steerAngle = m_steeringAngle;

        
    }

    private void Accelerate()
    {
        frontDriverW.motorTorque = m_verticalInput * motorForce * (stop ? 2 : 1);
        frontPassengerW.motorTorque = m_verticalInput * motorForce * (stop ? 2 : 1);
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassengerW, frontPassengerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearPassengerW, rearPassengerT);
    }

    private void UpdateWheelPose(WheelCollider collider, Transform transform)
    {
        Vector3 pos;
        Quaternion quat;

        collider.GetWorldPose(out pos, out quat);

        
        transform.position = pos;
        transform.rotation = quat;
//        transform.localRotation.
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

    [SerializeField] private TrailRenderer flTrail;
    [SerializeField] private TrailRenderer frTrail;
    [SerializeField] private TrailRenderer blTrail;
    [SerializeField] private TrailRenderer brTrail;
    
    private Rigidbody rigidbody;

}
