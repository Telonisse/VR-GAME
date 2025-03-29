using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseDetectionTrgger : MonoBehaviour
{

    bool thumbUpPose = false;
    CastFireBall castfireball;

   public void thumbup()
    {
        Debug.LogError("it worked?");
        this.thumbUpPose = true;
    }

   public void fingersAllOpen()
    {
        if (thumbUpPose == true)
        {
            Debug.LogError("it worked? agaui ");

            castfireball.TriggerFireSpell();
            thumbUpPose = false;
        }
        else
        {
            return;
        }
    }
}
