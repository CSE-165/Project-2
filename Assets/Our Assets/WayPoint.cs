using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPointSpawner waypointSpawner; // Reference to the WayPointSpawner script
    public Respawn respawn; // Reference to the Respawn script

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            respawn.ResetPosition(transform.position); // Reset the player's position to the waypoint's position
            
            // Destroy the waypoint after it has been reached
            Destroy(gameObject);
            waypointSpawner.AdvanceWayPoint(); // Notify the WayPointSpawner to advance to the next waypoint
        }
    }
}
