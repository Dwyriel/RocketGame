using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource aS;
    [SerializeField] float rcsThrust = 110f, mainThrust = 800f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        float thrust = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.up * thrust);
            if (!aS.isPlaying) aS.Play();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(Vector3.down * thrust);
            if (!aS.isPlaying) aS.Play();
        }
        else
        {
            aS.Stop();
        }
    }

    private void Rotate()
    {
        float rotation = rcsThrust * Time.deltaTime;
        rb.freezeRotation = true; //pauses rotation physics
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
        rb.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("Not ded");
                break;
            case "Fuel":
                print("got fuel");
                break;
            case "Finish":
                print("win");
                break;
            default:
                print("die");
                break;
        }
    }
}
