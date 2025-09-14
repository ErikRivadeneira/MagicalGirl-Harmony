using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFlowManager : MonoBehaviour
{
    public static event Action OnLevelChange;
    /*public static UIFlowManager Instance { get; private set; }
    
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }*/

    public void NextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if(currentScene + 1< SceneManager.sceneCountInBuildSettings)
        {
            OnLevelChange?.Invoke();
            StartCoroutine(LoadSpecificScene(currentScene + 1, 1f));
        }
        else
        {
            Debug.Log("NoMoreScenes");
        }
    }

    public void RetryLevel()
    {
        OnLevelChange?.Invoke();
        StartCoroutine(LoadSpecificScene(SceneManager.GetActiveScene().buildIndex, 1f));
    }

    public void QuitGame()
    {
        OnLevelChange?.Invoke();
        StartCoroutine(QuitGameAfterDelay(1f));
    }

    public void MainMenu()
    {
        OnLevelChange?.Invoke();
        StartCoroutine(LoadSpecificScene(0,1f));
    }

    IEnumerator LoadSpecificScene(int sceneIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator QuitGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }
}
