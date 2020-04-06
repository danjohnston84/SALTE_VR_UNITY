using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachAnchor : MonoBehaviour
{
    public bool moveable = false;

    [SerializeField] GameObject reticle;

    public float distance;

    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
   

    [SerializeField] XRDeviceManager deviceManager;
    [SerializeField] GameObject canvas;
    [SerializeField] Transform controller;


    public bool leftController;
    public bool rightController;
    public Vector2 leftDebug;
    public Vector2 rightDebug;


    public bool canvasGripped;

   public bool inRange;


    bool isGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        XRDeviceManager.rightThumbAxisEvent += ChangePosition;
        XRDeviceManager.leftThumbAxisEvent += ChangePosition;
    }

    private void OnDestroy()
    {
        XRDeviceManager.rightThumbAxisEvent -= ChangePosition;
        XRDeviceManager.leftThumbAxisEvent -= ChangePosition;
    }

    public void IsGrabbed()
    {
        isGrabbed = true;
    }

    public void IsReleased()
    {
        isGrabbed = false;
    }


    private void DistancClamp()
    {

        float speed = 10f;

        if (distance < 0.21f)
        {
            Vector3 clampedPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.05f);
            this.transform.position = clampedPosition;
        }
        else if (distance > 3.9f)
        {
            Vector3 clampedPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.05f);
            this.transform.position = clampedPosition;
        }
        else
            return;
        
    }

    private void ChangePosition(Vector2 axis)
    {
        if (isGrabbed)
        {

            if (axis.y > 0)
            {

                float step = axis.y * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, endPoint.position, step);

                if (Vector3.Distance(transform.position, endPoint.position) < 0.001f)
                {
                    return;
                }

            }
            else if (axis.y < 0)
            {
                float step = axis.y * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, startPoint.position, step * -1f);

                if (Vector3.Distance(transform.position, startPoint.position) < 0.001f)
                {
                    return;
                }
            }
        }
    }

 //   private void ChangePosition(Vector2 axis)
  //  {
        
//
   //     if (clampedDistance >= 0.21 && clampedDistance <= 3.9)
   //     {
  //          inRange = true;

    //        if (inRange)
    //        {

    //            if (axis.y > 0)
    //            {
     //               float speed = axis.y;


     //               if (clampedDistance <= 3.9)
     //                   transform.Translate(Vector3.forward * (Time.deltaTime * speed));
      //              else
                 //   {
                      //  Vector3 clampedPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 0.05f);
                       // this.transform.position = clampedPosition;
                  //  }
                        


   //             }
   //             if (axis.y < 0)
  //              {
   //                 float speed = axis.y * -1f;

//
   //                 if (clampedDistance >= 0.21)
  //                      transform.Translate(Vector3.back * (Time.deltaTime * speed));
   //                 else
  //                  {
                     //  Vector3 clampedPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.05f);
                     //   this.transform.position = clampedPosition;
   //                 }
   //
   //             }
    //        }
 //       }
//        else
 //       {
          //  DistancClamp();
//        }

//    }

    // Update is called once per frame
    void Update()
    {


        distance = Vector3.Distance(controller.position, canvas.transform.position);
        clampedDistance = Mathf.Clamp(distance, 0.2f, 4f);
        clampedDistance = (float)System.Math.Round(clampedDistance, 2);


        UpdateCubePosition();
       

        RightController();
        LeftController();



    }
   public float clampedDistance = 0;

    private void UpdateCubePosition()
    {



      



            if (reticle.activeInHierarchy && !isGrabbed)
            {
                this.transform.position = reticle.transform.position;
            }
        
        else
        {
            return;
        }

        
   }

    private void RightController()
    {
        if (rightController)
        {
            // Move the canvas
          bool  moveableRight = isGrabbed;

            if (reticle.activeInHierarchy && moveableRight)
            {
                canvas.transform.position = this.transform.position;
                canvasGripped = true;
            }
            else
            {
                return;
            }
        }
    }
    private void LeftController()
    {
        if (leftController)
        {
            // Move the Canvas
           bool moveableLeft = isGrabbed;

           
             if (reticle.activeInHierarchy && moveableLeft)
            {
             canvas.transform.position = this.transform.position;
                canvasGripped = true;

            }
            else
            {
                return;
            }
        }
    }






 


}
