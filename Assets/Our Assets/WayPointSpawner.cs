using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointSpawner : MonoBehaviour
{
    public GameObject waypointPrefab; // Prefab for the waypoint to be instantiated
    public GameObject spawnPointPrefab; // Prefab for the spawn point to be instantiated
    private List<Vector3> positions;
    public Parser parser; // Reference to the Parser script
    public Respawn respawn; // Reference to the Respawn script
    private Vector3 curr; // Current position of the waypoint

    // Start is called before the first frame update
    void Start()
    {
    
        positions = parser.ParseFile();
        Instantiate(spawnPointPrefab, positions[0], Quaternion.identity); // Spawn the first waypoint at the starting position
        respawn.SpawnPosition(positions[0]); // Send the starting position to the Respawn script
        positions.RemoveAt(0); // Remove the first position from the list
        AdvanceWayPoint(); // Spawn the first waypoint
    }

    public void AdvanceWayPoint()
    {
        if (positions.Count > 0)
        {
            Vector3 position = positions[0];
            curr = position;
            positions.RemoveAt(0); // Remove the first position from the list
            SpawnWayPoint(position);
        }
    }

    public Vector3 CurrentWayPoint()
    {
        return curr; // Return the current waypoint position
    }
    void SpawnWayPoint(Vector3 position)
    {
        GameObject waypoint = Instantiate(waypointPrefab, position, Quaternion.identity);
    }
}
