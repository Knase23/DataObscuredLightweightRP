using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralGameManager : MonoBehaviour
{
    public Canvas loseCanvas;
    public Canvas exitCanvas;


    // Start is called before the first frame update
    void Start()
    {
        PlayerCharacter.OnPlayerDeath += PlayerCharacter_OnPlayerDeath;
        loseCanvas.gameObject.SetActive(false);
        exitCanvas.gameObject.SetActive(false);
    }

    private void PlayerCharacter_OnPlayerDeath()
    {
        Time.timeScale = 0;
        FirstPersonCamera.UnlockCursor();
        loseCanvas.gameObject.SetActive(true);
    }
    public void Retry()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGamePrompt()
    {
        Time.timeScale = 0;
        loseCanvas.gameObject.SetActive(false);
        exitCanvas.gameObject.SetActive(true);
    }
    public void ExitNo()
    {
        loseCanvas.gameObject.SetActive(true);
        exitCanvas.gameObject.SetActive(false);
    }
    public void ExitGame()
    {
        Application.Quit();
    }



}
