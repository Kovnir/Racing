using System;
using Signals;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Zenject;

public class CarController : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    private float steeringAngle;
    
    [Header("Wheel Colliders")]
    [SerializeField]
    private WheelCollider frontLeftWheelCollider;
    [SerializeField]
    private WheelCollider frontRightWheelCollider;
    [SerializeField]
    private WheelCollider backLeftWheelCollider;
    [SerializeField]
    private WheelCollider backRightWheelCollider;

    [Header("Wheel Transforms")]
    [SerializeField]
    private Transform frontLeftWheelTransform;
    [SerializeField]
    private Transform frontRightWheel;
    [SerializeField]
    private Transform backLeftWheelTransform;
    [SerializeField]
    private Transform backRightWheelTransform;
    
    [Header("Trails for Drift")]
    [SerializeField] private TrailRenderer flTrail;
    [SerializeField] private TrailRenderer frTrail;
    [SerializeField] private TrailRenderer blTrail;
    [SerializeField] private TrailRenderer brTrail;
    
    [Header("Trails for Grass")]
    [SerializeField] private TrailRenderer flGrassTrail;
    [SerializeField] private TrailRenderer frGrassTrail;
    [SerializeField] private TrailRenderer blGrassTrail;
    [SerializeField] private TrailRenderer brGrassTrail;
    
    [Header("Particles for Grass")]
    [SerializeField] private ParticleSystem blGrassParticles;
    [SerializeField] private ParticleSystem brGrassParticles;

    [Header("Other")]
    [SerializeField] private float stopForce;
    [SerializeField] private float maxSteerAngle = 30;
    [SerializeField] private float motorForce = 50;

    [SerializeField] private CarSound sound;
    [SerializeField] private GameObject brakeLights;
    
    private bool canControl = true;
    private Rigidbody rigidbody;
    private Vector3 locVelocity;
    private bool stop;
    private LightEffects lightEffects;
    [Inject] private DiContainer container;
    [Inject] private SignalBus bus;
    
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        sound.PlayStart();
        lightEffects = GetComponent<LightEffects>();
        //lightEffects
    }

    public void GetInput()
    {
        float horizontalInputPC = Input.GetAxis("Horizontal");
        float verticalInputPC = Input.GetAxis("Vertical");
        float horizontalInputMobile = CrossPlatformInputManager.GetAxis("Horizontal");
        float verticalInputMobile = CrossPlatformInputManager.GetAxis("Vertical");

        if (Mathf.Abs(verticalInputPC) > Mathf.Abs(verticalInputMobile))
        {
            verticalInput = verticalInputPC;
        }
        else
        {
            verticalInput = verticalInputMobile;
        }
        if (Mathf.Abs(horizontalInputPC) > Mathf.Abs(horizontalInputMobile))
        {
            horizontalInput = horizontalInputPC;
        }
        else
        {
            horizontalInput = horizontalInputMobile;
        }

        
        locVelocity = transform.InverseTransformDirection(rigidbody.velocity);
        stop = Input.GetAxis("Vertical") < 0 && (locVelocity.z > 0);
        if (!stop)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                stop = true;
            }
        }
    }

    private void UpdateTrails()
    {
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

        if (rigidbody.velocity.magnitude > 10 && Vector3.Angle(transform.forward, rigidbody.velocity) > 20)
        {
            flTrail.emitting = true;
            frTrail.emitting = true;
            blTrail.emitting = true;
            brTrail.emitting = true;
            sound.StartDrifting();
        }
        else
        {
            sound.EndDrifting();
        }

        //don't draw trails in air
        flTrail.emitting &= frontLeftWheelCollider.isGrounded;
        frTrail.emitting &= frontRightWheelCollider.isGrounded;

        blTrail.emitting &= backLeftWheelCollider.isGrounded;
        brTrail.emitting &= backRightWheelCollider.isGrounded;
    }

    private void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalInput * (stop ? 2 : 1);
        frontLeftWheelCollider.steerAngle = steeringAngle;
        frontRightWheelCollider.steerAngle = steeringAngle;
    }

    private void Accelerate()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        if (verticalInput > 0)
        {
            sound.StartBoost();
        }
        else
        {
            sound.EndBoost();            
        }
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
    }

    private void FixedUpdate()
    {
        if (canControl)
        {
            GetInput();
            CheckTurningOver();
        }
        else
        {
            verticalInput = 0;
            horizontalInput = 0;
            stop = true;
        }
        UpdateBrakeLights();
        UpdateTrails();
        UpdateFriction();
        Steer();
        Accelerate();
        UpdateWheelPoses();
        EnableGrassTrails();
        EnableGrassParticles();
    }

    private void EnableGrassParticles()
    {     
        EnableGrassParticles(backLeftWheelCollider, blGrassParticles);
        EnableGrassParticles(backRightWheelCollider, brGrassParticles);
    }

    private float timeAfterTurningOver;
    private void CheckTurningOver()
    {
        if (timeAfterTurningOver >= 2)
        {
            bus.Fire(new OnLevelFailedSignal(OnLevelFailedSignal.FailReason.CarCrushed));
        }
        else
        {
            if (rigidbody.velocity.magnitude < 1)
            {
                //if upside down
                if (Math.Abs(transform.up.y - -1) < 0.1f)
                {
                    timeAfterTurningOver += Time.deltaTime;
                    return;
                }
            }
            timeAfterTurningOver = 0;
        }
    }

    private void UpdateBrakeLights()
    {
        brakeLights.SetActive(verticalInput < 0 || stop);
    }

    private void EnableGrassTrails()
    {
        EnableGrassTrail(frontLeftWheelCollider, flGrassTrail);
        EnableGrassTrail(frontRightWheelCollider, frGrassTrail);
        EnableGrassTrail(backLeftWheelCollider, blGrassTrail);
        EnableGrassTrail(backRightWheelCollider, brGrassTrail);
    }

    private void EnableGrassTrail(WheelCollider wheel, TrailRenderer grassTrail)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            if (hit.collider.name == "Grass")
            {
                grassTrail.emitting = true;
                return;
            }
        }
        grassTrail.emitting = false;
    }
    
    private void EnableGrassParticles(WheelCollider wheel, ParticleSystem grassParticles)
    {
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            if (hit.collider.name == "Grass" && verticalInput > 0)
            {
                if (!grassParticles.isPlaying)
                {
                    grassParticles.Play();
                }
                return;
            }
        }

        grassParticles.Stop();
    }

    public SpeedData GetSpeed()
    {
        SpeedData data = new SpeedData();
        data.MaxSpeed = 30;
        data.CurrentSpeed = Mathf.Clamp(rigidbody.velocity.magnitude, 0, 30);
        return data;
    }

    public void TakeControl()
    {
        canControl = false;
    }

    public void ReturnControl()
    {
        canControl = true;
    }
}
