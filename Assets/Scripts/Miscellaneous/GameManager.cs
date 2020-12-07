using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
// Michael Schmidt

public class GameManager : NetworkBehaviour
{

    [SyncVar]
    private int m_clientCounter;

    [SerializeField]
    private Text m_textClientCounter;

    void Update()
    {
        if (isServer)
        {
            m_clientCounter = GameObject.FindGameObjectsWithTag("Player").Length;
            RpcConnectedClients();
        }
    }

    [ClientRpc]
    void RpcConnectedClients()
    {
        m_textClientCounter.text = m_clientCounter.ToString();
    }
}
