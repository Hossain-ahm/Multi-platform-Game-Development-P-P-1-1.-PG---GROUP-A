using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject birdObj, hudUI, pauseUI, mainMenuUI, deathScreen, tutorialObj, stamBarUI;
    bool paused;

    private void Start()
    {
        MainMenu();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
    public void PlayGame()
    {
        if (!PlayerPrefs.HasKey("TutorialPlayed"))
        {
            tutorialObj.SetActive(true);
            hudUI.SetActive(true);
            stamBarUI.SetActive(false);
        }
        else
        {
            Debug.Log("SKIPPED");
            StartCoroutine(StartPlay());
        }
    }
#if UNITY_EDITOR
    [ContextMenu("Reset Tutorial")]
    void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("TutorialPlayed");
        PlayerPrefs.Save();
    }
#endif

    IEnumerator StartPlay()
    {
        yield return new WaitForSeconds(1f);
        birdObj.SetActive(true);
        hudUI.SetActive(true);
        stamBarUI.SetActive(true);
        mainMenuUI.SetActive(false);
        yield break;
    }
    public void MainMenu()
    {
        TogglePause(false);
        birdObj.SetActive(false);
        hudUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void DeathScreen()
    {
        deathScreen.SetActive(true);
    }
    public void TogglePause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
            pauseUI.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            paused = true;
            pauseUI.SetActive(true);

        }
    }
    public void TogglePause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        pauseUI.SetActive(pause);
        paused = pause;
    }
}
