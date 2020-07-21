using UnityEngine;

public class AttachAnchor : MonoBehaviour
{

    /// <summary>
    /// Class used to move UI canvas object around the envrionment. Thi scan be down by picking up and dragging
    /// and by moving forward and backwards in a fixed line emmitted from the controller. 
    /// 
    /// TODO:
    /// 
    /// </summary>


    // Used to give visual feedback on when raycast hits interactable object
    [SerializeField] GameObject reticle;   

    // Fixed points on line canvas moves between
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
   
    [SerializeField] GameObject canvas;
    [SerializeField] Transform controller;

    [HideInInspector]
    public bool canvasGripped;
    bool isGrabbed;

    // Start is called before the first frame update
    void OnEnable()
    {
        DeviceManager.rightThumbAxisEvent += ChangePosition;
        DeviceManager.leftThumbAxisEvent += ChangePosition;
    }

    private void OnDisable()
    {
        DeviceManager.rightThumbAxisEvent -= ChangePosition;
        DeviceManager.leftThumbAxisEvent -= ChangePosition;
    }

    // Invoked by two unity public events. 
    public void IsGrabbed()
    {
        if (reticle.activeInHierarchy)
        {
            isGrabbed = true;
        }
    }
    public void IsReleased()
    {
        isGrabbed = false;
    }

    private void DropUI()
    {
        canvas.transform.parent = null;
    }


    // Moves anchor point (and canvas) forwards (4m) and backwards (0.2m) between two points attached to the controller based on 2D axis input events.
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
        UpdateCubePosition();
        MoveUI();
    }
  

    // Locks anchor point to reticle position
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

    // Grabs canvas and adds it as a child to the anchor point transform when grabbed.
    public void MoveUI()
    {
        if (reticle.activeInHierarchy && isGrabbed)
        {
            canvas.transform.SetParent(transform);
            canvasGripped = true;
        }
        else if (reticle.activeInHierarchy && !isGrabbed)
        {
            canvas.transform.SetParent(null);
        }
    }
}
