using UnityEngine;
using TMPro;

public class LabelSettings : MonoBehaviour
{

    TextMeshProUGUI _labelText;
    public int _labelIndex;

    public void UpdateIndex(int index)
    {
        _labelIndex = index + 1;
    }

    public void UpdateLabelText(string text)
    {
        _labelText.text = text;
    }
}
