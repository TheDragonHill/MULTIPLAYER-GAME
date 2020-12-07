using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// Dimitrios Kitsikidis

/// <summary>
/// A fail :(, cuz networking not good person.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Ghost : NetworkBehaviour
{

    [SerializeField] private bool m_isAggressive;
    [SerializeField] private float m_speed = 1f;

    #region used for Wander method
    [SerializeField] private float startWaitTime = 10;
    [SerializeField] private float waitTime;

    [SerializeField] private float m_wanderRadius = 2;


    private float m_minX;
    private float m_maxX;

    private float m_minY;
    private float m_maxY;
    #endregion

    private int m_health = 3;

    [SerializeField] private GameObject m_targetPosition;
    private List<GameObject> m_playerList;


    private Rigidbody2D m_rb;


    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_playerList = new List<GameObject>();
        m_playerList.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        m_minX = transform.position.x - 2;
        m_maxX = transform.position.x + 2;
        m_minY = transform.position.y - 2;
        m_maxY = transform.position.y + 2;
    }


    void FixedUpdate()
    {
        if (m_playerList != null)
        {
            Movement();
        }
        waitTime -= Time.fixedDeltaTime;
    }


    void Movement()
    {
        #region Older Version
        //Transform closestPlayer = GetClosestPlayer(m_playerList, this.transform);
        #endregion

        GetClosestTarget();
        transform.LookAt(m_targetPosition.transform);
        Flip();
        m_rb.velocity = transform.forward * m_speed;
    }


    /// <summary>
    /// Gets Distances between Ghost and Players
    /// 
    /// 
    /// When Aggressive..
    /// Chase closest player
    /// 
    /// When Passive..
    /// Wander around
    /// </summary>
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

        StateSwitch(distances);

        // When the Ghost is aggressive....
        #region Compare to which one is the closest player.... Set closest player found as targetPosition

        if (m_isAggressive)
        {
            for (int i = 0; i < m_playerList.Count; i++)
            {
                // <..Stand still>
                // <Play Transform Animation..>
                
                if (Mathf.Abs(Vector3.Distance(transform.position, m_playerList[i].transform.position)) > distances[i])
                {
                    m_targetPosition = m_playerList[i];
                }
            }
        }

        if (m_isAggressive == false)
        {
            Wander();
        }
        #endregion

        #region Older Version of locating players

        //Transform GetClosestPlayer(GameObject[] p_player, Transform p_thisPos)
        //{


        //Transform bestTarget = null;
        //float closestDistanceSqr = Mathf.Infinity;
        //Vector3 currentPosition = p_thisPos.position;

        //foreach (GameObject target in p_player)
        //{
        //    Vector3 directionToTarget = target.transform.position - currentPosition;
        //    float dSqrToTarget = directionToTarget.sqrMagnitude;

        //    if (dSqrToTarget < closestDistanceSqr)
        //    {
        //        closestDistanceSqr = dSqrToTarget;
        //        bestTarget = target.transform;
        //    }
        //}
        //return bestTarget;
        //}
        #endregion
    }


    /// <summary>
    /// Switches Ghost's state to Aggressive or Passive
    /// ...based on Player's distance to it
    /// </summary>
    /// <param name="p_player"></param>
    private void StateSwitch(float[] distances)
    {
        for (int i = 0; i < distances.Length; i++)
        {
            if (distances[i] <= 3 && m_isAggressive == false)
            {
                m_isAggressive = true;
                m_speed = 3f;
            }

            if (Vector3.Distance(transform.position, m_playerList[i].transform.position) > 6)
            {
                m_isAggressive = false;
                m_speed = 1f;
            }
        }
    }


    /// <summary>
    /// Calculates and sets a "wander to position"
    /// ..as targetPosition within a certain radius from the Ghost
    /// </summary>
    private void Wander()
    {
        if (waitTime <= 0f)
        {
            do
            {
                m_targetPosition.transform.position = new Vector3(Random.Range(m_minX, m_maxX), (Random.Range(m_minY, m_maxY)));
                Debug.Log("Attempting to find wander to position");
            } while (Vector3.Distance(transform.position, m_targetPosition.transform.position) > m_wanderRadius);
            Debug.Log("Wander Position Found ! ! !");
            waitTime = Random.Range(0f, startWaitTime);
        }
    }


    private void Flip()
    {
        if (transform.rotation.z >= 180f || transform.rotation.z <= -180f)
        {
            transform.Rotate(0f, 180f, 0f);
            Debug.Log("Ready to flip");
        }
    }


    public void TakeDamage(int DamageTaken)
    {
        m_health -= DamageTaken;

        // <Play Damaged Animation>

        //m_canBeHitted = false;
        //m_hittedTime = m_startHittedTime;
    }

    //private void Attack()
    //{
    //    // <Play Attack Animation>

    //    Collider2D[] collisionsInCastArea = Physics2D.OverlapCircleAll(transform.up, 1f);
    //    for (int i = 0; i < collisionsInCastArea.Length; i++)
    //    {
    //        // If the Entity.state == Neutral or Bad or Good & Entity.type  == Aggresive
    //        if (collisionsInCastArea[i].GetComponent<PlayerManager>().m_isDead == false &&
    //            collisionsInCastArea[i].GetComponent<PlayerManager>().m_canBeHitted)
    //        {
    //            collisionsInCastArea[i].GetComponent<PlayerManager>().TakeDamage(1);
    //        }
    //    }
    //}
}
