using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class VideoManager : MonoBehaviour
{
    // Class variables 
       
     VideoPlayer videoPlayer;

    void Start()
    {
        string testDebug = "D:/Downloads/test.mp4";
        VideoSetup();
        ChangeVideo360(testDebug);
    }


    void VideoSetup()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.playOnAwake = true;
        videoPlayer.isLooping = true;
    }


    public void ChangeVideo360(string filepath)
    {
        if (System.IO.File.Exists(filepath))
        {
            videoPlayer.url = filepath;
        } else
            return;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
