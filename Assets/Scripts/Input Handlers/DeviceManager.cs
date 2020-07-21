using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;
 using UnityEngine.Serialization;
using UnityEngine.XR;

[System.Serializable]
public class AButtonEvent : UnityEvent<bool>
{
    public bool Value { get; set; }

    public void Initialize(bool value, UnityAction<bool> method)
    {
        Value = value;
        AddListener(method);
    }
}


public class DeviceManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>



    #region singleton
    private static DeviceManager _instance;

    public static DeviceManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("DeviceManager");
                go.AddComponent<DeviceManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion


    public GameObject LeftAnchor;
    public GameObject RightAnchor;

    private ControllerManager _leftController;
    private ControllerManager _rightController;

    private UnityEngine.XR.InputDevice _leftDevice;
    private UnityEngine.XR.InputDevice _rightDevice;

    public delegate void UpdateThumbAxis(Vector2 leftAxisEvent);
    public static event UpdateThumbAxis leftThumbAxisEvent;    
    public static event UpdateThumbAxis rightThumbAxisEvent;

    // Start is called before the first frame update
    void Start()
    {
      
        SetDevices();

        //Initialize Hands
        _leftController = LeftAnchor.GetComponent<ControllerManager>();
        _rightController = RightAnchor.GetComponent<ControllerManager>();

        _leftController.handIndex = 0;
        _rightController.handIndex = 1;

    }

    // Update is called once per frame
    private  void Update()
    {
      

        //Set Tracked Devices
        SetDevicePosAndRot(XRNode.LeftHand, LeftAnchor);
        SetDevicePosAndRot(XRNode.RightHand, RightAnchor);

        //Set Buttons
        UpdateButtonState(_leftDevice, CommonUsages.gripButton, _leftController.gripEvent);
        UpdateButtonState(_rightDevice, CommonUsages.gripButton, _rightController.gripEvent);

        UpdateButtonState(_leftDevice, CommonUsages.primary2DAxisClick, _leftController.primaryAxisEvent);
        UpdateButtonState(_rightDevice, CommonUsages.primary2DAxisClick, _rightController.primaryAxisEvent);

        UpdateButtonState(_leftDevice, CommonUsages.triggerButton, _leftController.triggerEvent);
        UpdateButtonState(_rightDevice, CommonUsages.triggerButton, _rightController.triggerEvent);

        UpdateButtonState(_leftDevice, CommonUsages.primaryButton, _leftController.primaryButtonEvent);
        UpdateButtonState(_rightDevice, CommonUsages.primaryButton, _rightController.primaryButtonEvent);

        UpdateAxisState(_leftDevice, 1 );
        UpdateAxisState(_rightDevice, 0);
    }


    private void UpdateAxisState(InputDevice device, int hand  ){

        Vector2 axis = Vector2.zero;

        if(device.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis))
        {
            if(axis != Vector2.zero)
            {
                if(hand == 1)
                {
                    leftThumbAxisEvent(axis);
                  
                }
                else
                {
                    rightThumbAxisEvent(axis);
                   
                }
            }
        }
    }
           

    private static void SetDevicePosAndRot(XRNode trackedDevice, GameObject anchor)
    {
        anchor.transform.localPosition = UnityEngine.XR.InputTracking.GetLocalPosition(trackedDevice);
        anchor.transform.localRotation = UnityEngine.XR.InputTracking.GetLocalRotation(trackedDevice);
    }

    private static InputDevice GetCurrentDevice(XRNode node)
    {
        var device = new InputDevice();
        var devices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(node,
            devices);
        if (devices.Count == 1)
        {
            device = devices[0];
            Debug.Log($"Device name '{device.name}' with role '{device.role.ToString()}'");
        }
        else if (devices.Count > 1)
        {
            Debug.Log($"Found more than one '{device.characteristics.ToString()}'!");
            device = devices[0];
            
        }

        return device;
    }

    private void UpdateButtonState(InputDevice device, InputFeatureUsage<bool> button,
        AButtonEvent aButtonPressEvent)
    {
        bool tempState;
        bool invalidDeviceFound = false;
        bool buttonState = false;

        tempState = device.isValid // the device is still valid
                    && device.TryGetFeatureValue(button, out buttonState) // did get a value
                    && buttonState; // the value we got

        if (!device.isValid)
            invalidDeviceFound = true;

        if (tempState != aButtonPressEvent.Value) // Button state changed since last frame
        {
            aButtonPressEvent.Invoke(tempState);
            aButtonPressEvent.Value = tempState;
        }

        if (invalidDeviceFound) // refresh device lists
            SetDevices();
    }

    private void SetDevices()
    {
        //Set Controller Devices
        _leftDevice = GetCurrentDevice(XRNode.LeftHand);
        _rightDevice = GetCurrentDevice(XRNode.RightHand);
    }

    private void ShowCurrentlyAvailableXRDevices()
    {
        var inputDevices = new List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevices(inputDevices);
        foreach (var device in inputDevices)
        {
            Debug.Log($"Device found with name '{device.name}' and role '{device.characteristics.ToString()}'");
        }
    }
}




