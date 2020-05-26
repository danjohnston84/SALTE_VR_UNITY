using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ExampleListener : MonoBehaviour
{
    public ButtonHandler primaryAxisClickHandler = null;

    public void OnEnable()
    {
        primaryAxisClickHandler.OnButtonDown += PrintPrimaryButtonDown;
        primaryAxisClickHandler.OnButtonUp += PrintPrimaryButtonUp;
    }

    public void OnDisable()
    {
        primaryAxisClickHandler.OnButtonDown -= PrintPrimaryButtonDown;
        primaryAxisClickHandler.OnButtonUp -= PrintPrimaryButtonUp;
    }

    private void PrintPrimaryButtonDown(XRController controller)
    {
        print("Primary button down");
    }

    private void PrintPrimaryButtonUp(XRController controller)
    {
        print("Primary button up");
    }

    private void PrintPrimaryAxis(XRController controller, Vector2 value)
    {

    }

    private void PrintTrigger(XRController controller, float value)
    {

    }
}
