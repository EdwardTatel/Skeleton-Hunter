using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    public string menuSceneName = "Menus"; 
    public GameObject[] gameObjectsToActivate; 

    private bool isLoading = false; 
    public static bool gameOver = false;


    private void Start()
    {
        gameOver = false;
    }
   
    void Update()
    {
        
        if (gameOver)
        {
            
            SceneManager.LoadSceneAsync(menuSceneName, LoadSceneMode.Additive);
            gameOver = false;
            AsyncLoader.startGame = false;
            
            
        }
    }


}