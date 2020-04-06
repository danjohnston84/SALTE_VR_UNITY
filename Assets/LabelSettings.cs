using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LabelSettings : MonoBehaviour
{

    TextMeshProUGUI _labelText;
    public int _labelIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateIndex(int index)
    {
        _labelIndex = index + 1;
    }

    public void UpdateLabelText(string text)
    {
        _labelText.text = text;
    }
}
