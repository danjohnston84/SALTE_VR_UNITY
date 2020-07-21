using System.Collections.Generic;
using UnityEngine;
using OscJack;

public class OSCInput : MonoBehaviour
{
    /// <summary>
    /// OSC input class handles all osc messages recieved from the binaural audio engine.  Messages are used to build the user interface according
    /// to the test parameters stated in the audio engine. 
    /// </summary>



    #region Singleton
    private static OSCInput _instance;

    public static OSCInput Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new OSCInput();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    #endregion



    [HideInInspector]
    public int toolbarTab;
    public string currentTab;


    // OSC variables
    public string IPAddress = "127.0.0.1"; // IP address for OSC 
    public int oscPortIN = 6000; // Port for OSC
    public int oscPortOut = 9000;
    OscServer server;

    public float[] sliderValues;
    private int[] buttonStates;
    private int numberOfAttributeLabels;
    public bool visibleUI { get; set; }    
    public bool referenceButtonPresent = false;   
    public bool ABbuttonsPresent = false;
    public bool uiStart;
    public bool isMushra;



    public int slidersNum;
    public int labelsNum;     
    
    // Lists 
    public List<string> labelStrings = new List<string>();
    public List<string> attributeLabels = new List<string>();
    public List<string> screenMessages = new List<string>();

       

    #region Events
    // used to clear and build new UI
    public delegate void UpdateTest();
    public static event UpdateTest updateTest;
    #endregion
   

    private void Start()
    {
        BlankList();
        SetupArrays();      

        buttonStates = new int[6];
        for (int i = 0; i < buttonStates.Length; ++i)
        {
            buttonStates[i] = 0;
        }            

        // hide UI
        this.visibleUI = false; 
    }

    public void TestEvent()
    {
        updateTest();
    }

    private void SetupArrays()
    {
        sliderValues = new float[26];
        for (int i = 0; i < sliderValues.Length; ++i)
        {
            sliderValues[i] = 0;
        }        
    }

