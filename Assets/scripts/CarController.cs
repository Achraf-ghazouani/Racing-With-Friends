using UnityEngine;

public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;
    private Rigidbody rb;

    // Lap & Position Tracking
    public int currentLap = 0;
    public int lastPassedWaypoint = 0; // Track waypoint progress

    // Settings
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;
    [SerializeField] private float decelerationRate = 20f; // Rate of deceleration when no input

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // Brake Lights
    [SerializeField] private Light brakeLightLeft;
    [SerializeField] private Light brakeLightRight;

    // Player Input Configuration
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";
    [SerializeField] private KeyCode brakeKey = KeyCode.Space;

    // Car Engine Sound Settings
    [SerializeField] private AudioSource engineSound;  // AudioSource for the engine sound
    [SerializeField] private float minPitch = 0.8f;     // Minimum pitch at low speed
    [SerializeField] private float maxPitch = 2.5f;     // Maximum pitch at high speed
    [SerializeField] private float maxVolume = 1f;      // Maximum volume at high speed

    public bool isDisabled = false; // Property to check if the car is disabled

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isDisabled) return; // Skip control updates if the car is disabled

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
        UpdateBrakeLights();
        UpdateEngineSound();  // Update the engine sound based on speed
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis(horizontalAxis);
        verticalInput = Input.GetAxis(verticalAxis);
        isBreaking = Input.GetKey(brakeKey);
    }

    private void HandleMotor()
    {
        float decelerationMultiplier = 20f; // Increase this value to slow down faster

        if (Mathf.Abs(verticalInput) > 0.1f) // If player is pressing acceleration or brake
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        else
        {
            // Gradually reduce speed when no vertical input is given
            frontLeftWheelCollider.motorTorque = Mathf.Lerp(frontLeftWheelCollider.motorTorque, 0, Time.deltaTime * decelerationRate * decelerationMultiplier);
            frontRightWheelCollider.motorTorque = Mathf.Lerp(frontRightWheelCollider.motorTorque, 0, Time.deltaTime * decelerationRate * decelerationMultiplier);
        }

        if (Mathf.Abs(horizontalInput) < 0.1f) // If player is not steering
        {
            // Gradually reduce speed when no horizontal input is given
            frontLeftWheelCollider.motorTorque = Mathf.Lerp(frontLeftWheelCollider.motorTorque, 0, Time.deltaTime * decelerationRate * decelerationMultiplier);
            frontRightWheelCollider.motorTorque = Mathf.Lerp(frontRightWheelCollider.motorTorque, 0, Time.deltaTime * decelerationRate * decelerationMultiplier);
        }

        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();

        // Start engine sound when the car starts moving
        if (engineSound != null && !engineSound.isPlaying && rb.velocity.magnitude > 0.1f)
        {
            engineSound.loop = true; // Ensure the engine sound loops
            engineSound.Play();
        }
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void UpdateBrakeLights()
    {
        bool lightsActive = isBreaking;
        brakeLightLeft.enabled = lightsActive;
        brakeLightRight.enabled = lightsActive;
    }

    // Update the engine sound based on speed
    private void UpdateEngineSound()
    {
        if (engineSound != null)
        {
            // Get current speed in km/h
            float speed = GetCurrentSpeed();

            // Normalize speed (between 0 and 1)
            float speedFactor = speed / 100f; // Adjust max speed as needed

            // Set pitch based on speed
            engineSound.pitch = Mathf.Lerp(minPitch, maxPitch, speedFactor);

            // Set volume based on speed
            engineSound.volume = Mathf.Lerp(0.2f, maxVolume, speedFactor);
        }
    }

    public float GetCurrentSpeed()
    {
        return rb.velocity.magnitude * 3.6f; // Convert from m/s to km/h
    }

    // Method to disable the car's controls
    public void DisableCar()
    {
        isDisabled = true;
        rb.velocity = Vector3.zero; // Stop the car
        rb.angularVelocity = Vector3.zero; // Stop any rotation
    }
}