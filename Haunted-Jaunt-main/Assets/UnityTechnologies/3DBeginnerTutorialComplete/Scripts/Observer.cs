using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;

    // The maximum angle in which the ghost can see the player.
    public float detectionAngle = 60f; // NEW.

    // Gargoyle rotation settings. NEW.
    public bool useLerpRotation = false;
    public Transform rotationTarget; // Assign gargoyle rotation target in the Inspector.
    public float leftAngle = -45f; // Left rotation angle for Lerp rotation.
    public float rightAngle = 45f; // Right rotation angle for Lerp rotation.
    public float rotationSpeed = 2f; // Speed of rotation Lerp.
    private float lerpTime = 0f; // Time variable for Lerp rotation.
    


    void OnTriggerEnter (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }

    void OnTriggerExit (Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update ()
    {
        // Only run vision check if the player is within range.
        if (m_IsPlayerInRange) { // NEW.
            // Get a normalized direction vector from the ghost to the player.
            UnityEngine.Vector3 toPlayer = (player.position - transform.position).normalized; // NEW.

            // Dot product measures how closely the ghost is facing the player.
            float dot = UnityEngine.Vector3.Dot (transform.forward, toPlayer); // NEW.

            // Convert detection angle from degrees to a dot product value.
            float dotValue = Mathf.Cos (detectionAngle * Mathf.Deg2Rad); // NEW.

            // If player is within the vision cone (dot product is greater than the dot value).
            if (dot > dotValue) // NEW.
            {
                // Perform a raycast to check if the player is visible (not obstructed by walls or obstacles).
                UnityEngine.Vector3 direction = player.position - transform.position + UnityEngine.Vector3.up;
                Ray ray = new Ray (transform.position, toPlayer);
                RaycastHit raycastHit;
                
                if (Physics.Raycast (ray, out raycastHit))
                {
                    // If the raycast hit the player, call CaughtPlayer.
                    if (raycastHit.collider.transform == player)
                    {
                        gameEnding.CaughtPlayer ();
                    }
                }
            }
        }
        
    }

    // Draw visualization of the detection angle in the Scene view for debugging.
    void OnDrawGizmos() // NEW.
    {
        // Draw forward direction in green.
        Gizmos.color = UnityEngine.Color.green;
        Gizmos.DrawRay (transform.position, transform.forward * 2f);

        // Draw vision cone boundaries in yellow.
        UnityEngine.Vector3 rightBoundary = UnityEngine.Quaternion.Euler (0, detectionAngle, 0f) * transform.forward;
        UnityEngine.Vector3 leftBoundary = UnityEngine.Quaternion.Euler (0, -detectionAngle, 0f) * transform.forward;

        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawRay (transform.position, rightBoundary * 2f);
        Gizmos.DrawRay (transform.position, leftBoundary * 2f);
    }
    
}
