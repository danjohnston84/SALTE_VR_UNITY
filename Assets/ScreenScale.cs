
using UnityEngine;
using UnityEngine.Rendering;

public class ScreenScale : MonoBehaviour
{   
    
     /// <summary>
     /// Used to scale the UI screen based on user input from controller 2D axis (x). Also rotates the UI to always face the center of environment.
     /// </summary>

    [SerializeField] Transform target; // target for UI rotation
    [SerializeField] AttachAnchor[] anchors; // Used to detect if controller has grabbed UI.
    Transform canvasAnchor;


    private float rotateSpeed = 10f;
    float scale = 0;
    float h = 0;

    private void Start()
    {
        canvasAnchor = GameObject.Find("CanvasAnchor").transform;
    }

    private void OnEnable()
    {
        DeviceManager.leftThumbAxisEvent += ScaleCanvas;
        DeviceManager.rightThumbAxisEvent += ScaleCanvas;
    }

    private void OnDisable()
    {
        DeviceManager.leftThumbAxisEvent -= ScaleCanvas;
        DeviceManager.rightThumbAxisEvent -= ScaleCanvas;
    }

    private void Update()
    {
       // Rotate UI to face center to environment 
       LookAt();
    }

    // Scale canvas based on 2D Axis Input Event
    private void ScaleCanvas(Vector2 axis)
    {
        if (anchors[0].canvasGripped || anchors[1].canvasGripped)
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

            // clamp 2D axis input to eliminate noise of artefacts
            scale = Mathf.Clamp(scale, -1, 1);

            // Min and max values for the size of UI
            float tmpScale = ScaleValue(-1f, 1f, 0.2f, 1.7f, scale); 

            Vector3 newCanvasScale = new Vector3(tmpScale, tmpScale, 0.001f);              
            transform.localScale = newCanvasScale;
        }
        else
            return;
    }
    
    // Scale values to new min/max
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
        Vector3 targetDirection = target.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotateSpeed * Time.deltaTime;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }


}
