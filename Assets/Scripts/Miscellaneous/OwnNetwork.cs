using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
// Michael Schmidt

public class OwnNetwork : MonoBehaviour
{
    private NetworkManager m_network;

    private void Awake()
    {
        m_network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    /// <summary>
    /// Hosting a server
    /// </summary>
    public void StartServer()
    {
        m_network.StartHost();
    }

    /// <summary>
    /// Joining a server
    /// </summary>
    public void StartClient()
    {  
        m_network.StartClient();
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void ButtonExitGame()
    {
        Application.Quit();
    }
}
