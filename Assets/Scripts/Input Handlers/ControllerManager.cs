using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>

    [HideInInspector] public bool isGripPressed;
    [HideInInspector] public bool isTriggerPressed;
    [HideInInspector] public bool isPrimaryButtonPressed;
    [HideInInspector] public bool isSecondaryButtonPressed;
    [HideInInspector] public bool isPrimaryAxisPressed;

       
    // Button events Handler
    public AButtonEvent gripEvent { get; set; }
    public AButtonEvent triggerEvent { get; set; }
    public AButtonEvent primaryButtonEvent { get; set; }
    public AButtonEvent secondaryButtonEvent { get; set; }
    public AButtonEvent primaryAxisEvent { get; set; }


    public UnityEvent GripButtonDown;
    public UnityEvent GripButtonUp;
    

    [HideInInspector] public int handIndex;
    string[] hand = new string[] { "Left", "Right" };
    void Start()
    {
        InitializeButtons();   

    }

   

    private void InitializeButtons()
    {
        (gripEvent = new AButtonEvent()).Initialize(isGripPressed, OnGripButtonEvent);
        (triggerEvent = new AButtonEvent()).Initialize(isTriggerPressed, OnTriggerButtonEvent);
        (primaryButtonEvent = new AButtonEvent()).Initialize(isPrimaryButtonPressed, OnPrimaryButtonEvent);
        (secondaryButtonEvent = new AButtonEvent()).Initialize(isSecondaryButtonPressed, OnSecondaryButtonEvent);
        (primaryAxisEvent = new AButtonEvent()).Initialize(isPrimaryAxisPressed, OnAxisClickButtonEvent);
    }

    // Button Functions
    private void OnGripButtonEvent(bool pressed)
    {
        isGripPressed = pressed;
        

        if (pressed)
        {
            Debug.Log("Grip Pressed " + hand[handIndex]);
            GripButtonDown.Invoke();
        }
        else
        {
            Debug.Log("Grip Released " + hand[handIndex]);
            GripButtonUp.Invoke();
        }
    }

    private void OnTriggerButtonEvent(bool pressed)
    {
        isTriggerPressed = pressed;
        

        if (pressed)
        {
            Debug.Log("Trigger Pressed " + hand[handIndex]);
        }
        else
        {
            Debug.Log("Trigger Released " + hand[handIndex]);
        }
    }

    private void OnPrimaryButtonEvent(bool pressed)
    {
        isPrimaryButtonPressed = pressed;
        if (pressed)
        {
            Debug.Log("Primary Pressed " + hand[handIndex]);
            
        }
        else
        {
            Debug.Log("Primary Released " + hand[handIndex]);
        }
    }

    private void OnSecondaryButtonEvent(bool pressed)
    {
        isSecondaryButtonPressed = pressed;
        if (pressed)
        {
            Debug.Log("Menu Pressed " + hand[handIndex]);
        }
    }

    private void OnAxisClickButtonEvent(bool pressed)
    {
        isPrimaryAxisPressed = pressed;
        

        if (pressed)
        {
            Debug.Log("Click Pressed " + hand[handIndex]);
        }
        else
        {
            Debug.Log("Click Released " + hand[handIndex]);
        }
    }



}
