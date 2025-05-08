using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public GameObject waypointPrefab; // Prefab for the waypoint to be instantiated
    private List<Vector3> positions;
    // Start is called before the first frame update
    void Start()
    {
        Parser parse = new Parser();
        positions = parser.ParseFile();
        AdvanceWayPoint();
    }

    void AdvanceWayPoint()
    {
        if (positions.Count > 0)
        {
            Vector3 position = positions[0];
            positions.RemoveAt(0);
            SpawnWayPoint(position);
        }
    }
    void SpawnWayPoint(Vector3 postion)
    {
        GameObject waypoint = Instantiate(waypointPrefab, position, Quaternion.identity);
    }
}
