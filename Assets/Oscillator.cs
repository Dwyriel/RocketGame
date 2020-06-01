using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movVector = new Vector3(0f, 0f, 0f);
    Vector3 startingPos;
    [Range(0, 1)] [SerializeField] float movFactor; // 0 to 1
    [SerializeField]float period = 3f;
    float halfMultiMF = .5f;

    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float cycles;
        if (period <= Mathf.Epsilon)
        {
            halfMultiMF = 0;
            cycles = 0f;
        }
        else
            cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float rawSinWave = Mathf.Sin(cycles * tau);
        movFactor = (rawSinWave / 2f) + halfMultiMF;
        Vector3 offset = movVector * movFactor;
        transform.position = startingPos + offset;
    }
}
