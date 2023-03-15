using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class MainMenu : MonoBehaviour
{
    public void ExitGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}
