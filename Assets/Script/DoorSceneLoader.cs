using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorSceneLoader : MonoBehaviour
{

    [SerializeField] private string sceneToLoad = "HouseInterior";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    
    private const string ReturnSceneKey = "ReturnScene";
    private const string ReturnXKey = "ReturnX";
    private const string ReturnYKey = "ReturnY";
    private const string ReturnZKey = "ReturnZ";
    private const string ReturnRotYKey = "ReturnRotY";

    private bool playerInTrigger;
    private Transform player;

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

        if (Input.GetKeyDown(interactKey))
        {
            // Save where we came from
            PlayerPrefs.SetString(ReturnSceneKey, SceneManager.GetActiveScene().name);
            PlayerPrefs.SetFloat(ReturnXKey, player.position.x);
            PlayerPrefs.SetFloat(ReturnYKey, player.position.y);
            PlayerPrefs.SetFloat(ReturnZKey, player.position.z);
            PlayerPrefs.SetFloat(ReturnRotYKey, player.eulerAngles.y);

            //force write immediately
            PlayerPrefs.Save();

            // Load interior
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}