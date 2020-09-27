using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public const int sceneStart = 0;
    public const int sceneLevel01 = 1;
    public const int sceneLevel02 = 2;
    public const int sceneLevel03 = 3;
    public const int sceneEnd = 4;
    public const int sceneArena = 5;

    public void OnStartArcade()
    {
        SceneManager.LoadScene(sceneLevel01, LoadSceneMode.Single);
    }

    public void OnStartCombat()
    {
        SceneManager.LoadScene(sceneArena, LoadSceneMode.Single);
    }

    public int CurrentScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(CurrentScene(), LoadSceneMode.Single);
    }

    public void NextLevel()
    {
        WorldCreator.finishZoneLocation = Vector3.zero;
        GameObject.Find("BootstrapContainer").GetComponent<WorldCreator>().ClearLevel();
        SceneManager.LoadScene(CurrentScene() + 1, LoadSceneMode.Single);
    }

    public void MainMenu()
    {   
        GameObject.Find("GameManager").GetComponent<GameManager>().ResetValues();
        GameObject.Find("BootstrapContainer").GetComponent<WorldCreator>().ClearLevel();
        SceneManager.LoadScene(sceneStart, LoadSceneMode.Single);
    }

    public int GetLastLevelIndex()
    {
        return sceneEnd - 1;
    }

    public void EndScene()
    {
        WorldCreator.finishZoneLocation = Vector3.zero;
        GameObject.Find("BootstrapContainer").GetComponent<WorldCreator>().ClearLevel();
        SceneManager.LoadScene(sceneEnd, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}