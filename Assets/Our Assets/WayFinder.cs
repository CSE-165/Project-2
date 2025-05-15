using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayFinder : MonoBehaviour
{
    [Header("References")]
    public Transform playerRig; // Reference to the player's rig (camera and body)
    public GameObject arrowPrefab; // Prefab for the arrow to be instantiated
    private GameObject arrow; // Reference to the instantiated arrow
    public WayPointSpawner wayPoint;
    private Vector3 currPosition;

    // Start is called before the first frame update
    void Start()
    {
        currPosition = wayPoint.CurrentWayPoint(); // Get the current waypoint position

        arrow = Instantiate(arrowPrefab, Camera.main.transform.position + new Vector3(0, -3,0)+ Camera.main.transform.forward * 4f , Quaternion.identity); // Instantiate the arrow prefab

        //Spawn arrow infront of rig pointing at the first waypoint
        Vector3 direction = currPosition - arrow.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, direction);

        arrow.transform.rotation = rotation; // Rotate the arrow to point towards the waypoint
    

        
    }


    // Update is called once per frame
    void Update()
    {
        currPosition = wayPoint.CurrentWayPoint(); // Update the current waypoint position
        Vector3 direction = currPosition - arrow.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction, direction) * Quaternion.Euler(90,0,0); // Calculate the rotation to point towards the waypoint
        arrow.transform.position = Camera.main.transform.position + new Vector3(0, -3,0) + Camera.main.transform.forward * 4f; // Move the arrow to the player's position
        arrow.transform.rotation = rotation; // Rotate the arrow to point towards the waypoint
    }
}
