using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rb;
    AudioSource aS;
    float np, nm;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        aS = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        np = 110 * Time.deltaTime;
        nm = (-1) * np;
        Thrust();
        Rotate();
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rb.AddRelativeForce(Vector3.up);
            if (!aS.isPlaying) aS.Play();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rb.AddRelativeForce(Vector3.down);
            if (!aS.isPlaying) aS.Play();
        }
        else
        {
            aS.Stop();
        }
    }

    private void Rotate()
    {
        rb.freezeRotation = true; //pauses rotation physics
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            print("Ignore A & D");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, nm, Space.World);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, np, Space.World);
        }
        rb.freezeRotation = false;
    }
}