    /// <summary>
    /// Initialise the OSC server to input messages from audio engine 
    /// </summary>
    private void InitOSCServer()
    {
        var server = new OscServer(oscPortIN); // Create OSC server with port number
        Debug.Log("OSC server created");

        // Receives OSC data to display two messages on the screen
        server.MessageDispatcher.AddCallback(
               "/screenMessages", // OSC address
               (string address, OscDataHandle data) =>
               {
                   if (data.GetElementAsString(0) != null && data.GetElementAsString(1) != null)
                   {
                       screenMessages[0] = data.GetElementAsString(0); // small message
                       screenMessages[1] = data.GetElementAsString(1); // long message
                   }
               }
           );

        // Receives OSC data to show / hide UI
        server.MessageDispatcher.AddCallback(
               "/showUI", // OSC address
               (string address, OscDataHandle data) =>
               {
                   if (data.GetElementAsInt(0) != null)
                   {          
                       if (data.GetElementAsInt(0) == 1)
                       {
                           updateTest();                           
                           visibleUI = false;                           
                       }
                       else
                       {                          
                           visibleUI = true;                           
                       }
                   }
               }
           );

        // Shows/Hides reference trigger buttons for Mushra testing paradigm
        server.MessageDispatcher.AddCallback(
               "/RefTrigButtonPresent", // OSC address
               (string address, OscDataHandle data) =>
               {
                   if (data.GetElementAsInt(0) == 1)
                   {
                      referenceButtonPresent = true;                      
                   }
                   else
                   {
                      referenceButtonPresent = false;              
                   }
               }
           );

        // Shows/Hides A B trigger buttons for 3G testing paradigm
        server.MessageDispatcher.AddCallback(
               "/ABTrigButtonsPresent", // OSC address
               (string address, OscDataHandle data) =>
               {
                   if (data.GetElementAsInt(0) == 1)
                   {
                       ABbuttonsPresent = true;                  
                   }
                   else
                   {
                       ABbuttonsPresent = false;                      
                   }
               }
           );

        // Receives OSC data to control button hightlights, this is obsolete but will be introduced in future versions
        server.MessageDispatcher.AddCallback(
               "/buttonState", // OSC address
               (string address, OscDataHandle data) =>
               {
                   if (data.GetElementAsString(0) != null && data.GetElementAsInt(1) != null)
                   {
                       string oscButton = data.GetElementAsString(0);
                       int state = data.GetElementAsInt(1);

                       if (oscButton == "play") buttonStates[0] = state;
                       else if (oscButton == "stop") buttonStates[1] = state;
                       else if (oscButton == "loop") buttonStates[2] = state;
                       else if (oscButton == "A") buttonStates[3] = state;
                       else if (oscButton == "B") buttonStates[4] = state;
                       else if (oscButton == "reference") buttonStates[5] = state;
                   }
               }
           );


        // Receives OSC data to control button hightlights, this is obsolete but will be introduced in future versions
        server.MessageDispatcher.AddCallback(
               "/buttonState", // OSC address
               (string address, OscDataHandle data) =>
               {
                   if (data.GetElementAsString(0) != null && data.GetElementAsInt(1) != null)
                   {
                       string oscButton = data.GetElementAsString(0);
                       int state = data.GetElementAsInt(1);

                       if (oscButton == "play") buttonStates[0] = state;
                       else if (oscButton == "stop") buttonStates[1] = state;
                       else if (oscButton == "loop") buttonStates[2] = state;
                       else if (oscButton == "A") buttonStates[3] = state;
                       else if (oscButton == "B") buttonStates[4] = state;
                       else if (oscButton == "reference") buttonStates[5] = state;
                   }
               }
           );

        server.MessageDispatcher.AddCallback(
               "/condTrigButtonState", // OSC address
               (string address, OscDataHandle data) =>
               {
                   if (data.GetElementAsInt(0) != null && data.GetElementAsInt(1) != null)
                   {
                       int sliderIndex = data.GetElementAsInt(0); // 0,1,2,3... equals A,B,C,D...
                       int state = data.GetElementAsInt(1); // 0 off, 1 on
                   }
               }
           );

        // Receives OSC data to control sliders 
        server.MessageDispatcher.AddCallback(
              "/slider", // OSC address
              (string address, OscDataHandle data) =>
              {
                  if (data.GetElementAsInt(0) != null && data.GetElementAsFloat(1) != null)
                  {
                      Debug.Log("slider callback");
                      Debug.Log(data.GetElementAsInt(0) + " - " + data.GetElementAsFloat(1));
                      int index = data.GetElementAsInt(0);
                      float value = data.GetElementAsFloat(1);

                      sliderValues[data.GetElementAsInt(0)] = data.GetElementAsFloat(1);

                      if (sliderValues[index] != null) sliderValues[index] = value;
                  }
              }
          );

        // Receives messages to determine the slider type and amount 
        server.MessageDispatcher.AddCallback(
                  "/numOfSliders", // OSC address
                  (string address, OscDataHandle data) =>
                  {
                      if (data.GetElementAsInt(0) != null)
                      {
                          if (data.GetElementAsInt(0) > 0)
                          {
                              uiStart = true;
                          }
                          slidersNum = data.GetElementAsInt(0);
                      }
                  }
              );

        // recieves messages to set slider values in UI to refelct audio render engine
        server.MessageDispatcher.AddCallback(
                 "/sliderState", // OSC address
                 (string address, OscDataHandle data) =>
                 {
                     if (data.GetElementAsInt(0) != null && data.GetElementAsFloat(1) != null && data.GetElementAsFloat(2) != null && data.GetElementAsFloat(3) != null)
                     {
                         if (data.GetElementAsInt(3) == 3.00f && uiStart)
                         {                             
                             isMushra = false;
                             uiStart = false;
                         }
                         if (data.GetElementAsFloat(3) == 100.0f && uiStart)
                         {                             
                             isMushra = true;
                             uiStart = false;
                         }
                         sliderValues[data.GetElementAsInt(0)] = data.GetElementAsFloat(1); 
                     }
                 }
             );

        // receives messages ato set number of labels
        server.MessageDispatcher.AddCallback(
                  "/numOfRatingLabels", // OSC address
                  (string address, OscDataHandle data) =>
                  {
                      if (data.GetElementAsInt(0) != null)
                      {
                          labelsNum = data.GetElementAsInt(0);
                      }
                  }
              );

        // recieves messages to set rating labels
        server.MessageDispatcher.AddCallback(
                "/ratingLabel", // OSC address
                (string address, OscDataHandle data) =>
                {
                    if (data.GetElementAsInt(0) != null && data.GetElementAsString(1) != null)
                    {
                        labelStrings[data.GetElementAsInt(0)] = data.GetElementAsString(1);
                    }
                }
            );

        // receives messages to set number of attribute labels 
        server.MessageDispatcher.AddCallback(
                  "/numOfAttributeLabels", // OSC address
                  (string address, OscDataHandle data) =>
                  {
                      if (data.GetElementAsInt(0) != null)
                      {
                          numberOfAttributeLabels = data.GetElementAsInt(0);
                      }
                  }
              );

        // recieves messages to set attribute labels
        server.MessageDispatcher.AddCallback(
                "/attributeLabel", // OSC address
                (string address, OscDataHandle data) =>
                {
                    if (data.GetElementAsInt(0) != null && data.GetElementAsString(1) != null)
                    {
                        attributeLabels[data.GetElementAsInt(0)] = data.GetElementAsString(1);
                    }
                }
            );
    }


    private void BlankList()
    {
        for (int i = 0; i < 20; i++)
        {
            labelStrings.Add("");
        }
    }

  

}
