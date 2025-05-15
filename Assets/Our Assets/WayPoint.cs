using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPointSpawner waypointSpawner; // Reference to the WayPointSpawner script
    public Respawn respawn; // Reference to the Respawn script
    public AudioSource waypointChecked; // Reference to the AudioSource component

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            respawn.ResetPosition(transform.position); // Reset the player's position to the waypoint's position
            
            // Destroy the waypoint after it has been reached
            Destroy(this.gameObject);
            // Play the audio clip
            if (waypointChecked != null)
            {
                waypointChecked.Play();
            }

            waypointSpawner.AdvanceWayPoint(); // Notify the WayPointSpawner to advance to the next waypoint
        }
    }
}
