using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSceneLoader : MonoBehaviour
{
    public SpawnEnemy SpawnEnemy;
    [SerializeField] private string sceneToLoad = "HouseInterior";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    public bool IsDoorOpen;

    private const string ReturnSceneKey = "ReturnScene"; // Key for storing the return scene name in PlayerPrefs
    private const string ReturnXKey = "ReturnX"; // Key for storing the return X position in PlayerPrefs
    private const string ReturnYKey = "ReturnY"; // Key for storing the return Y position in PlayerPrefs
    private const string ReturnZKey = "ReturnZ"; // Key for storing the return Z position in PlayerPrefs
    private const string ReturnRotYKey = "ReturnRotY"; // Key for storing the return Y rotation in PlayerPrefs

    private bool playerInTrigger;
    private Transform player;

    public void Start()
    {
        //IsDoorOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInTrigger = true;
        player = other.transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInTrigger = false;
        player = null;
    }



    private void Update()
    {
        if (!playerInTrigger || player == null) return;

        if (Input.GetKeyDown(interactKey) && IsDoorOpen == true)
        {
            // Save progress
            PlayerPrefs.SetInt("ItemsCollected", SpawnEnemy.ItemsCollected);
            PlayerPrefs.SetInt("CurrentWave", SpawnEnemy.currentWave);
            PlayerPrefs.Save();

            // Save where we came from
            PlayerPrefs.SetString(ReturnSceneKey, SceneManager.GetActiveScene().name);


            //force write immediately
            PlayerPrefs.Save();

            // Load interior
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}