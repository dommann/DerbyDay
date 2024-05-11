using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCamera : MonoBehaviour
{
    public Transform target; // The object the camera will follow
    public float smoothSpeed = 0.125f; // The smoothness of camera movement
    private bool _isInitialized = false;
    void LateUpdate()
    {
        if(!_isInitialized)
            return;

        target = RaceController.Instance.GetLeadingHorse().transform;

        if (target != null)
        {
            // Get the position of the target
            Vector3 desiredPosition = transform.position;
            desiredPosition.z = target.position.z; // Only consider the z-axis component of the target's position

            // Smoothly move towards the target position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Update the camera's position
            transform.position = smoothedPosition;
        }
    }

    public void Initialize()
    {
//        target = RaceController.Instance.GetLeadingHorse().transform;
        _isInitialized = true;
    }
}
