using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFeet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (pState == State.Alive)
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    break;
                case "Finish":
                    Victory();
                    break;
                default:
                    Death();
                    break;
            }
    }
}
