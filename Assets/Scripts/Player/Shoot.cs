using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// Dimitrios Kitsikidis
// Michael Schmidt

[RequireComponent(typeof(Animator))]
public class Shoot : NetworkBehaviour
{

    [SerializeField]
    private GameObject m_bullet;

    [SerializeField]
    private Transform m_firingPosition;

    Animator m_animator;

    #region minitimers
    float m_shot, m_shotStart = 0.1f;
    #endregion

    private void Start()
    {
        m_animator = GameObject.Find("Gun").GetComponent<Animator>();
    }


    void FixedUpdate()
    {
        m_shot -= Time.fixedDeltaTime;
        if (isLocalPlayer)
        {
            CmdRotation();

            if (Input.GetMouseButtonDown(0))
            {
                m_animator.SetBool("Shooting", true);
                m_shot = m_shotStart;
                CmdShoot();
            }
            if (m_shot <= 0f) m_animator.SetBool("Shooting", false);
        }
    }

    // Rotate towards mouse
    [Command]
    void CmdRotation()
    {
        Vector2 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
        lookPos = lookPos - transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // A flip function that does not work.. yay... xD
    private void Flip()
    {
        if (transform.localRotation.z >= 90f || transform.localRotation.z <= -90f)
        {
            transform.Rotate(0f, 180f, 0f);
            Debug.Log("Ready to flip");
        }
    }

    // Shoot a bullet
    [Command]
    void CmdShoot()
    {
        Debug.Log("sds");

        GameObject bullet = Instantiate(m_bullet, m_firingPosition.position, m_firingPosition.rotation);

        NetworkServer.Spawn(bullet);
        Destroy(bullet, 4);
    }
}
