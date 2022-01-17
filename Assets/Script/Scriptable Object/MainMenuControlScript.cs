using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuControlScript : MonoBehaviour
{
    public Button lv01, lv02, lv03, lv04, lv05, lv06, lv07, lv08, lv09, lv10;
    int levelPassed;
    // Start is called before the first frame update
    void Start()
    {
        levelPassed = PlayerPrefs.GetInt("LevelPassed");
        lv01.interactable = false;
        lv02.interactable = false;
        lv03.interactable = false;
        lv04.interactable = false;
        lv05.interactable = false;
        lv06.interactable = false;
        lv07.interactable = false;
        lv08.interactable = false;
        lv09.interactable = false;
        lv10.interactable = false;

        switch (levelPassed)
        {
            case 1:
                lv01.interactable = true;
                break;
            case 2:
                lv01.interactable = true;
                lv02.interactable = true;
                lv03.interactable = true;
                lv04.interactable = true;
                lv05.interactable = true;
                lv06.interactable = true;
                lv07.interactable = true;
                lv08.interactable = true;
                lv09.interactable = true;
                lv10.interactable = true;
                break;
        }
    }

    public void levelToLoad(int level)
    {
        SceneManager.LoadScene(level);
    }

}
