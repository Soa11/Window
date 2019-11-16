using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLerpMultiStep : MonoBehaviour
{
    float timer;
    public float timerMax;
    float t;

    public Vector3[] rotations;

    private const float third = (1f / 3f);
    private const float quarter = (1f / 4f);

    private float interval;

    private void Start()
    {
        interval = 1f / rotations.Length;
        Debug.Log(interval);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timerMax)
        {
            timer -= timerMax;
        }
        t = timer / timerMax;


        Quaternion newRotation = new Quaternion();
        /*
        int sector = 1;//should always be overwritten

        for (int i = 1; i == rotations.Length; i++)
        {
            if (t < (float)i * interval)
            {
                //great
            }
            else
            {
                sector = i;
            }
        }


        Debug.Log(sector);

        float tSector = Mathf.InverseLerp((float)(sector - 1), interval * (float)sector, t);

        if (sector < rotations.Length)
        {
            newRotation = Quaternion.Slerp(Quaternion.Euler(rotations[sector - 1]), Quaternion.Euler(rotations[sector]), tSector);

        }
        else
        {
            newRotation = Quaternion.Slerp(Quaternion.Euler(rotations[sector]), Quaternion.Euler(rotations[0]), tSector);

        }
        */


        //The Brute Force Way
        
        if (t < quarter)
        {
            //do a-b
            float tA = Mathf.InverseLerp(0, quarter, t);
            newRotation = Quaternion.Slerp(Quaternion.Euler( rotations[0]), Quaternion.Euler(rotations[1]), tA);
        }
        else if (t < 2 * quarter)
        {
            //do b-c
            float tB = Mathf.InverseLerp(quarter, 2* quarter, t);
            newRotation = Quaternion.Slerp(Quaternion.Euler(rotations[1]), Quaternion.Euler(rotations[2]), tB);

        }
        else if (t < 3 * quarter)
        {
            //do b-c
            float tC = Mathf.InverseLerp(2*quarter, 3 * quarter, t);
            newRotation = Quaternion.Slerp(Quaternion.Euler(rotations[2]), Quaternion.Euler(rotations[3]), tC);

        }
        else if (t < 1f)
        {
            //do d-a
            float tD = Mathf.InverseLerp(3* quarter, 1f, t);
            newRotation = Quaternion.Slerp(Quaternion.Euler(rotations[3]), Quaternion.Euler(rotations[0]), tD);

        }
        transform.rotation = newRotation;
    }
}
