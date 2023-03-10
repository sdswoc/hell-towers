using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AsyncManager : MonoBehaviour
{
    [Header("Menu Screens")]
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject mainMenu;

    [Header("Slider")]
    [SerializeField] private Slider loadingSlider;


    public void StartGameBtn()
    {
        mainMenu.SetActive(false);
        loadingScreen.SetActive(true);

        //run the async
    }

    //IEnumerator StartLevelAsync()
    //{
    //    AsyncOperation loadOperation = SceneManager.LoadSceneAsync 
    //}
}
