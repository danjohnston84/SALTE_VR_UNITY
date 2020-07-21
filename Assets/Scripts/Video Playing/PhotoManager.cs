using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PhotoManager : MonoBehaviour
{
    /// <summary>
    /// Changes Skybox to 360 degree image.  Waiting on OSC messages to be added to audio rendering engine. 
    /// </summary>


    public Material photoMaterial;  


    void Start()
    {
        string debugTest = "D:/Downloads/testphoto.jpg";       
    }
    public void ChangePhoto360(string filepath)
    {
       string filePath = filepath;

        if (System.IO.File.Exists(filePath))
        {
            Debug.Log("Exists!!");
            var bytes = System.IO.File.ReadAllBytes(filePath);
            var tex = new Texture2D(4096, 2048, TextureFormat.RGBA32, false);

            tex.LoadImage(bytes);
            photoMaterial.mainTexture = tex;
        } else
        {
            Debug.Log("No File Exists");
        }
    }


}
