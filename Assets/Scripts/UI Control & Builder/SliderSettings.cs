
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OscJack;

public class SliderSettings : MonoBehaviour
{
    /// <summary>
    /// This Class is used to Setup the slider according to the test paradigms.
    /// It is also used to export the slider values to the renderer via OSC for experimental results
    /// </summary>


    #region Slider Setting Variables
    public bool isMushra;
    public int sliderIndex;
    public float sliderValue = 0;
    public float lastSliderValue = 0;
    Slider slider;
    [SerializeField] TextMeshProUGUI valueUI;
    #endregion

    #region OSC variables
    public string IPAddress = OSCInput.Instance.IPAddress;
    public int oscPortOut = OSCInput.Instance.oscPortOut;
    OscClient client;
    string oscAddress = "/slider";
    string msg;

    public string buttonmsg;
    #endregion


    [SerializeField] TextMeshProUGUI buttonLabel;
    [SerializeField] ButtonOutOSC buttonOut;
    [SerializeField] TextMeshProUGUI sliderAttribute;
    [SerializeField] GameObject buttonObject;
    OSCInput oscInput;


    private string[] _buttonText = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    void Start()
    {
        slider = GetComponent<Slider>();

        SetupRange(this.isMushra);  // SetUp Range Of Slider

        sliderValue = slider.value;        

    }

    void Update()
    {
        UpdateSliderVale();      
    }

    public void SetAttribute(string attribute)
    {
        sliderAttribute.text = attribute;
        
    }   

    private void SetupRange(bool isMushra)
    {
      
        buttonObject.SetActive(isMushra);
        sliderAttribute.enabled = !isMushra;
        if (isMushra)
        {
            slider.minValue = 0;
            slider.maxValue = 100;
            if (buttonObject.activeSelf)
            {
                buttonmsg = "cond" + _buttonText[sliderIndex];
            }

            oscInput = OSCInput.Instance;
            slider.value = oscInput.sliderValues[sliderIndex];

        }
        else
        {
            slider.minValue = -3;
            slider.maxValue = 3;
            oscInput = GameObject.Find("OSC.IN").GetComponent<OSCInput>();
            slider.value = this.oscInput.sliderValues[sliderIndex];

        }
    }

    public void SetUpIndex(int index)
    {
        sliderIndex = index;
        buttonLabel.text = _buttonText[index];
        
    } 


    public void SendButtonMsg()
    {
        client = new OscClient(IPAddress, oscPortOut);
        string buttonAddress = "/button";
        client.Send(buttonAddress, buttonmsg);
    }     

    private void UpdateSliderVale()
    {
        valueUI.text = this.slider.value.ToString();
    }

    public void GetValue(float value)
    {

        if (value == 0)
            return;

        Debug.Log(sliderIndex + ":" + value);
        sliderValue = value;

        SendData();
        
    }

    public void SendData()
    {
        client = new OscClient(IPAddress, oscPortOut);
        client.Send(oscAddress, sliderIndex, sliderValue);
        Debug.Log(oscAddress + ":" + ":" + sliderIndex + ":" + sliderValue);
    } 

}
