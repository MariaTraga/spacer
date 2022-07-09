using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMovement : MonoBehaviour
{
    // PARAMETERS
    [SerializeField] float thrusterSpeed = 750f;
    [SerializeField] AudioClip mainEgine;

    [SerializeField] ParticleSystem mainBoosterParticles;

    // CACHE
    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrusterSpeed * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEgine);
        }

        if (!mainBoosterParticles.isPlaying)
        {
            mainBoosterParticles.Play();
        }
    }
}
