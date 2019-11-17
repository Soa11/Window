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
                FadesAnim.Play();
                //obj.GetComponent<Animation>().Play("Fades");


                /*if(dayProgress < quarter)
                {
                    //Quadrant 1, pre-morning
                    float t = dayProgress / quarter;
                    Color newColor = nightTimeFrameMaterial.color;
                    //nightTimeFrameMaterial.
                }
                else if(dayProgress > 3 * quarter)
                {
                    //Quadrant 2, post-evening

                }
                */


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
