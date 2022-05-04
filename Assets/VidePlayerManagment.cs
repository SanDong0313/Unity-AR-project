using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VidePlayerManagment : MonoBehaviour
{
    // Start is called before the first frame update

    public VideoPlayer videoPlayer;
    public string videoLink;
    public GameObject loadingPanel;
    void Start()
    {
      //  SetVideoByUrl(videoLink);
    }

    // Update is called once per frame
   
    public void SetVideoByUrl(string videoUrl)
    {
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoUrl;
        if (!videoPlayer.isPrepared)
        {
            loadingPanel.SetActive(true);
        }
        videoPlayer.Prepare();

        videoPlayer.Play();
       // replaybutton.SetActive(false);

    }

    void Update()
    {
        if (videoPlayer.isPrepared)
        {
            loadingPanel.SetActive(false);
        }
    }

    public void OnTrckingLostAaction()
    {

    }
    public void OnTrackingFoundOption()
    {

    }
}
