using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTrigger : MonoBehaviour
{
    public Room valueRoom;
    public string sceneToLoad;

    private bool hasPlayerEntered = false;

    private void Update()
    {
        if (!hasPlayerEntered)
        {
            // Find the RoomNavigation script in the scene
            RoomNavigation roomNavigation = FindObjectOfType<RoomNavigation>();

            if (roomNavigation != null && roomNavigation.currentRoom != null && roomNavigation.currentRoom == valueRoom)
            {
                hasPlayerEntered = true;
                LoadScene();
            }
        }
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("No scene to load specified.");
        }
    }
}
