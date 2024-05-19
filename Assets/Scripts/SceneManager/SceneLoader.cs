using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    private string currentScene;
    private string previousScene;
    private string nextScene;
    private bool firstChange = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
        currentScene = SceneManager.GetActiveScene().name;
    }

    public void ChangeScene()
    {
        if (!firstChange)
        {
            firstChange = true;
            previousScene = currentScene;
            nextScene = "HotelRoomFord";
            SceneManager.LoadScene(nextScene);
            return;
        }
        else
        {
            currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == previousScene)
            {
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            }
            else
            {
                SceneManager.LoadScene(previousScene, LoadSceneMode.Single);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            string identifier = playerController.name; // Use character's name as identifier
            if (SaveSystem.HasSavedPosition("Player", identifier))
            {
                playerController.transform.position = SaveSystem.LoadPosition("Player", identifier);
            }
            PlayerController.items = SaveSystem.LoadInventory("Player", identifier);
            playerController.canvasManager.UpdateCanvas(PlayerController.items);
        }

        FordController fordController = FindObjectOfType<FordController>();
        if (fordController != null)
        {
            string identifier = fordController.name; // Use character's name as identifier
                                                     // This assignment is redundant, removing it
                                                     // fordController.transform = fordController.transform;

            if (SaveSystem.HasSavedPosition("Ford", identifier))
            {
                fordController.transform.position = SaveSystem.LoadPosition("Ford", identifier);
            }
            FordController.items = SaveSystem.LoadInventory("Ford", identifier);
            fordController.canvasManager.UpdateCanvas(FordController.items);
        }
    }
}