using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseMenu : MonoBehaviour
{
    public GameObject menuObj;
    public bool isPaused;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isPaused)
            {
                isPaused = false;
                menuObj.SetActive(false);
                setSceneTimeScale(1.0f);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                isPaused = true;
                menuObj.SetActive(true);
                setSceneTimeScale(0);
                Cursor.lockState = CursorLockMode.None;

            }
        }
    }

    public void setSceneTimeScale(float timeScaleToSet)
    {
        Time.timeScale = timeScaleToSet;
    }
}
