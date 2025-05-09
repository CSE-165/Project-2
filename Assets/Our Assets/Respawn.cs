using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{

    Vector3 resetPosition; 

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            // Reset the player's position to the starting point
            transform.position = resetPosition;
        }
    }

    public void ResetPosition(Vector3 position)
    {
        this.resetPosition = position;
    }

    public void SpawnPosition(Vector3 position)
    {
        transform.position = position;
    }

}