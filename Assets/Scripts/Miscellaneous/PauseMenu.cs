using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
// Michael Schmidt

public class PauseMenu : NetworkBehaviour
{
    [SerializeField]
    private GameObject m_uiPauseMenu;

    private NetworkManager m_network;

    void Start()
    {
        m_network = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    void Update()
    {
        // open or close pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }


    public void PauseGame()
    {
        // checks after pressing "escape" button if pause Menu is active or not

        // if pause Menu is not active
        if (m_uiPauseMenu.activeInHierarchy == false)
        {
            // pause Menu is setting active
            m_uiPauseMenu.gameObject.SetActive(true);

        }

        // if pause menu is active after pressing "escape" button
        else
        {          
            // pause menu is setting inactive
            m_uiPauseMenu.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// toggle pausemenu by pressing the button
    /// </summary>
    public void ButtonResumeGame()
    {
        PauseGame();
    }

    /// <summary>
    /// Disconnect from Server when Client
    /// Shutdown Server when Host
    /// </summary>
    public void ButtonBackToMenu()
    {
        m_network.StopHost(); 
    }
}