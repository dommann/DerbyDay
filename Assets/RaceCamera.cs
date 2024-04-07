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
        if(_isInitialized == false) 
            return; 
        target = RaceController.Instance.GetLeadingHorse().transform;
        if (target != null)
        {
            Vector3 desiredPosition = target.position; // Get the position of the target
            Vector3 smoothedPosition =
                Vector3.Lerp(transform.position, desiredPosition,
                    smoothSpeed); // Smoothly move towards the target position
            transform.position = smoothedPosition; // Update the camera's position
        }
    }

    public void Initialize()
    {
//        target = RaceController.Instance.GetLeadingHorse().transform;
        _isInitialized = true;
    }
}
