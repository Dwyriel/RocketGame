using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource asMainThruster, asRCSThruster, asOthers;
    enum State { Alive, Dying, Transcending, Debug }
    [SerializeField] State pState = State.Alive;
    [SerializeField] float rcsThrust = 110f, mainThrust = 800f, musicVolume = 0.2f;
    [SerializeField] AudioClip mainEngine, rcsSound, death, win, bm;
    [SerializeField] ParticleSystem engineParticle, winParticle, deathParticle;
    Scene scene;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var AudioSources = GetComponents<AudioSource>();
        asMainThruster = AudioSources[0];
        asRCSThruster = AudioSources[1];
        asOthers = AudioSources[2];
        asOthers.clip = bm;
        asOthers.Play();
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Debug.isDebugBuild)
            DebugMode();
        asOthers.volume = musicVolume;
        if (pState == State.Alive || pState.Equals(State.Debug))
        {
            InputThrust();
            InputRotation();
        }
    }

    private void DebugMode()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextScene();
        }
        if (pState == State.Alive && Input.GetKeyDown(KeyCode.C))
            pState = State.Debug;
        else if (pState == State.Debug && Input.GetKeyDown(KeyCode.C))
            pState = State.Alive;
    }

    private void InputThrust()
    {
        float thrust = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.up * thrust);
            if (!engineParticle.isEmitting) engineParticle.Play();
            if (!asMainThruster.isPlaying) asMainThruster.PlayOneShot(mainEngine, 0.8f);
        }
        else
        {
            asMainThruster.Stop();
            engineParticle.Stop();
        }
    }

    private void InputRotation()
    {
        float rotation = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (!asRCSThruster.isPlaying) asRCSThruster.PlayOneShot(rcsSound, 1f);
            rb.maxAngularVelocity = 0f; // reduces rotation enough to be able to turn
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, 0, Space.World);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, -rotation, Space.World);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, rotation, Space.World);
        }
        else
        {
            asRCSThruster.Stop();
            rb.maxAngularVelocity = 7f;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (pState == State.Alive)
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Fuel": // TODO
                    print("got fuel");
                    break;
                case "Finish":
                    Victory();
                    break;
                default:
                    Death();
                    break;
            }
    }

    private void Victory()
    {
        DeathOrVictorySound(win);
        winParticle.Play();
        pState = State.Transcending;
        Invoke("LoadNextScene", 3f);
    }

    private void Death()
    {
        deathParticle.Play();
        DeathOrVictorySound(death);
        pState = State.Dying;
        Invoke("LoadPreviousLevel", 4f);
    }

    private void DeathOrVictorySound(AudioClip aC)
    {
        engineParticle.Stop();
        asOthers.Stop();
        asOthers.volume = 1f;
        asRCSThruster.Stop();
        asMainThruster.Stop();
        rb.maxAngularVelocity = 7f; //if player dies or wins holding A or B it resets anyway;
        asOthers.PlayOneShot(aC, 1f);
    }

    private void LoadNextScene()
    {
        if (scene.name == "Sandbox")
            SceneManager.LoadScene(scene.buildIndex);
        else if ((scene.buildIndex + 1) < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(scene.buildIndex + 1);
        else SceneManager.LoadScene(0);
    }
    //Loads new scene, so pState goes back to alive, change later if needed.
    private void LoadPreviousLevel()
    {
        if (scene.name == "Sandbox")
            SceneManager.LoadScene(scene.buildIndex);
        else if (scene.buildIndex > 0)
            SceneManager.LoadScene(scene.buildIndex - 1);
        else SceneManager.LoadScene(0);
    }
}
