using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    private GameObject menuManager;
    [SerializeField] private List<GameObject> spawnPoints;
    void Start()
    {
        if (GameObject.Find("Manager") != null)
        {
            GameObject.Find("Manager").GetComponent<AsyncLoader>().SpawnPoints = spawnPoints;
        }
    }

}
