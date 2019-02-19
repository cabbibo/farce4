using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using NatCorderU.Core;
using NatCorderU.Core.Recorders;
using NatCorderU.Core.Clocks;


using NatMicU;
using NatMicU.Core;
using NatMicU.Core.Recorders;


using NatShareU;

using UnityEngine.Video;


public class Recorder : MonoBehaviour
{

    public TouchToRay touch;

    public BoxCollider collider;

    public bool recording;


    public GameObject recordBtn;
    public GameObject shareBtn;
    public GameObject shareXBtn;
    public GameObject previewCanvas;
    public GameObject iconHolder;
   
    public Camera recordingCamera;
    public Camera previewCamera;
   

    private CameraRecorder cameraRecorder;
    private RealtimeClock recordingClock;

    public VideoPlayer videoPlayer;
    public AudioSource audioSrc;
    private string currPath;



    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRecord(){

      
            print("yaboi2");
            recording = true;
            StartRecording();
        



    }

    public void EndRecord(){

      if( recording == true ){
        recording = false;
        StopRecording();
      }

      print( "endRecord");

    }


    public void StartRecording () {

        // Start the microphone
        var microphoneFormat = Format.Default;
       // device, format, OnSampleBuffer

        var device = Device.Default;
        NatMic.StartRecording(device,microphoneFormat, OnSampleBuffer);
       
        // Start recording
        recordingClock = new RealtimeClock();
        var audioFormat = new AudioFormat(microphoneFormat.sampleRate, microphoneFormat.channelCount);
        
        float aspect = Camera.main.aspect;
        var videoFormat = new VideoFormat(720, (int)(720 / aspect));
        NatCorder.StartRecording(Container.MP4, videoFormat, audioFormat, OnRecordingFinish);
        
        recordingCamera.gameObject.SetActive( true );
        // Create a camera recorder for the main cam
        cameraRecorder = CameraRecorder.Create(recordingCamera, recordingClock);


    }

    public void StopRecording () {
        // Stop the microphone
        NatMic.StopRecording();
        // Stop recording
        cameraRecorder.Dispose();
        recordingCamera.gameObject.SetActive( false );
        NatCorder.StopRecording();
    }


    #region --Callbacks--

    // Invoked by NatCam once the camera preview starts
    private void OnPreviewStart () {

        // Display the camera preview
        //StartCoroutine(playVideo());
        //previewRawImage.texture = NatCam.Preview;
        //recordRawImage.texture = NatCam.Preview;
        // Scale the panel to match aspect ratios
        //previewAspectFitter.aspectRatio = NatCam.Preview.width / (float)NatCam.Preview.height;
        //recordAspectFitter.aspectRatio = NatCam.Preview.width / (float)NatCam.Preview.height;
    }

    // Invoked by NatMic on new microphone events
   private void OnSampleBuffer (float[] sampleBuffer, long timestamp) {
        // Send sample buffers directly to NatCorder for recording
        if ( NatCorder.IsRecording )
            NatCorder.CommitSamples( sampleBuffer, recordingClock.CurrentTimestamp );
    }

    // Invoked by NatCorder once video recording is complete
    private void OnRecordingFinish (string path) {
        currPath = path;
        ToggleRecordUI(false);
        StartCoroutine(playVideo(path)); 
    }
    #endregion

    IEnumerator playVideo(string src)
    //IEnumerator playVideo()
    {

        //Add VideoPlayer to the GameObject
        //videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        //audioSrc = gameObject.AddComponent<AudioSource>();
        //previewAspectFitter.aspectMode = AspectRatioFitter.AspectMode.None;
        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSrc.playOnAwake = false;
        audioSrc.Pause();

        //We want to play from video clip not from url

        //videoPlayer.source = VideoSource.VideoClip;

        // Vide clip from Url
        videoPlayer.source = VideoSource.Url;
        //videoPlayer.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";
        videoPlayer.url = src;


        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSrc);

        //Set video To Play then prepare Audio to prevent Buffering
        //videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }

        Debug.Log("Done Preparing Video");


        //Assign the Texture from Video to RawImage to be displayed
       // previewRawImage.texture = videoPlayer.texture;
       // previewAspectFitter.aspectRatio = videoPlayer.texture.width/ (float)videoPlayer.texture.height;

        //Play Video
        videoPlayer.Play();
        videoPlayer.isLooping = true;

        //Play Sound
        audioSrc.Play();



        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }

    public void KillShareBtnPress()
    {
        ToggleRecordUI(true);
        Debug.Log( "deactivate canavs");
    }

    public void ShareBtnPress()
    {
        videoPlayer.Pause();
        audioSrc.Pause();

        StartCoroutine(HandleShare());

    }

    IEnumerator HandleShare()
    {
        yield return new WaitForSeconds(0.1f);
        NatShare.Share(currPath, OnShare);
    }

    private void OnShare()
    {
        videoPlayer.Play();
        audioSrc.Play();
    }

    private void ToggleRecordUI(bool showRecordingUI)
    {

        if (showRecordingUI)
        {
            //previewAspectFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
           // NatCam.StartPreview(DeviceCamera.FrontCamera ?? DeviceCamera.RearCamera, OnPreviewStart);

            videoPlayer.Stop();
            audioSrc.Stop();



            recordBtn.SetActive(true);
            shareXBtn.SetActive(false);
            shareBtn.SetActive(false);
            previewCanvas.SetActive(false);
            iconHolder.SetActive(true);

        }
        else
        {
            recordBtn.SetActive(false);
            shareXBtn.SetActive(true);
            shareBtn.SetActive(true);
            previewCanvas.SetActive(true);
            iconHolder.SetActive(false);
            
        }
    }



}


