using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverDay : MonoBehaviour
{
    const float secondsInDay = 86400;
    float secondsSinceMidnight;
    float dayProgress; //How far are we through the day in terms of 0-1

    public bool manual = false;
    public float manualProgressSlider = 250f; // arbitrary sliding scale for manual testing

    public bool fastAuto = false;
    float fastAutoTimer = 0;
    public float fastAutoDurationSeconds = 20f;


    public float nightTimeSpeedMultiplier = 2;

    const float quarter = 1f / 4f;

    [Space(10)]
    public bool showDebugValues;
    
    Quaternion originalRotation;
    float lastFrameXRotation = 0; // temp variable for calculating how much to move each frame

    void Start()
    {
        originalRotation = transform.rotation; //Save the default rotation

        //========The default rotation in the ~~Unity Scene~~ should be set to where Midnight is! =============
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

            //float fakeDayProgress = dayProgress;

            if(dayProgress < quarter || dayProgress > 3 * quarter)
            {
                //do faster
                fastAutoTimer += Time.deltaTime * nightTimeSpeedMultiplier;
            }
            else if(dayProgress >quarter && dayProgress < 3*quarter)
            {
                //Normal speed for daytime half
                fastAutoTimer += Time.deltaTime * 1/nightTimeSpeedMultiplier;
            }

            dayProgress = fastAutoTimer / fastAutoDurationSeconds;


        }
        else //if in normal mode
        {
            dayProgress = secondsSinceMidnight / secondsInDay; //get day progress (t)
            manualProgressSlider = dayProgress * 500f; //update manual slider to be correct position
            fastAutoTimer = fastAutoDurationSeconds * dayProgress;
        }

        if(dayProgress > 1)
        {
            dayProgress -= 1;
        }

        float xRotation = 360 * dayProgress;        //The rotation we want to reach
        float thisFrameX = xRotation - lastFrameXRotation;        //difference between last frame rotation and this new one
        lastFrameXRotation = xRotation;        //save the new one for use next frame

        transform.rotation = transform.rotation * Quaternion.AngleAxis(thisFrameX, Vector3.right) ; //Apply the rotation


        if (showDebugValues)
        {
            //Debug.Log("DayProgress: " + dayProgress.ToString() + " // " + "X Axis Rotation: " + transform.rotation.eulerAngles.x.ToString());
            //Debug.Log((fastAutoDurationSeconds / nightTimeSpeedMultiplier) + (fastAutoDurationSeconds / (1 / nightTimeSpeedMultiplier)));
        }
    }
}
