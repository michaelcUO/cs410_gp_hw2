using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;

    bool m_IsPlayerInRange;

    // Particle effect to show when the player is detected.
    public GameObject alertEffectPrefab; // NEW.

    // The maximum angle in which the ghost can see the player.
    public float detectionAngle = 60f; // NEW.

    // Gargoyle rotation settings for lerp. NEW.
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
            Debug.Log(gameObject.name + " has detected the player!"); // Log message when player is detected for debugging.
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

        // Handle Lerp rotation for Gargoyles. NEW.
        if (useLerpRotation && rotationTarget != null) {
            lerpTime += Time.deltaTime * rotationSpeed; // Increment lerp time based on speed.
            float angle = Mathf.Lerp(leftAngle, rightAngle, Mathf.SmoothStep(0f, 1f, lerpTime)); // Lerp between left and right angles. Updated to use SmoothStep for smoother transition.
            rotationTarget.localRotation = UnityEngine.Quaternion.Euler(0f, angle, 0f); // Apply rotation to the target.

            if (lerpTime >= 1f) // If lerp time reaches 1, reverse the direction.
            {
                lerpTime = 0f; // Reset lerp time.
                (leftAngle, rightAngle) = (rightAngle, leftAngle); // Swap angles for next rotation.
            }
        }


        // Only run vision check if the player is within range.
        if (m_IsPlayerInRange) { // NEW.

            // Use rotationTarget forward if available (for gargoyle), else use whole object forward (ghost).
            UnityEngine.Vector3 forwardDirection = rotationTarget != null ? rotationTarget.forward : transform.forward; // NEW.


            // Get a normalized direction vector from the ghost to the player.
            UnityEngine.Vector3 toPlayer = (player.position - transform.position).normalized; // NEW.

            // Dot product measures how closely the ghost is facing the player.
            float dot = UnityEngine.Vector3.Dot (forwardDirection, toPlayer); // NEW. Updated for gargoyle.

            // Convert detection angle from degrees to a dot product value.
            float dotValue = Mathf.Cos (detectionAngle * Mathf.Deg2Rad); // NEW.
            Debug.Log ("Dot value: " + dot); // Log the dot value for debugging.

            // If player is within the vision cone (dot product is greater than the dot value).
            if (dot > dotValue) // NEW.
            {
                // Perform a raycast to check if the player is visible (not obstructed by walls or obstacles).
                UnityEngine.Vector3 direction = player.position - transform.position + UnityEngine.Vector3.up;
                Ray ray = new Ray (transform.position, toPlayer);
                RaycastHit raycastHit;
                
                Debug.DrawRay(transform.position, toPlayer * 20f, Color.red);
                if (Physics.Raycast (ray, out raycastHit, 20f))
                {
                    Debug.Log("Raycast hit: " + raycastHit.collider.name); // Log the name of the object hit by the raycast for debugging.
                    // If the raycast hit the player, call CaughtPlayer.
                    if (raycastHit.collider.transform == player)
                    {
                        Instantiate(alertEffectPrefab, player.position + UnityEngine.Vector3.up * 1.5f, UnityEngine.Quaternion.identity); // NEW. Instantiate the alert effect prefab at the player's position. Update the position to be above the player.
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

        // For gargoyle, use rotationTarget forward if available, else use whole object forward (ghost).
        UnityEngine.Vector3 forwardDirection = rotationTarget != null ? rotationTarget.forward : transform.forward;

        Gizmos.DrawRay (transform.position, forwardDirection * 2f);

        // Draw vision cone boundaries in yellow.
        UnityEngine.Vector3 rightBoundary = UnityEngine.Quaternion.Euler (0, detectionAngle, 0f) * forwardDirection;
        UnityEngine.Vector3 leftBoundary = UnityEngine.Quaternion.Euler (0, -detectionAngle, 0f) * forwardDirection;

        Gizmos.color = UnityEngine.Color.yellow;
        Gizmos.DrawRay (transform.position, rightBoundary * 2f);
        Gizmos.DrawRay (transform.position, leftBoundary * 2f);
    }
    
}
