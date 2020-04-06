using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllerManager : MonoBehaviour
{

    public bool IsGripPressed;
    public bool IsTriggerPressed;
    public bool IsMenuPressed;
    public bool IsClickPressed;

    // Button Events
    public AButtonEvent GripEvent { get; set; }
    public AButtonEvent TriggerEvent { get; set; }
    public AButtonEvent MenuEvent { get; set; }
    public AButtonEvent ClickEvent { get; set; }


    public UnityEvent IsGripped;
    public UnityEvent NotGripped;

    void Start()
    {
        InitializeButtons();
     

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializeButtons()
    {
        (GripEvent = new AButtonEvent()).Initialize(IsGripPressed, OnGripButtonEvent);
        (TriggerEvent = new AButtonEvent()).Initialize(IsTriggerPressed, OnTriggerButtonEvent);
        (MenuEvent = new AButtonEvent()).Initialize(IsMenuPressed, OnMenuButtonEvent);
        (ClickEvent = new AButtonEvent()).Initialize(IsClickPressed, OnClickButtonEvent);
    }

    // Button Functions
    private void OnGripButtonEvent(bool pressed)
    {
        IsGripPressed = pressed;
      

        if (pressed)
        {
            Debug.Log("Grip Pressed");
            IsGripped.Invoke();
        }
        else
        {
            NotGripped.Invoke();
            Debug.Log("Grip Released");
        }
    }

    private void OnTriggerButtonEvent(bool pressed)
    {
        IsTriggerPressed = pressed;
     

        if (pressed)
        {
            Debug.Log("Trigger Pressed");
        }
        else
        {
            Debug.Log("Trigger Released");
        }
    }

    private void OnMenuButtonEvent(bool pressed)
    {
        IsMenuPressed = pressed;
        if (pressed)
        {
            Debug.Log("Menu Pressed");
        }
    }

    private void OnClickButtonEvent(bool pressed)
    {
        IsClickPressed = pressed;
   

        if (pressed)
        {
            Debug.Log("Click Pressed");
        }
        else
        {
            Debug.Log("Click Released");
        }
    }
}
