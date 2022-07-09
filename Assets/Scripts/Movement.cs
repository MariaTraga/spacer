using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // PARAMETERS
    [SerializeField] float thrusterSpeed = 750f;
    [SerializeField] float steeringSpeed = 300f;
    [SerializeField] float fuel = 1500f;
    public bool IsFuelRestricted = true;
    [SerializeField] AudioClip mainEgine;

    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem leftSideBoosterParticles;
    [SerializeField] ParticleSystem rightSideBoosterParticles;

    // CACHE
    Rigidbody rb;
    AudioSource audioSource;
    public Action onFuelChanged;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!IsFuelRestricted)
            {
                StartThrusting();
            }
            else if(fuel > 0)
            {
                StartThrusting();
                fuel--;
                onFuelChanged?.Invoke();
            }
        }
        else
        {
            StopThrusting();
        }
    }

    void StartThrusting()
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

    void StopThrusting()
    {
        audioSource.Stop();
        mainBoosterParticles.Stop();
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void RotateLeft()
    {
        ApplyRotation(steeringSpeed);
        if (!rightSideBoosterParticles.isPlaying)
        {
            rightSideBoosterParticles.Play();
        }
    }

    void RotateRight()
    {
        ApplyRotation(-steeringSpeed);
        if (!leftSideBoosterParticles.isPlaying)
        {
            leftSideBoosterParticles.Play();
        }
    }

    void StopRotating()
    {
        rightSideBoosterParticles.Stop();
        leftSideBoosterParticles.Stop();
    }

    void ApplyRotation(float rotationThisFrame)
    {
        //rb.freezeRotation = true; // Freezing rotation before manual rotation - Conflict between Rotate and Physics engine
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        //rb.freezeRotation = false;
    }

    public float GetFuel()
    {
        return fuel;
    }
}
