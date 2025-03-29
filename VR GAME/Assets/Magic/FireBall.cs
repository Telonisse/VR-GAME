using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float fireballSpeed = 10f;
    public float lifeTime = 5f;
    //public GameObject explosionPrefab;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Apply initial force in the direction the hand is facing
        rb.velocity = transform.forward * fireballSpeed;
        // Destroy the fireball after a set lifetime
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Instantiate the explosion at the point of impact
       // if (explosionPrefab != null)
        //{
          //  Instantiate(explosionPrefab, transform.position, Quaternion.identity);
       // }

        // Destroy the fireball upon collision
        Destroy(gameObject);
    }
}
