using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControler : MonoBehaviour
{
    public static LevelControler instance = null;
    int sceneIndex, levelPassed;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        sceneIndex = SceneManager.GetActiveScene().buildIndex;
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
    }

    public void levelCompleted()
    {
        if (sceneIndex == 12)
            Invoke("loadMainMenu", 1f);
        else
        {
            if (levelPassed < sceneIndex)
                PlayerPrefs.SetInt("LevelPassed", sceneIndex);
            Invoke("loadNextLevel", 1f);
        }
    }

    void loadNextLevel()
    {
        SceneManager.LoadScene(sceneIndex + 1);
    }

    void loadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
