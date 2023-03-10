using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManagmentFunctions : MonoBehaviour
{
    public static void loadScene(int sceneIndex)
    {
        
        if(sceneIndex > SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("There is no scene with an index of " + sceneIndex);
            Debug.LogWarning("Please make sure all scenes are loaded in the build settings before trying again");
            return;
        }
        SceneManager.LoadScene(sceneIndex);
    }

    public static void quitGame()
    {
        if (Application.isEditor) Debug.LogWarning("Should Be Quitting, but is playing in editor");
        Application.Quit();
    }

}
