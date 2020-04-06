using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

public class XRDeviceManager : MonoBehaviour
{
    #region singleton
    private static XRDeviceManager _instance;

    public static XRDeviceManager Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("XRDeviceManager");
                go.AddComponent<XRDeviceManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion

    private List<InputDevice> AllXRDevices = new List<InputDevice>();
    private List<InputDevice> xrControllers = new List<InputDevice>();

    [SerializeField] UIMover uiMover;

    public bool rightGrip = false;
    public bool leftGrip = false;

    public Vector2 leftThumb = Vector2.zero;
    public Vector2 rightThumb = Vector2.zero;


    #region Events
    public delegate void UpdateLeftThumbAxis(Vector2 leftAxisEvent);
    public static event UpdateLeftThumbAxis leftThumbAxisEvent;

    public delegate void UpdateRightThumbAxis(Vector2 rightAxisEvent);
    public static event UpdateRightThumbAxis rightThumbAxisEvent;

    public delegate void UpdateLeftGrip(bool leftGrip);
    public static event UpdateLeftGrip leftGripEvent;

    public delegate void UpdateRightGrip(bool rightGrip);
    public static event UpdateRightGrip rightGripEvent;


    #endregion

    Vector2 rightAxis = Vector2.zero;
    Vector2 leftAxis = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        SetupDevices();
        TrackControllerInputs();
    }

   

    // Update is called once per frame
    void Update()
    {
      TrackControllerInputs();
        SetupDevices();
    }

    private void SetupDevices()
    {

        // This section grabs all the input devices and determines the maufacturer
        InputDevices.GetDevices(AllXRDevices);   
             

        // This section grabs the controllers and puts them in a list
   InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.HeldInHand, xrControllers);



        #region Debug Log 

      //  Debug.Log(AllXRDevices.Count);
     //   Debug.Log(AllXRDevices[0].manufacturer);

        foreach (InputDevice device in xrControllers)
        {
       //     Debug.Log(device.name + device.characteristics.ToString());
        }

        foreach (InputDevice device in AllXRDevices)
        {
      //      Debug.Log(device.name + device.characteristics.ToString());

        }

        #endregion
    }

    // This works 
    private void TrackControllerInputs()
    {
      //   Debug.Log(xrControllers.Count);

        if (xrControllers.Count >= 1)
        {


            // Thumbstick AXIS
            if (xrControllers[1].TryGetFeatureValue(CommonUsages.primary2DAxis, out rightAxis))
            {

                if (rightAxis != Vector2.zero)
                {



                  Debug.Log(rightAxis);
                rightThumbAxisEvent(rightAxis);

               //    uiMover.MoveCanvas(rightAxis);


                }


            }

            if (xrControllers[0].TryGetFeatureValue(CommonUsages.primary2DAxis, out leftAxis))
            {
               
                if (leftAxis != Vector2.zero)
                {

                    

                    leftThumbAxisEvent(leftAxis);


                }

            }
            // right
           
    

            // Grip
            // left
            
            if (xrControllers[0].TryGetFeatureValue(CommonUsages.gripButton,   out leftGrip))
            {
                

            }
            // right
           
            if (xrControllers[1].TryGetFeatureValue(CommonUsages.gripButton, out rightGrip))
            {
                

            }

           
        }
       
    }










}
