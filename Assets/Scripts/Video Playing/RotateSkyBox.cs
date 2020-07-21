using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
    /// <summary>
    /// this class will be used to rotate the 360 photo to match the angles of the audio rednering engine.
    /// </summary>
  
    public float curRot = 0;

    private void Start()
    {
        RotateSky();
    }

    public void RotateSky()
    {        
        curRot %= 360;
        RenderSettings.skybox.SetFloat("_Rotation", curRot);
    }

}
