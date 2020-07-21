using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuilder : MonoBehaviour
{
    /// <summary>
    /// Class to build UI within VR environment based on OSC messages sent from the audio rendering engine. 
    /// 
    /// TODO:
    /// - Replace slider and label instantiation with object pooling
    /// </summary>

    [Header("Array of UI Objects")]
    public GameObject sliderPrefab;
    public GameObject labelPrefab;

    // Canvas transforms for UI building.  Main screen split into to, one for sliders, one for labels. 
    RectTransform sliderCanvasTransform;
    RectTransform labelCanvasTransform;
  
   int _numberOfSliders;
   int _numberOfLabels;
   


    // List of Sliders, Labels and on screen instructions
    List<GameObject> activeSliders = new List<GameObject>(); 
    List<GameObject> activeLabels = new List<GameObject>();
    List<TextMeshProUGUI> screenMessages = new List<TextMeshProUGUI>();

    // LisButton Variables
   [SerializeField] List<GameObject> _activeButtons = new List<GameObject>(); 


    // If test is Mushra or 3G
    bool isMushra;
       
    private void OnEnable()
    {
        OSCInput.updateTest += UpdateTest;
    }
    private void OnDisable()
    {
         OSCInput.updateTest -= UpdateTest;       
    }

    void Start()
    {
        Canvas sliderCanvas = GameObject.Find("SliderCanvas").GetComponent<Canvas>();
        sliderCanvasTransform = sliderCanvas.GetComponent<RectTransform>();


        Canvas labelCanvas = GameObject.Find("LabelCanvas").GetComponent<Canvas>();
        labelCanvasTransform = labelCanvas.GetComponent<RectTransform>();


        screenMessages.Add(GameObject.Find("ShortMessage").GetComponent<TextMeshProUGUI>());
        screenMessages.Add(GameObject.Find("LongMessage").GetComponent<TextMeshProUGUI>());

    }
   
    // Builds new UI environment from OSC messages, or loads previous one.
    public void UpdateTest()
    {

        ClearUI();

        // set test type
        this.isMushra = OSCInput.Instance.isMushra;
        
        
        int numberOfSliders = OSCInput.Instance.slidersNum;       
        int numberofLabels = OSCInput.Instance.labelsNum;
        

        ResetCanvas();
        SplitCanvas(numberOfSliders, numberofLabels);
        UpdateSliderValue();
        UpdateLabelText();
        UpdateInstructions();
        UpdateButtons();
        UpdateSliderAttributes();
    }

    private void ClearUI()
    {
        foreach (GameObject slider in this.activeSliders)
        {
            Destroy(slider);
        }
        activeSliders.Clear();

        foreach (GameObject label in this.activeLabels)
        {
            Destroy(label);
        }
        activeLabels.Clear();
    }

    private void ResetCanvas()
    {
        if (activeSliders.Count != 0)
        {
            foreach (GameObject slider in activeSliders)
            {
                Destroy(slider);
            }
            activeSliders.Clear();
        }

        if (activeLabels.Count != 0)
        {
            foreach (GameObject label in activeLabels)
            {
                Destroy(label);
            }
            activeLabels.Clear();
        }

    }

    private void UpdateSliderValue()
    {
        bool visableUI = OSCInput.Instance.visibleUI;

        if (visableUI)
        {
            for (int i = 0; i < this.activeSliders.Count; i++)
            {
                activeSliders[i].GetComponent<Slider>().value = OSCInput.Instance.sliderValues[i];
            }

        }
        else
            return;
    }

    private void UpdateLabelText()
    {
        this.activeLabels.Reverse();

        for (int i = 0; i < this.activeLabels.Count; i++)
        {
            activeLabels[i].GetComponent<TextMeshProUGUI>().text = OSCInput.Instance.labelStrings[i];
        }
    }

    private void UpdateInstructions()
    {
        for(int i = 0; i < this.screenMessages.Count; i++)
        {
            screenMessages[i].text = OSCInput.Instance.screenMessages[i];
        }
    }

    private void UpdateButtons()
    {
        bool referenceButtonPresent = OSCInput.Instance.referenceButtonPresent;
        bool ABbuttonsPresent = OSCInput.Instance.ABbuttonsPresent;

        if (_activeButtons.Count != 0)
        {
            _activeButtons[0].SetActive(referenceButtonPresent); // reference Button 
            _activeButtons[1].SetActive(ABbuttonsPresent); // reference Button 
            _activeButtons[2].SetActive(ABbuttonsPresent); // reference Button 
        }
        else
            return;
    }

    private void UpdateSliderAttributes()
    {
        for(int i = 0; i < activeSliders.Count; i++)
        {
            activeSliders[i].GetComponent<SliderSettings>().SetAttribute(OSCInput.Instance.attributeLabels[i]);
        }
    }

    private void SplitCanvas(int numberOfSliders, int numberOfLabels)
    {
        // Find the size of the canvas

        float heightOfCanvas = sliderCanvasTransform.sizeDelta.y;

        float width = sliderCanvasTransform.rect.width  / numberOfSliders;

    
        float height = labelCanvasTransform.rect.height / numberOfLabels;

        // Update the sliders
        for ( int i = 0; i < numberOfSliders; i++)
        {     
            // Create a new Slider Item 
            GameObject tmpSlider = Instantiate(sliderPrefab, sliderCanvasTransform.transform) as GameObject;

            RectTransform tmpRectTransform = tmpSlider.GetComponent<RectTransform>();
          
            float x = sliderCanvasTransform.rect.width / 2 + width * (i % numberOfSliders);
            float y = 0;

            tmpRectTransform.offsetMin = new Vector2(x, y);
         

            x = tmpRectTransform.offsetMin.x + width;
            tmpRectTransform.offsetMax = new Vector2(x - 1000, y);
            tmpRectTransform.sizeDelta = new Vector2(20f, 120f);

            // Add slider to list of active sliders
            this.activeSliders.Add(tmpSlider);
            SliderSettings sliderSettings = tmpSlider.GetComponent<SliderSettings>();
            sliderSettings.isMushra = isMushra;
            sliderSettings.SetUpIndex(i);
        }

        // Update the labels
        for(int j = 0; j < numberOfLabels; j++)
        {
            // Create a new Slider Item 
            GameObject tmpLabel = Instantiate(labelPrefab, labelCanvasTransform.transform) as GameObject;

            RectTransform tmpRectTransform = tmpLabel.GetComponent<RectTransform>();

            float x = 0;
            float y = 0;
            
            if (j != 0) 
            {
            y  = height * j;
            }

            tmpRectTransform.offsetMin = new Vector2(x, y);

            y = tmpRectTransform.offsetMin.y + height;
            tmpRectTransform.offsetMax = new Vector2(x , y - 100);
            tmpRectTransform.sizeDelta = new Vector2(140f, 30f);

            this.activeLabels.Add(tmpLabel);
            LabelSettings labelSettings = tmpLabel.GetComponent<LabelSettings>();
            labelSettings.UpdateIndex(j);
        }
    }
       
  

}
