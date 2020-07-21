using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;

public class ButtonOutOSC : MonoBehaviour
{
    /// <summary>
    /// Send out OSC data to the audio rendering engine to trigger audio stimuli or move to next trial.  This is determined by the 
    /// msg string.
    /// </summary>



    // OSC Variables
    string IPAddress = OSCInput.Instance.IPAddress;
    int oscPortOut = OSCInput.Instance.oscPortOut;
    OscClient client;

    string buttonAddress = "/button";

    public string msg;  // Change message in inspector

    private void Start()
    {
        client = new OscClient(IPAddress, oscPortOut);
    }

    public void sendData()
    {
        client.Send(buttonAddress, msg);
    }
}
