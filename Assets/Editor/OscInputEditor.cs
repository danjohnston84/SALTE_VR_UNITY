using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(OSCInput))]
public class OscInputEditor : Editor
{
    /// <summary>
    /// Custom editor for the OSC Input class
    /// </summary>

    private OSCInput oscTarget;
    private SerializedObject soTarget;

    

    // Osc server variables
    SerializedProperty ipAddress;
    SerializedProperty oscPortIn;
    SerializedProperty oscPortOut;


    // Variables used to test UIbuilder within Editor, no need for OSC.
    SerializedProperty numberOfSliders;
   
    SerializedProperty isMushra;

    SerializedProperty numberOfLabels;
    SerializedProperty labelStrings;

    SerializedProperty numberOfAttributes;
    SerializedProperty attributeLabels;

    SerializedProperty screenMessages;

    SerializedProperty referenceButtton;
    SerializedProperty ABbutton;


    private void OnEnable()
    {

        oscTarget = (OSCInput)target;
        soTarget = new SerializedObject(target);
        


        // OSC server Setup 
        ipAddress = soTarget.FindProperty("IPAddress");
        oscPortIn = soTarget.FindProperty("oscPortIN");
        oscPortOut = soTarget.FindProperty("oscPortOut");


        // Test UI Builder with test OSC messages within Editor

        // Slider Tab
        numberOfSliders = soTarget.FindProperty("slidersNum");
        isMushra = soTarget.FindProperty("isMushra"); // slider type (mushra or 3G)
        oscTarget.isMushra = true; // set to mushra 
       
        // Label Tab
        labelStrings = soTarget.FindProperty("labelStrings"); 
        numberOfLabels = soTarget.FindProperty("labelsNum");
        attributeLabels = soTarget.FindProperty("attributeLabels"); 
        screenMessages = soTarget.FindProperty("screenMessages");

        // Button Tab
        referenceButtton = soTarget.FindProperty("referenceButtonPresent");
        ABbutton = soTarget.FindProperty("ABbuttonsPresent");

    }

    public override void OnInspectorGUI()
    {     

        soTarget.Update();
        EditorGUI.BeginChangeCheck();

        // Adjustments can be made to OSC ipaddress and ports
        EditorGUILayout.PropertyField(ipAddress);
        EditorGUILayout.PropertyField(oscPortIn);
        EditorGUILayout.PropertyField(oscPortOut);


        // Tabs for creating screen in the Editor without the need for OSC messages
        EditorGUILayout.LabelField("Build UI in Editor here without OSC", EditorStyles.boldLabel);
        
        // setup button tabs
        oscTarget.toolbarTab = GUILayout.Toolbar(oscTarget.toolbarTab, new string[] { "Slider Setup", "Label Setup", "Button Setup" });

        switch (oscTarget.toolbarTab)
        {
            case 0:
                oscTarget.currentTab = "Slider Setup";
                break;

            case 1:
                oscTarget.currentTab = "Label Setup";
                break;

            case 2:
                oscTarget.currentTab = "Button Setup";
                break;
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();
            GUI.FocusControl(null);

        }

        EditorGUI.BeginChangeCheck();

        switch (oscTarget.currentTab)
        {
            case "Slider Setup":
                EditorGUILayout.PropertyField(numberOfSliders);
               
                EditorGUILayout.PropertyField(isMushra);
                if (!oscTarget.isMushra)
                {                    
                        EditorGUILayout.PropertyField(attributeLabels);                
                }
                
                break;

            case "Label Setup":
                EditorGUILayout.PropertyField(numberOfLabels);
                EditorGUILayout.PropertyField(labelStrings);
                EditorGUILayout.PropertyField(screenMessages);
                break;

            case "Button Setup":
                EditorGUILayout.PropertyField(referenceButtton);
                EditorGUILayout.PropertyField(ABbutton);
                break;
        }


        if(GUILayout.Button("Build Screen"))
        {
            oscTarget.TestEvent();
        }

        if (EditorGUI.EndChangeCheck())
        {
            soTarget.ApplyModifiedProperties();            
        }

    }


}
