# John Lemon's Haunted Jaunt - Gameplay Elements README

## Team Members

| Michael Cheung | 

Implemented all new gameplay elements including dot product detection, lerp-based gargoyle rotation, particle effects on detection, and sound triggers. Integrated new gameplay logic and tested interactions in Unity.|

---

## Gameplay Elements & Technical Descriptions

### Dot Product (Facing Direction Detection)
I used the dot product to determine if the ghost or gargoyle is facing the player within their cone of vision.

- In `Observer.cs`, I calculate the dot product between the forward direction of the ghost or gargoyle (either `transform.forward` or `rotationTarget.forward`) and the vector pointing from the observer to the player.
- If the dot product is greater than a calculated threshold (based on detection angle), and the player is within the trigger area, the enemy performs a raycast to check if the player is in clear sight.
- If successful → the player is caught.

---

### Lerp (Linear Interpolation for Gargoyle Rotation)
To create tension and increase the challenge, I implemented linear interpolation (lerp) to have the gargoyle slowly and smoothly sweep a cone of vision back and forth like a security camera (imitating their head movement) and indicated by a red light to the player.

- This was handled in `Observer.cs` by interpolating between a left angle and a right angle using `Mathf.Lerp` combined with `Mathf.SmoothStep` for smoother transitions.
- The lerp behavior only activates for objects with `useLerpRotation` enabled and a designated `rotationTarget`.

---

### Particle Effect (Detection Burst Effect)
To make enemy detection feel more dramatic, I created a red particle effect that erupts from the player when spotted.

- The particle effect, a burst of red ghostly energy, spawns directly above the player when caught by either a ghost or a gargoyle.
- This is handled in `Observer.cs` by instantiating a `GhostAlertEffect` prefab when the player is detected.
- The particles expand outward rapidly to fill the area for visual impact.

---

### Sound Effect (Spooky Voice Trigger)
I added an ambient sound trigger to build tension when the player approaches a key area.

- When the player moves to the right and approaches the hallway where the ghost patrols, a ghostly voice whispers "I see you..." to make the situation more spooky.
- This is handled using a `SoundTrigger.cs` script attached to a trigger collider in that hallway. When the player enters this zone (checked using `CompareTag("Player")`), the sound plays at that location.

---

## How to See These Elements In-Game

| Dot Product (Vision Detection) | Any ghost or gargoyle → They only catch you if you are within their vision cone and line of sight |

| Lerp (Head Sweep) | Gargoyles in the level have a rotating cone of vision that sweeps side to side (imitating their heads) like security cameras |

| Particle Effect | Triggered immediately when a ghost or gargoyle catches the player → a red energy burst appears above the player |

| Sound Effect | Walk to the right side of the map before the ghost hallway → ghost voice whispers "I see you..." |