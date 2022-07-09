using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // PARAMETERS
    [SerializeField] float levelLoadDelay = 2f;
    [SerializeField] AudioClip crashClip;
    [SerializeField] AudioClip successClip;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    // CACHE
    AudioSource audioSource;

    // STATE
    bool isTransitioning;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        isTransitioning = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning){ return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;
            case "Fuel":
                Debug.Log("Refuel");
                break;
            case "Finish":
                StartCoroutine(VictorySequence());
                break;
            default:
                StartCoroutine(CrashSequence());
                break;
        }
    }

    private IEnumerator VictorySequence()
    {
        isTransitioning = true;
        successParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(successClip);
        DisableControls();
        yield return new WaitForSeconds(levelLoadDelay);
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }

    private IEnumerator CrashSequence()
    {
        isTransitioning = true;
        crashParticles.Play();
        audioSource.Stop();
        audioSource.PlayOneShot(crashClip);
        DisableControls();
        DisassembleRocket();
        yield return new WaitForSeconds(levelLoadDelay);
        ReloadLevel();
    }

    private void DisableControls()
    {
        GetComponent<Movement>().enabled = false;
    }

    private void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void DisassembleRocket()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.AddComponent<Rigidbody>();
            foreach (Transform grandChild in child)
            {
                grandChild.gameObject.AddComponent<Rigidbody>();
            }
            child.DetachChildren();
        }
        transform.DetachChildren();
    }
}
