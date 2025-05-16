using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    Vector3 resetPosition;
    public GameObject player;

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("CRASH");
            // Reset the player's position to the starting point
            //transform.position = resetPosition;
            player.transform.position = resetPosition;
        }

    }

    public void ResetPosition(Vector3 position)
    {
        this.resetPosition = position;
    }

    public void SpawnPosition(Vector3 position)
    {
        //transform.position = position;
        player.transform.position = position;
    }

}