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

    private Vector3 recticleOffset = Vector3.zero;

    bool isGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        DeviceManager.rightThumbAxisEvent += ChangePosition;
        DeviceManager.leftThumbAxisEvent += ChangePosition;
        DeviceManager.rightThumbAxisEvent += ScaleCanvas;
        DeviceManager.leftThumbAxisEvent += ScaleCanvas;

    }

    private void OnDestroy()
    {
        DeviceManager.rightThumbAxisEvent -= ChangePosition;
        DeviceManager.leftThumbAxisEvent -= ChangePosition;
        DeviceManager.rightThumbAxisEvent -= ScaleCanvas;
        DeviceManager.leftThumbAxisEvent -= ScaleCanvas;
    }

    public void IsGrabbed()
    {

        if (reticle.activeInHierarchy)
        {
            isGrabbed = true;
            
        }
      

    }

    private void DropUI()
    {
        canvas.transform.parent = null;
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
            if (axis.x >= 0.9f || axis.x <= -0.9f) return;
            else
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
    }

 
    void Update()
    {
      

        distance = Vector3.Distance(controller.position, canvas.transform.position);
        clampedDistance = Mathf.Clamp(distance, 0.2f, 4f);
        clampedDistance = (float)System.Math.Round(clampedDistance, 2);


        UpdateCubePosition();


        //  RightController();
        //   LeftController();
        MoveUI();


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

    private void ScaleCanvas(Vector2 axis)
    {
        float h = 0;
        float scale = 0;

        if (isGrabbed)
        {

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

    public void MoveUI()
    {
      


        if (reticle.activeInHierarchy && isGrabbed)
        {


            // canvas.transform.position = this.transform.position + recticleOffset;
            //  canvas.transform.rotation = this.transform.rotation;
            canvas.transform.SetParent(transform);
            canvasGripped = true;
        }
       else  if(reticle.activeInHierarchy && !isGrabbed)
        {

        canvas.transform.SetParent(null);
           
        }
    }




  






}
