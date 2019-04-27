using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralGameManager : MonoBehaviour
{
    public Canvas loseCanvas;
    public Canvas exitCanvas;
    public Canvas skillTreeCanvas;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter.OnPlayerDeath += PlayerCharacter_OnPlayerDeath;
        SetCanvases(false, false, false);
    }

    public void PlayerCharacter_OnPlayerDeath()
    {
        Time.timeScale = 0;
        FirstPersonCamera.UnlockCursor();
        SetCanvases(false, true, false);
    }

    public void Retry()
    { 
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGamePrompt()
    {
        Time.timeScale = 0;
        SetCanvases(false, false, true);
    }
    public void ExitNo()
    {
        SetCanvases(false, true, false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    private  void SetCanvases(bool skillTree, bool lose, bool exit)
    {
        if (skillTreeCanvas)
            skillTreeCanvas.gameObject.SetActive(skillTree);
        if (loseCanvas)
            loseCanvas.gameObject.SetActive(lose);
        if (exitCanvas)
            exitCanvas.gameObject.SetActive(exit);
    }

}
