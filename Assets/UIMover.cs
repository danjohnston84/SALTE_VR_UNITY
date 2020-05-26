using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices.WindowsRuntime;

public class UIMover : MonoBehaviour
{

 
    [SerializeField] Transform target;
    [SerializeField] Transform centerPoint;
    [SerializeField] AttachAnchor[] anchors;
   

    public CharacterController controller;

    Transform canvasAnchor;


  

    public float angle;

    
    private float rotateSpeed = 2f;
    private float moveSpeed = 1f;

    public float distance;

    float scale = 0;

    public bool followEyes { get; set; } = true;


    Vector3 newScale;

    private void Start()
    {
        this.canvasAnchor = GameObject.Find("CanvasAnchor").transform;

   
     //   DeviceManager.leftThumbAxisEvent += ScaleCanvas;
     //   DeviceManager.rightThumbAxisEvent += ScaleCanvas;

        newScale = transform.localScale;

    }

    private void OnDestroy()
    {
  
     //   DeviceManager.leftThumbAxisEvent -= ScaleCanvas;
      //  DeviceManager.rightThumbAxisEvent -= ScaleCanvas;
    }

    private void Update()
    {

        this.distance = Vector3.Distance(centerPoint.transform.position, this.transform.position);
       // LookAt();

        angle = AngleDifference();

    }

    private float AngleDifference()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir = target.transform.InverseTransformDirection(dir);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle;
    }

    
 
    private void ScaleCanvas(Vector2 axis)
    {
        float h = 0;

        if (anchors[0].canvasGripped || anchors[1].canvasGripped)
        {
            if (axis.y >= 0.9 || axis.y <= -0.9) return;
            else
            {
                this.transform.SetParent(null);
                if (axis.x > 0)
                {
                    h = axis.x * (axis.x * Time.deltaTime);
                }
                else if (axis.x < 0)
                {
                    h = axis.x * ((axis.x * -1) * Time.deltaTime);
                }

                scale += h;
                scale = Mathf.Clamp(scale, -1, 1);
                float tmpScale = ScaleValue(-1f, 1f, 0.2f, 1.7f, scale);

                Vector3 newCanvasScale = new Vector3(tmpScale, tmpScale, 0.001f);


                Debug.Log(newCanvasScale);

                this.transform.localScale = newCanvasScale;
            }
            
        }
        else
            return;


    } 



    private float ScaleValue(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    private void LookAt()
    {
          // Determine which direction to rotate towards
        Vector3 targetDirection = this.target.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = this.rotateSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);           

        // Calculate a rotation a step closer to the target and applies rotation to this object
        this.transform.rotation = Quaternion.LookRotation(newDirection);
    }


}
