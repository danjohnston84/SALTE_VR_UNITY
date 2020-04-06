using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSkyBox : MonoBehaviour
{
  
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



    // Update is called once per frame
    void Update()
    {
        
    }
}
