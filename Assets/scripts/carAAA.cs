using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAAA : MonoBehaviour
{
    // Wheel Transforms
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // Rotation Speed
    [SerializeField] private float wheelRotationSpeed = 30f; // Adjust as needed

    private void Update()
    {
        RotateWheels();
    }

    private void RotateWheels()
    {
        // Simulate wheel rotation
        frontLeftWheelTransform.Rotate(Vector3.right * wheelRotationSpeed * Time.deltaTime);
        frontRightWheelTransform.Rotate(Vector3.right * wheelRotationSpeed * Time.deltaTime);
        rearLeftWheelTransform.Rotate(Vector3.right * wheelRotationSpeed * Time.deltaTime);
        rearRightWheelTransform.Rotate(Vector3.right * wheelRotationSpeed * Time.deltaTime);
    }
}
