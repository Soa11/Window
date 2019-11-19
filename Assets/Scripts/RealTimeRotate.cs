using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeRotate : MonoBehaviour
{
    const float secondsInDay = 86400;
    const float quarter = 1f / 4f;
    const float hour = secondsInDay / 24f;


    float secondsSinceMidnight;
    float dayProgress; //How far are we through the day in terms of 0-1 (percentage)

    [Space(10)]
    public GameObject obj;
    public AnimationClip anim;
    Animation FadesAnim;


    [Space(10)]
    public bool showDebugValues;

    Quaternion originalRotation;
    float lastFrameXRotation = 0; // temp variable for calculating how much to move each frame

    bool hasPlayedAnimation = false;

    int nextTriggerHour = 0;
    float currentHour = 0;

    void Start()
    {
        //========The default rotation in the ~~Unity Scene~~ should be set to where Midnight is! =============
        originalRotation = transform.rotation; //Save the default rotation
        FadesAnim = obj.GetComponent<Animation>();

        //Time Updates
        secondsSinceMidnight = (float)System.DateTime.Now.TimeOfDay.TotalSeconds;
        dayProgress = secondsSinceMidnight / secondsInDay; //get day progress (t)
        currentHour = secondsSinceMidnight / hour;
        //=============

        nextTriggerHour = Mathf.CeilToInt(currentHour); // figure out what the next hour is going to be
    }


    void Update()
    {
        //Time Updates
        secondsSinceMidnight = (float)System.DateTime.Now.TimeOfDay.TotalSeconds;
        dayProgress = secondsSinceMidnight / secondsInDay; //get day progress (t)
        currentHour = secondsSinceMidnight / hour;
        //=============

        //Check if the animation should trigger
        if (currentHour >= nextTriggerHour) //if the current hour is greater than the trigger, do stuff and increase the trigger
        {
            Debug.Log("animation triggered at " + nextTriggerHour);
            FadesAnim.Play();

            nextTriggerHour++;

            //cleanup for midnight
            if(nextTriggerHour == 24)
            {
                nextTriggerHour = 0;
            }
        }
        //=================================
        
        //End of day cleanup stuff, resetting timers to 0 and letting animation trigger again
        if (dayProgress > 1)
        {
            dayProgress -= 1;
            hasPlayedAnimation = false;
        }
        //=================================================

        float xRotation = 360 * dayProgress;        //The rotation we want to reach
        float thisFrameX = xRotation - lastFrameXRotation;        //difference between last frame rotation and this new one
        lastFrameXRotation = xRotation;        //save the new one for use next frame

        transform.rotation = transform.rotation * Quaternion.AngleAxis(thisFrameX, Vector3.right); //Apply the rotation

        if (showDebugValues)
        {
            Debug.Log("Day Progress: " + dayProgress + " / " + "Current Hour: " + currentHour);
            Debug.Log("Next Trigger Hour: " + nextTriggerHour);
        }
    }
}
