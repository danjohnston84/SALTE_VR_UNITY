using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.XR;





public class DeviceManager : MonoBehaviour
{
     public GameObject LeftAnchor;
    public GameObject RightAnchor;
 
    private ControllerManager _leftController;
    private ControllerManager _rightController;
 
    private UnityEngine.XR.InputDevice _leftDevice;
    private UnityEngine.XR.InputDevice _rightDevice;
 
    // Start is called before the first frame update
    void Start()
    {
       
 
        SetDevices();
 
        //Initialize Hands
        _leftController = LeftAnchor.GetComponent<ControllerManager>();
        _rightController = RightAnchor.GetComponent<ControllerManager>();
 
    }
 
    // Update is called once per frame
    private  void Update()
    {
        
 
        //Set Tracked Devices
      //  SetDevicePosAndRot(XRNode.LeftHand, LeftAnchor);
   //    SetDevicePosAndRot(XRNode.RightHand, RightAnchor);
 
        //Set Buttons
        UpdateButtonState(_leftDevice, CommonUsages.gripButton, _leftController.GripEvent);
        UpdateButtonState(_rightDevice, CommonUsages.gripButton, _rightController.GripEvent);
 
        UpdateButtonState(_leftDevice, CommonUsages.primary2DAxisClick, _leftController.ClickEvent);
        UpdateButtonState(_rightDevice, CommonUsages.primary2DAxisClick, _rightController.ClickEvent);
 
        UpdateButtonState(_leftDevice, CommonUsages.triggerButton, _leftController.TriggerEvent);
        UpdateButtonState(_rightDevice, CommonUsages.triggerButton, _rightController.TriggerEvent);
 
        UpdateButtonState(_leftDevice, CommonUsages.menuButton, _leftController.MenuEvent);
        UpdateButtonState(_rightDevice, CommonUsages.menuButton, _rightController.MenuEvent);
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
            //Debug.Log($"Device name '{device.name}' with role '{device.role.ToString()}'");
        }
        else if (devices.Count > 1)
        {
            Debug.Log($"Found more than one '{device.role.ToString()}'!");
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
 
  
}


[System.Serializable] // Generic Event holding button value
public class AButtonEvent : UnityEvent<bool>
{
    public bool Value { get; set; }
 
    public void Initialize(bool value, UnityAction<bool> method)
    {
        Value = value;
        AddListener(method);
    }
}