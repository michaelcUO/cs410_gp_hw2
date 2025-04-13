using UnityEngine;

public class SoundTrigger : MonoBehaviour
{
    public AudioClip spookySound; // The sound to play when the player enters the trigger.
    public float volume = 1.0f; // Volume of the sound.

    // This method is called when another collider enters the trigger collider attached to this GameObject.
    private void OnTriggerEnter(Collider other) 
    {
      if (other.CompareTag("Player")) // Check if the object that entered the trigger has the "Player" tag.
      {
          Debug.Log("Player entered spooky sound trigger!"); // Log message for debugging.
          AudioSource.PlayClipAtPoint(spookySound, transform.position, volume); // Play the spooky sound at the trigger's position.
          Destroy(gameObject); // Destroy the trigger after playing the sound.
      }
    }
}
