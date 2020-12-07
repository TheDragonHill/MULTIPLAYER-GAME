using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// Michael Schmidt
// Dimitrios Kitsikidis

public class EnemyManager : NetworkBehaviour
{
    [SerializeField] private float m_speed = 1;

    [SerializeField] private GameObject m_targetPosition;

    [SerializeField] private int m_health = 3;

    private bool m_FacingRight;

    private Animator animator;

    public GameObject damagedParticle;
    private List<GameObject> m_playerList = new List<GameObject>();

    private Rigidbody2D m_rb;


    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        m_playerList.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }


    void FixedUpdate()
    {
        if (m_health <= 0)
        {
            animator.SetBool("death", true);
            Destroy(gameObject, 1.2f);
        }

        if (m_playerList != null)
        {
            Movement();
        }
    }


    void Movement()
    {
        GetClosestTarget();
        if (Vector2.Distance(transform.position, m_targetPosition.transform.position) > 1.2f)
        {
            transform.position = Vector2.MoveTowards(transform.position, m_targetPosition.transform.position, m_speed * Time.fixedDeltaTime);

            Vector2 direction = m_targetPosition.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, m_targetPosition.transform.rotation, m_speed * Time.fixedDeltaTime);
        }
    }

    public void TakeDamage(int DamageTaken)
    {
        m_health -= DamageTaken;
    }

    private void GetClosestTarget()
    {
        float[] distances = new float[m_playerList.Count];
        m_targetPosition = m_playerList[Random.Range(0, m_playerList.Count)];

        #region Get Distance from all players in the PlayerList

        for (int i = 0; i < m_playerList.Count; i++)
        {
            if (m_playerList[0] != null)
            {
                distances[i] = Mathf.Abs(Vector3.Distance(transform.position, m_playerList[i].transform.position));
            }
        }
        #endregion

        // When the Ghost is aggressive....
        #region Compare to which one is the closest player.... Set closest player found as targetPosition

        for (int i = 0; i < m_playerList.Count; i++)
        {
            if (Mathf.Abs(Vector3.Distance(transform.position, m_playerList[i].transform.position)) > distances[i])
            {
                m_targetPosition = m_playerList[i];
            }
        }
        #endregion
    }


    // Check for player and damage
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerManager>().TakeDamage(1);
        }
    }

    //private void Flip()
    //{
    //    // If the input is moving the player right and the player is facing left...
    //    if (move > 0 && !m_FacingRight)
    //    {
    //        Flip();
    //    }
    //    // Otherwise if the input is moving the player left and the player is facing right...
    //    else if (move < 0 && m_FacingRight)
    //    {
    //        Flip();
    //    }

    //    // Switch the way the player is labelled as facing.
    //    m_FacingRight = !m_FacingRight;

    //    transform.Rotate(0f, 180f, 0f);
    //}
}
