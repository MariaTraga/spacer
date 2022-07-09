using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] MenuMovement spacecraftMovement;

    bool isTransitioning = false;

    public void ExitGame()
    {
        if (isTransitioning) { return; }
        Debug.Log("You've quit the game.");
        Application.Quit();
    }

    public void PlayGame()
    {
        if (isTransitioning) { return; }
        Debug.Log("You've started the game.");
        StartCoroutine(LaunchSpacecraft());
        StartCoroutine(LoadNextLevel());
    }

    private IEnumerator LoadNextLevel()
    {
        isTransitioning = true;
        yield return new WaitForSeconds(1.5f);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator LaunchSpacecraft()
    {
        while (true)
        {
            spacecraftMovement.StartThrusting();
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
