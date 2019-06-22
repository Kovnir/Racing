using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

public class CarController : MonoBehaviour
{
    
    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    [SerializeField]
    private WheelCollider frontLeftWheelCollider;
    [SerializeField]
    private WheelCollider frontRightWheelCollider;
    [SerializeField]
    private WheelCollider backLeftWheelCollider;
    [SerializeField]
    private WheelCollider backRightWheelCollider;

    [SerializeField]
    private Transform frontLeftWheelTransform;
    [SerializeField]
    private Transform frontRightWheel;
    [SerializeField]
    private Transform backLeftWheelTransform;
    [SerializeField]
    private Transform backRightWheelTransform;
    
    public float maxSteerAngle = 30;
    public float motorForce = 50;

    [SerializeField] private TrailRenderer flTrail;
    [SerializeField] private TrailRenderer frTrail;
    [SerializeField] private TrailRenderer blTrail;
    [SerializeField] private TrailRenderer brTrail;
    
    private Rigidbody rigidbody;
    
    public bool stop;
    public float stopForce;
    private Vector3 locVelocity;

    [Inject] private DiContainer container;
    
    private void Awake()
    {
//        container.Bind<CarController>().FromInstance(this);
        rigidbody = GetComponent<Rigidbody>();
    }

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");
        locVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        stop = Input.GetAxis("Vertical") < 0 && (locVelocity.z > 1);
        if (!stop)
        {
            if (Input.GetKey(KeyCode.Space) && locVelocity.z > 1)
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
            backLeftWheelCollider.brakeTorque = stopForce;
            backRightWheelCollider.brakeTorque = stopForce;
            //            rigidbody.AddForce(transform.forward * stopForce);
            blTrail.emitting = true;
            brTrail.emitting = true;
        }
        else
        {
            backLeftWheelCollider.brakeTorque = 0;
            backRightWheelCollider.brakeTorque = 0;
        }

        if (rigidbody.velocity.magnitude > 1 && Vector3.Angle(transform.forward, rigidbody.velocity) > 20)
        {
            flTrail.emitting = true;
            frTrail.emitting = true;
            blTrail.emitting = true;
            brTrail.emitting = true;
        }
        
        //don't draw trails in air
        frTrail.emitting &= frontRightWheelCollider.isGrounded;
        flTrail.emitting &= frontLeftWheelCollider.isGrounded;

        blTrail.emitting &= backLeftWheelCollider.isGrounded;
        brTrail.emitting &= backRightWheelCollider.isGrounded;
    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput * (stop ? 2 : 1);
        frontLeftWheelCollider.steerAngle = m_steeringAngle;
        frontRightWheelCollider.steerAngle = m_steeringAngle;

        
    }

    private void Accelerate()
    {
        frontLeftWheelCollider.motorTorque = m_verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPose(frontRightWheelCollider, frontRightWheel);
        UpdateWheelPose(backLeftWheelCollider, backLeftWheelTransform);
        UpdateWheelPose(backRightWheelCollider, backRightWheelTransform);
    }

    private void UpdateFriction()
    {
        UpdateWheelFriction(frontLeftWheelCollider);
        UpdateWheelFriction(frontRightWheelCollider);
        UpdateWheelFriction(backLeftWheelCollider);
        UpdateWheelFriction(backRightWheelCollider);
    }

    private void UpdateWheelFriction(WheelCollider wheel)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            WheelFrictionCurve fFriction = wheel.forwardFriction;
            fFriction.stiffness = hit.collider.material.staticFriction;
            wheel.forwardFriction = fFriction;
            WheelFrictionCurve sFriction = wheel.sidewaysFriction;
            sFriction.stiffness = hit.collider.material.staticFriction;
            wheel.sidewaysFriction = sFriction;
        }

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
        UpdateFriction();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    public SpeedData GetSpeed()
    {
        SpeedData data = new SpeedData();
        data.MaxSpeed = 30;
        data.CurrentSpeed = Mathf.Clamp(rigidbody.velocity.magnitude, 0, 30);
        return data;
    }
}
