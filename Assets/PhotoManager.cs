using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PhotoManager : MonoBehaviour
{

    public Material photoMaterial;


    


    void Start()
    {
        string debugTest = "D:/Downloads/testphoto.jpg";
       // ChangePhoto360(debugTest);
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
