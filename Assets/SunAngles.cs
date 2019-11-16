using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAngles : MonoBehaviour
{
    
    public Vector3[] Rotations;

    enum NextRotation { A, B, C }
    NextRotation nextRotation = NextRotation.A;

    public float timerMax = 60f;
    float timerMid;
    float timer;

    const float third = 1 / 3;

    private void Start()
    {
        timerMid = timerMax / 2;
    }

    private void Update()
    {
        timer += Time.deltaTime;


        float t = timer / timerMax;

        Quaternion newRotation = transform.rotation;

        
        
        
        if (t <= 1/3)
        {
            //do a-b
            float tA = t / third;
            newRotation = Quaternion.Lerp(Quaternion.Euler(Rotations[0]), Quaternion.Euler(Rotations[1]), tA);
        }
        else if (t <= 2 / 3)
        {
            //do b-c
            float tB = t- third / third;
            newRotation = Quaternion.Lerp(Quaternion.Euler(Rotations[1]), Quaternion.Euler(Rotations[2]), tB);
        }
        else
        {
            //do c-a
        }


        transform.rotation = newRotation;
        
    }
}