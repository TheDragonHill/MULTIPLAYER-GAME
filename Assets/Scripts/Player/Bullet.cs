using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// Dimitrios Kitsikidis
// Michael Schmidt

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : NetworkBehaviour
{

    public float m_bulletSpeed;

    [HideInInspector] public Rigidbody2D m_rb;


    private void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Gives the bullet movement
    private void FixedUpdate()
    {
        m_rb.velocity = transform.up * m_bulletSpeed;
    }


    // Check collision tag and call the damaging method from the collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyManager>().TakeDamage(1);
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

}
