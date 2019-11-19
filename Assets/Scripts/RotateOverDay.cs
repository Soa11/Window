﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverDay : MonoBehaviour
{
    const float secondsInDay = 86400;
    float secondsSinceMidnight;
    float dayProgress; //How far are we through the day in terms of 0-1 (percentage)

    public bool manual = false;
    public float manualProgressSlider = 250f; // arbitrary sliding scale for manual testing

    public bool fastAuto = false;
    float fastAutoTimer = 0;
    public float fastAutoDurationSeconds = 20f;
    public float nightTimeSpeedMultiplier = 2;

    [Space(10)]
    //public MeshRenderer nightTimeFrame;
    //Material nightTimeFrameMaterial;
    public GameObject obj;
    public AnimationClip anim;
    Animation FadesAnim; 

    const float quarter = 1f / 4f;

    [Space(10)]
    public bool showDebugValues;
    
    Quaternion originalRotation;
    float lastFrameXRotation = 0; // temp variable for calculating how much to move each frame


    bool hasPlayedAnimation = false;

    void Start()
    {
        //========The default rotation in the ~~Unity Scene~~ should be set to where Midnight is! =============
        originalRotation = transform.rotation; //Save the default rotation
        FadesAnim = obj.GetComponent<Animation>();

        //nightTimeFrameMaterial = nightTimeFrame.material;
    }


    void Update()
    {

     

        secondsSinceMidnight = (float)System.DateTime.Now.TimeOfDay.TotalSeconds;

        if (manual) //if in manual mode
        {
            dayProgress = manualProgressSlider / 500f; //take slider value and apply it to progress
        }

        else if (fastAuto)
        {
            if(dayProgress < quarter || dayProgress > 3 * quarter)
            {
                //do faster
                fastAutoTimer += Time.deltaTime * nightTimeSpeedMultiplier;
                //obj.GetComponent<Animation>().Play("Fades");


                if(dayProgress < quarter)
                {
                    //Quadrant 1, pre-morning
                }
                else if(dayProgress > 3 * quarter)
                {
                    //Quadrant 2, post-evening
                    if (!hasPlayedAnimation)
                    {
                        FadesAnim.Play();
                        hasPlayedAnimation = true;
                        Debug.Log("animation triggered");
                    }

                }



            }
            else if(dayProgress >quarter && dayProgress < 3*quarter)
            {
                //Normal speed for daytime half
                fastAutoTimer += Time.deltaTime * 1/nightTimeSpeedMultiplier;
            }

            dayProgress = fastAutoTimer / fastAutoDurationSeconds;


        }
        //if in realtime mode
        else
        {
            dayProgress = secondsSinceMidnight / secondsInDay; //get day progress (t)
            manualProgressSlider = dayProgress * 500f; //update manual slider to be correct position
            fastAutoTimer = fastAutoDurationSeconds * dayProgress;
        }

        //End of day cleanup stuff, resetting timers to 0 and letting animation trigger again
        if(dayProgress > 1)
        {
            dayProgress -= 1;
        }
        if(fastAutoTimer > fastAutoDurationSeconds)
        {
            fastAutoTimer -= fastAutoDurationSeconds;
            hasPlayedAnimation = false;
        }
        //=================================================


        float xRotation = 360 * dayProgress;        //The rotation we want to reach
        float thisFrameX = xRotation - lastFrameXRotation;        //difference between last frame rotation and this new one
        lastFrameXRotation = xRotation;        //save the new one for use next frame

        transform.rotation = transform.rotation * Quaternion.AngleAxis(thisFrameX, Vector3.right) ; //Apply the rotation


        if (showDebugValues)
        {
            Debug.Log(dayProgress);

        }
    }
}
