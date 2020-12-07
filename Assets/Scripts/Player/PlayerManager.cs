using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// Michael Schmidt
// Henrik Hafner
// Dimitrios Kitsikidis

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NetworkIdentity))]


public class PlayerManager : NetworkBehaviour
{

    [SerializeField]
    private float m_movementSpeed;

    public bool m_isDead, m_canBeHitted;
    private int m_health = 6;

    #region Timers
    private float m_startHittedTime = 1f, m_hittedTime;
    #endregion

    private Rigidbody2D m_rb;
    private Animator m_animator;

    public GameObject playerCamera, damagedParticle;
    GameObject playerCameraInst;
    Vector3 camOffset;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_animator = GetComponent<Animator>();

        if (isLocalPlayer == true)
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }

        playerCameraInst = Instantiate(playerCamera, new Vector3(transform.position.x, transform.position.y, -10f), Quaternion.identity);
        camOffset = playerCameraInst.transform.position - transform.position;
    }

    private void LateUpdate()
    {
        playerCameraInst.transform.position = transform.position + camOffset;
    }

    void FixedUpdate()
    {
        m_hittedTime -= Time.fixedDeltaTime;
        if (m_hittedTime <= 0) m_canBeHitted = true;

        if (this.isLocalPlayer)
        {
            if (m_isDead) return;
            else if (!m_isDead)
            {
                if (m_health < 0)
                {
                    // <..Play death animation..>

                    m_isDead = true;
                }
                else if (m_health > 0) Movement();
            }
        }

    }

    // Movement and it's animation handler
    void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        m_rb.velocity = new Vector2(moveHorizontal,moveVertical) * m_movementSpeed;

        #region we can also use ... , not important though
        //transform.Translate(new Vector2(moveHorizontal, moveVertical));
        #endregion
        #region Where the magic of Animation happens

        //Changes Animation Based on direction facing.
        m_animator.SetFloat("FaceX", moveHorizontal);
        m_animator.SetFloat("FaceY", moveVertical);

        // If player moves change to the fitting animation
        if (moveHorizontal != 0 || moveVertical != 0)
        {
            m_animator.SetBool("isWalking", true);
            if (moveHorizontal > 0) m_animator.SetFloat("LastMoveX", 1f);
            else if (moveHorizontal < 0) m_animator.SetFloat("LastMoveX", -1f);
            else m_animator.SetFloat("LastMoveX", 0f);

            if (moveVertical > 0) m_animator.SetFloat("LastMoveY", 1f);
            else if (moveVertical < 0) m_animator.SetFloat("LastMoveY", -1f);
            else m_animator.SetFloat("LastMoveY", 0f);
        }
        else
        {
            m_animator.SetBool("isWalking", false);
        }
        #endregion
    }

    // Allows player to lose health
    public void TakeDamage(int DamageTaken)
    {
        m_health -= DamageTaken;
        m_canBeHitted = false;
        m_hittedTime = m_startHittedTime;
    }

    //void Rotation()
    //{
    //    Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
    //    Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
    //    lookPos = lookPos - transform.position;
    //    float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //}


    //[Command]
    //void CmdShoot()
    //{
    //    Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
    //    Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);

    //    lookPos = lookPos - transform.position;
    //    lookPos.Set(lookPos.x, lookPos.y, 0);

    //    float distance = lookPos.magnitude - 1;

    //    RaycastHit2D hit = Physics2D.Raycast(m_firingPosition.position, lookPos, distance);

    //    Debug.DrawRay(m_firingPosition.position, lookPos, Color.red);

    //    if (hit == gameObject && hit.collider.tag == "Enemy")
    //    {
    //        Debug.Log(hit.collider.name + "Hitted");

    //        Destroy(hit.collider.gameObject);
    //    }
    //}
}

