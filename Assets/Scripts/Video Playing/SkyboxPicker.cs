using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OscJack;

public class SkyboxPicker : MonoBehaviour
{
    /// <summary>
    /// This Class is used to change the unity Skybox material based upon the OSC messages recieved by the SALTE Audio Renderer.
    /// </summary>


    public Material[] skyBoxes;
    VideoManager videoManager;
    PhotoManager photoManager;

    public Material[] visuals;

    public void ChangeSkybox(int index)
    {
        // Change the skybox according to index.
        RenderSettings.skybox = this.skyBoxes[index];
        this.structure.SetActive(true);
    } 

 

    // Just to test. Remove if not needed

    #region Editor Testing variables
    public bool skybox;
    public bool video;
    public bool photo;
    public GameObject structure;

    #endregion
    private void Start()
    {

        videoManager = GetComponent<VideoManager>();
        photoManager = GetComponent<PhotoManager>();

        #region Editor Testing -- Delete before roll out 

        if (skybox)
        {
            ChangeSkybox(1);
            this.structure.SetActive(true);
        }
        if (video)
        {
            videoManager.ChangeVideo360("D:/Downloads/test.mp4");
            RenderSettings.skybox = this.visuals[0];
            this.structure.SetActive(false);
        }
        if (photo)
        {
            photoManager.ChangePhoto360("D:/Downloads/testphoto.jpg");
            RenderSettings.skybox = this.visuals[1];
            this.structure.SetActive(false);
        }
        #endregion
    }
}
