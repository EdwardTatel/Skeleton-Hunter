using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using TMPro;

public class MoveNextArea : MonoBehaviour
{
    [SerializeField] private string sceneToLoad = "SampleScene";
    [SerializeField] private List<AssetReference> prefabsToSpawn;
    [SerializeField] private List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject loadingPage;
    [SerializeField] private GameObject gameOverPage;
    [SerializeField] private GameObject loadingText;
    public static bool startGame = false;


    public Slider slider;
    public float sliderValue;

    private Camera secondSceneCamera;

    public List<GameObject> SpawnPoints
    {
        get { return spawnPoints; }
        set { spawnPoints = value; }
    }

    private void Start()
    {
        startGame = false;
        if (GameOver.gameOver)
        {
            mainMenu.SetActive(false);
            loadingPage.SetActive(false);
            gameOverPage.SetActive(true);
        }
        else
        {
            mainMenu.SetActive(true);
            loadingPage.SetActive(false);
            gameOverPage.SetActive(false);
        }

        slider.value = sliderValue;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        loadingPage.SetActive(true);
        StartCoroutine(LoadSceneAndSpawn());
    }

    IEnumerator LoadSceneAndSpawn()
    {
        
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);

        while (!loadOperation.isDone)
        {
            float progressPercentage = CalculateProgressPercentage();
            slider.value = progressPercentage;

            yield return null;
        }

        Scene loadedScene = SceneManager.GetSceneByName(sceneToLoad);
        if (loadedScene.IsValid())
        {
            foreach (GameObject rootObject in loadedScene.GetRootGameObjects())
            {
                Camera sceneCamera = rootObject.GetComponent<Camera>();
                if (sceneCamera != null)
                {
                    secondSceneCamera = sceneCamera;
                    break;
                }
            }
        }

        if (secondSceneCamera != null)
        {
            secondSceneCamera.enabled = false;
        }

        yield return StartCoroutine(SpawnPrefabs());
    }

    float CalculateProgressPercentage()
    {
        int totalPrefabs = Mathf.Max(spawnPoints.Count, prefabsToSpawn.Count);
        int prefabsSpawned = Mathf.Min(spawnPoints.Count, prefabsToSpawn.Count);

        float progressPercentage = (float)prefabsSpawned / totalPrefabs * 100f;

        return progressPercentage;
    }

    IEnumerator SpawnPrefabs()
    {
        int count = Mathf.Min(spawnPoints.Count, prefabsToSpawn.Count);

        float delayBetweenSpawns = 0.1f;

        float loadingProgress = 0f;

        for (int i = 0; i < count; i++)
        {
            AsyncOperationHandle<GameObject> handle = prefabsToSpawn[i].InstantiateAsync(spawnPoints[i].transform.position, spawnPoints[i].transform.rotation);
            yield return handle;

            GameObject newObject = handle.Result;
            newObject.transform.parent = spawnPoints[i].transform;

            loadingProgress += 100f / count;
            slider.value = loadingProgress * 0.01f;
            loadingText.GetComponent<TextMeshProUGUI>().text = "Loading Game Objects " + (int)loadingProgress + "%";

            if (loadingProgress > 99)
            {
                loadingText.GetComponent<TextMeshProUGUI>().text = "Object Loading Done, Stand By";
                StartCoroutine(FinishScene());
            }
            yield return new WaitForSeconds(delayBetweenSpawns);
        }
    }

    void OnSliderValueChanged(float newValue)
    {
        sliderValue = newValue;
    }
    IEnumerator FinishScene()
    {
        yield return new WaitForSeconds(1f);
        startGame = true;
        yield return new WaitForSeconds(1f);
        secondSceneCamera.enabled = true;
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}