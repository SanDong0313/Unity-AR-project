using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.Video;

public class SimpleCloudRecoEventHandler : MonoBehaviour
{
    

    private CloudRecoBehaviour mCloudRecoBehaviour;
    private bool mIsScanning = false;
    private string mTargetMetadata = "";

    public ImageTargetBehaviour ImageTargetTemplate;
    public GameObject videoPlayerObject;
    // Register cloud reco callbacks
    void Awake()
    { 
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        mCloudRecoBehaviour.RegisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.RegisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.RegisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.RegisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.RegisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    //Unregister cloud reco callbacks when the handler is destroyed
    void OnDestroy()
    {
        mCloudRecoBehaviour.UnregisterOnInitializedEventHandler(OnInitialized);
        mCloudRecoBehaviour.UnregisterOnInitErrorEventHandler(OnInitError);
        mCloudRecoBehaviour.UnregisterOnUpdateErrorEventHandler(OnUpdateError);
        mCloudRecoBehaviour.UnregisterOnStateChangedEventHandler(OnStateChanged);
        mCloudRecoBehaviour.UnregisterOnNewSearchResultEventHandler(OnNewSearchResult);
    }
    public void OnInitialized(TargetFinder targetFinder)
    {
        Debug.Log("Cloud Reco initialized");
    }
    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }
    public void OnStateChanged(bool scanning)
    {
        mIsScanning = scanning;
        if (scanning)
        {
            // clear all known trackables
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().ClearTrackables(false);
        }
    }

    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        TargetFinder.CloudRecoSearchResult cloudRecoSearchResult =
            (TargetFinder.CloudRecoSearchResult)targetSearchResult;
        // do something with the target metadata
        mTargetMetadata = cloudRecoSearchResult.MetaData;
        // stop the target finder (i.e. stop scanning the cloud)
        mCloudRecoBehaviour.CloudRecoEnabled = false;

        if (ImageTargetTemplate)
        {
            // enable the new result with the same ImageTargetBehaviour: 
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<ImageTargetFinder>().EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);
        }

        string[] splitString = mTargetMetadata.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
        var url = splitString[0];
        Debug.Log("testinggggggggggggggggggggggggggggggggg" + url);
        PlayVideoFromLink(url);
    }

    void OnGUI()
    {
        // Display current 'scanning' status
        GUI.Box(new Rect(100, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");
        // Display metadata of latest detected cloud-target
        GUI.Box(new Rect(100, 200, 200, 50), "Metadata: " + mTargetMetadata);
        // If not scanning, show button
        // so that user can restart cloud scanning
        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
            {
                // Restart TargetFinder
                mCloudRecoBehaviour.CloudRecoEnabled = true;
            }
        }
    }

    void PlayVideoFromLink(string url)
    {
        VideoPlayer yPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
        yPlayer.enabled = true;
        yPlayer.url = url;
        yPlayer.Play();

        //videoPlayer.source = VideoSource.Url;
        //videoPlayer.url = videoUrl;
        //if (!videoPlayer.isPrepared)
        //{
        //    loadingPanel.SetActive(true);
        //}
        //videoPlayer.Prepare();

        //videoPlayer.Play();
    }


    public void ONTrackingFound()
    {
        videoPlayerObject.SetActive(true);
    }
    public void ONTrackingLost()
    {
        videoPlayerObject.SetActive(false);
    }
}

