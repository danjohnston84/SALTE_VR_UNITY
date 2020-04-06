using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OscJack;

public class SliderSettings : MonoBehaviour
{
    /// <summary>
    /// This Class is used to Setup the slider according to the test paradigms.
    /// It is also used to export the slider values to the renderer via OSC
    /// </summary>


    #region Slider Setting Variables
    public bool _isMushra;
    public int _sliderIndex;
    public float _sliderValue = 0;
    public float _lastSliderValue = 0;
    Slider _slider;
    [SerializeField] TextMeshProUGUI _valueUI;
    #endregion

    #region OSC variables
    public string IPAddress = "127.0.0.1"; // IP address for OSC 
    public int oscPortOut = 9000; // Port for OSC
    OscClient client;
    string oscAddress = "/slider";
    string msg;

    public string buttonmsg;
    #endregion


    [SerializeField] TextMeshProUGUI _buttonLabel;
    [SerializeField] ButtonOutOSC _buttonOut;
    [SerializeField] TextMeshProUGUI _sliderAttribute;
    [SerializeField] GameObject _buttonObject;
    [SerializeField] OSCInput _oscInput;


    private string[] _buttonText = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    void Start()
    {
        _slider = GetComponent<Slider>();

        SetupRange(_isMushra);  // SetUp Range Of Slider

        _sliderValue = _slider.value;

        
      
    }

    void Update()
    {
        UpdateSliderVale();      
    }

    public void SetAttribute(string attribute)
    {
        _sliderAttribute.text = attribute;
        
    }

   

    private void SetupRange(bool isMushra)
    {
      
        _buttonObject.SetActive(isMushra);
        _sliderAttribute.enabled = !isMushra;
        if (isMushra)
        {
            _slider.minValue = 0;
            _slider.maxValue = 100;
            if (_buttonObject.activeSelf)
            {
                buttonmsg = "cond" + _buttonText[_sliderIndex];
            }

            _oscInput = GameObject.Find("OSC.IN").GetComponent<OSCInput>();
            _slider.value = _oscInput.sliderValues[_sliderIndex];

        }
        else
        {
            _slider.minValue = -3;
            _slider.maxValue = 3;
            _oscInput = GameObject.Find("OSC.IN").GetComponent<OSCInput>();
            _slider.value = _oscInput.sliderValues[_sliderIndex];

        }
    }

    public void SetUpIndex(int index)
    {
        _sliderIndex = index;
        _buttonLabel.text = _buttonText[index];
        
    } 


    public void SendButtonMsg()
    {
        client = new OscClient(IPAddress, oscPortOut);
        string buttonAddress = "/button";
        client.Send(buttonAddress, buttonmsg);
    }


  

    private void UpdateSliderVale()
    {
        _valueUI.text = _slider.value.ToString();
    }



    public void GetValue(float value)
    {

        if (value == 0)
            return;

        Debug.Log(_sliderIndex + ":" + value);
        _sliderValue = value;

        SendData();
        
    }

    public void SendData()
    {
        client = new OscClient(IPAddress, oscPortOut);
        client.Send(oscAddress, _sliderIndex, _sliderValue);
        Debug.Log(oscAddress + ":" + ":" + _sliderIndex + ":" + _sliderValue);
    }

    

 


}
