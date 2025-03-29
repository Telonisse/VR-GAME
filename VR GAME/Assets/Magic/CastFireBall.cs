using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CastFireBall : MonoBehaviour
{
    public GameObject fireballPrefab;
    public Transform handTransform;  // The transform where the fireball spawns (hand's position)
    public float spawnOffset = 0.2f; // Offset from the hand so it doesn’t spawn inside the hand
 
    public void TriggerFireSpell()
    {
        CastFireball();

    }
    void CastFireball()
    {
        // Calculate the spawn position based on the hand position and forward direction
        Vector3 spawnPosition = handTransform.position + handTransform.forward * spawnOffset;
        // Instantiate the fireball at the hand position
        Instantiate(fireballPrefab, spawnPosition, handTransform.rotation);
    }
}
