using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBuilder : MonoBehaviour
{
    [Header("Array of UI Objects")]
    public GameObject[] _UIPrefabs;
    public GameObject _labelPrefab;
    [SerializeField] Canvas _UICanvas;
    [SerializeField] Canvas _labelCanvas;

    [SerializeField] GameObject _panel;
    RectTransform _canvasTransform;
    RectTransform _labelTransform;
   public RectTransform[] _UItransforms;

    // Variables to build UI
    [Header("UI variables recieved via OSC")]
    public int _numberOfSliders;
    public int _numberOfLabels;
    public List<string> _lables = new List<string>();


    // List of Sliders and Labels
    public List<GameObject> _activeSliders = new List<GameObject>(); // turn to private once its working
    public List<GameObject> _activeLabels = new List<GameObject>(); // turn to private once its working




    // LisButton Variables
   [SerializeField] List<GameObject> _activeButtons = new List<GameObject>(); // turn to private once its working
    public bool _referenceButtonPresent = false;
    public bool _ABbuttonsPresent = false;

    // If test is Mushra or 3G
    public bool _isMushra;


    [SerializeField] OSCInput _oscInput;


    // Testing variables;
    public float width;
    public float height;
    public float widthSegment;
    public float heightSegment;

    // Start is called before the first frame update
    void Start()
    {
       
        _canvasTransform = _UICanvas.GetComponent<RectTransform>();
        _labelTransform = _labelCanvas.GetComponent<RectTransform>();

        #region Events
        OSCInput.testOSCEvent += TestEvent; // Delete
        OSCInput.isMushra += TestType;
        OSCInput.sliderValue += UpdateSliderValues;
      
        #endregion

        //    UpdateTest();

    }

    private void OnDisable()
    {
        #region Events
        OSCInput.testOSCEvent -= TestEvent; // Delete
        OSCInput.isMushra -= TestType;
        OSCInput.sliderValue -= UpdateSliderValues;
        
        #endregion
    }

    private void TestType(bool isMushra)
    {
        _isMushra = isMushra;
    }


    private void TestEvent(bool test)
    {
        Debug.Log("test: " + test);
    }

    public void UpdateTest()
    {
       
        

            UpdateUI();


            if (!_isMushra)
            {
                _oscInput.UpdateSliderAttributes();
            }
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateButtons();
    }

    private void UpdateUI()
    {
        ClearUI();
        ResetCanvas();
        SplitCanvas(_numberOfSliders, _numberOfLabels, 0);
        _oscInput.UpdateSliderValue();
        _oscInput.UpdateLabelText();


    }


   

    private void ResetCanvas()
    {
        if (_activeSliders.Count != 0)
        {
            foreach (GameObject slider in _activeSliders)
            {
                Destroy(slider);
            }

            _activeSliders.Clear();
        }

        if (_activeLabels.Count != 0)
        {
            foreach (GameObject label in _activeLabels)
            {
                Destroy(label);
            }

            _activeLabels.Clear();
        }

    }

    private void SplitCanvas(int numberOfSliders, int numberOfLabels, int type)
    {
        // Find the size of the canvas

        float heightOfCanvas = _canvasTransform.sizeDelta.y;

        float width = _canvasTransform.rect.width  / numberOfSliders;

     //   float ratio = width / _UItransforms[type].rect.width;
        float height = _labelTransform.rect.height / numberOfLabels;




// Update the sliders
        for ( int i = 0; i < numberOfSliders; i++)
        {

         


            // Create a new Slider Item 
            GameObject tmpSlider = Instantiate(_UIPrefabs[type], _UICanvas.transform) as GameObject;

            RectTransform tmpRectTransform = tmpSlider.GetComponent<RectTransform>();
          
            float x = _canvasTransform.rect.width / 2 + width * (i % numberOfSliders);
            float y = 0;

            tmpRectTransform.offsetMin = new Vector2(x, y);
         

            x = tmpRectTransform.offsetMin.x + width;
            tmpRectTransform.offsetMax = new Vector2(x - 1000, y);
            tmpRectTransform.sizeDelta = new Vector2(20f, 120f);

            // Add slider to list of active sliders
            _activeSliders.Add(tmpSlider);
            SliderSettings sliderSettings = tmpSlider.GetComponent<SliderSettings>();
            sliderSettings._isMushra = _isMushra;
            sliderSettings.SetUpIndex(i);
        }

// Update the labels
        for(int j = 0; j < numberOfLabels; j++)
        {
            // Create a new Slider Item 
            GameObject tmpLabel = Instantiate(_labelPrefab, _labelCanvas.transform) as GameObject;

            RectTransform tmpRectTransform = tmpLabel.GetComponent<RectTransform>();

            float x = 0;
            float y = 0;
            if (j != 0) {
          y  = height * j;
            }
            tmpRectTransform.offsetMin = new Vector2(x, y);


            y = tmpRectTransform.offsetMin.y + height;
            tmpRectTransform.offsetMax = new Vector2(x , y - 100);
            tmpRectTransform.sizeDelta = new Vector2(140f, 30f);

            _activeLabels.Add(tmpLabel);
            LabelSettings labelSettings = tmpLabel.GetComponent<LabelSettings>();
            labelSettings.UpdateIndex(j);

        }



    }


    public void SetLabelText(int index, string text)
    {
        int newIndex = index - 1;

        TextMeshProUGUI tmpText = _activeLabels[newIndex].GetComponent<TextMeshProUGUI>();
        tmpText.text = text;

    }


    private void UpdateButtons()
    {
        if (_activeButtons.Count != 0)
        {
            _activeButtons[0].SetActive(_referenceButtonPresent); // reference Button 
            _activeButtons[1].SetActive(_ABbuttonsPresent); // reference Button 
            _activeButtons[2].SetActive(_ABbuttonsPresent); // reference Button 
        }
        else
            return;
    }


    private void UpdateSliderValues(int index, float value)
    {
        _activeSliders[index].GetComponent<Slider>().value = value;
    }
   
    private void ClearUI()
    {


        for(int i = 0; i < _oscInput.sliderValues.Length; i++)
        {
           // _oscInput.sliderValues[i] = 0;
        }

        foreach(GameObject slider in _activeSliders)
        {
            Destroy(slider);
        }
        _activeSliders.Clear();

        foreach(GameObject label in _activeLabels)
        {
            Destroy(label);
        }

        _activeLabels.Clear();
    }

}
