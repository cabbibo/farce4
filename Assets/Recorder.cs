using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEditor;

using NatCorder;
using NatCorder.Clocks;
using NatCorder.Inputs;
//using NatMic.Inputs;
/*using NatCorderU.Core;
using NatCorderU.Core.Recorders;
using NatCorderU.Core.Clocks;


using NatMicU;
using NatMicU.Core;
using NatMicU.Core.Recorders;*/


using NatShareU;

using UnityEngine.Video;


public class Recorder : MonoBehaviour
{

    public TouchToRay touch;

    public BoxCollider collider;

    public bool recording;
    public Haptico haptics;


    public GameObject recordBtn;
    public GameObject shareBtn;
    public GameObject shareXBtn;
    public GameObject previewCanvas;
    public GameObject iconHolder;
    public GameObject recordingIcon;




   
    public Camera recordingCamera;
    //public Camera previewCamera;
    public Camera UICamera;
   

//    private CameraRecorder cameraRecorder;
    //private RealtimeClock recordingClock;



    [Header("Recording")]
    public int videoWidth = 720;

    [Header("Microphone")]
    public bool recordMicrophone;
    public AudioSource microphoneSource;

    private MP4Recorder videoRecorder;
    private IClock recordingClock;
    private CameraInput cameraInput;
    private AudioInput audioInput;


    public VideoPlayer videoPlayer;
    public AudioSource audioSrc;
    private string currPath;

    private bool oRecording = false;
    private bool ooRecording= false;

    public int recordStartFrame;

    public bool saveable = false;
    public bool previewing = false;



    public UnityEvent StartRecordEvent;
    public UnityEvent EndRecordEvent;
    public UnityEvent SaveRecordingEvent;
    public UnityEvent KillRecordingEvent;
    public UnityEvent PlaybackScreenStartEvent;




    // Start is called before the first frame update
    void Start()
    {
        recordStartFrame = 20;
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        ooRecording = oRecording;
        oRecording = recording;

        recordStartFrame ++;
        if( recording && recordStartFrame == 3 ){
            StartRecording();
        }
        
    }

    public void StartRecord(){

        recording = true;
        recordStartFrame = 0;
        //recordingCamera.gameObject.SetActive( true );
       
       // StartRecording();

    }

    public void EndRecord(){

      if( recording == true ){
        ooRecording = false;
        oRecording = false;
        recording = false;
        StopRecording();
      }


    }


    public void StartRecording () { 


        float aspect = Camera.main.aspect;
     

        Debug.Log( "AUDIO INFO : ");
        Debug.Log(AudioSettings.outputSampleRate + " : SAMPLE RATE" );
        Debug.Log((int)AudioSettings.speakerMode + " : speakerMode" );

        // Start recording
        recordingClock = new RealtimeClock();
        videoRecorder = new MP4Recorder(
            videoWidth,
            (int)(videoWidth / aspect),
            30,
            AudioSettings.outputSampleRate,
            (int)AudioSettings.speakerMode,
            OnReplay
        );

        // Create recording inputs
        cameraInput = new CameraInput(videoRecorder, recordingClock, Camera.main);
     
        StartMicrophone();
        audioInput = new AudioInput(videoRecorder, recordingClock, microphoneSource, true);

        StartRecordEvent.Invoke();
        iconHolder.SetActive( false );

    }



        private void StartMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR // No `Microphone` API on WebGL :(
            // Create a microphone clip
                microphoneSource.clip = Microphone.Start(null, true, 60, AudioSettings.outputSampleRate);
                while (Microphone.GetPosition(null) <= 0) ;
            // Play through audio source
                microphoneSource.timeSamples = Microphone.GetPosition(null);
                microphoneSource.loop = true;
                microphoneSource.Play();
            #endif
        }

        public void StopRecording () {

            // Stop the recording inputs
            if (recordMicrophone) {
                StopMicrophone();
                audioInput.Dispose();
            }

            cameraInput.Dispose();

            // Stop recording
            videoRecorder.Dispose();

            EndRecordEvent.Invoke();

        }

        private void StopMicrophone () {
            #if !UNITY_WEBGL || UNITY_EDITOR
                Microphone.End(null);
                microphoneSource.Stop();
            #endif
        }

        private void OnReplay (string path) {

            currPath = path;
            Debug.Log("Saved recording to: "+path);

            ToggleRecordUI(false);
            // Playback the video
            #if UNITY_EDITOR
            

            FullVidPlay(path);////EditorUtility.OpenWithDefaultApp(path);
            
            #elif UNITY_IOS

            FullVidPlay(path);

            #elif UNITY_ANDROID
            
            Handheld.PlayFullScreenMovie(path);
            
            #endif
        }



    private void FullVidPlay( string path ){

        print("PLAYIN");
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = path;

        videoPlayer.Prepare();

        //Wait until video is prepared
       /* while (!videoPlayer.isPrepared)
        {
            yield return null;
        }*/


        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSrc);

          //Play Video
        videoPlayer.Play();
        videoPlayer.isLooping = true;

        //Play Sound
        audioSrc.volume = 1;
        audioSrc.Play();
        audioSrc.loop = true;

        PlaybackScreenStartEvent.Invoke();


        //Debug.Log("Playing Video");
      /*  while (videoPlayer.isPlaying)
        {
            //Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }*/
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

       // print( sampleBuffer.Length );
     /*   if ( NatCorder.IsRecording )
            NatCorder.CommitSamples( sampleBuffer, recordingClock.CurrentTimestamp );*/
    }

    // Invoked by NatCorder once video recording is complete
    /*private void OnRecordingFinish (string path) {
        currPath = path;
        ToggleRecordUI(false);
        StartCoroutine(playVideo(path)); 
    }*/
    #endregion

    /*IEnumerator playVideo(string src)
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
        //audioSrc.Pause();

        //We want to play from video clip not from url

        //videoPlayer.source = VideoSource.VideoClip;

        // Vide clip from Url
        videoPlayer.source = VideoSource.Url;
        //videoPlayer.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";
        videoPlayer.url = src;


      
        //Set video To Play then prepare Audio to prevent Buffering
        //videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        //Wait until video is prepared
        while (!videoPlayer.isPrepared)
        {
            yield return null;
        }



  //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSrc);


        //Assign the Texture from Video to RawImage to be displayed
       // previewRawImage.texture = videoPlayer.texture;
       // previewAspectFitter.aspectRatio = videoPlayer.texture.width/ (float)videoPlayer.texture.height;

        //Play Video
        videoPlayer.Play();
        videoPlayer.isLooping = true;

        //Play Sound
        audioSrc.volume = 1;
        audioSrc.Play();
        audioSrc.loop = true;

        PlaybackScreenStartEvent.Invoke();


        //Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            //Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }

        //Debug.Log("Done Playing Video");

    }*/

    public void KillShareBtnPress()
    {


        if( saveable == true ){

            ToggleRecordUI(true);
            KillRecordingEvent.Invoke();

        }else{
            print("nonsave");
        }
    }

    public void ShareBtnPress()
    {

        if( saveable == true ){
            SaveRecordingEvent.Invoke();
            videoPlayer.Pause();
           // audioSrc.Pause();
            audioSrc.volume = 0;
            haptics.TriggerSuccess();
            StartCoroutine(HandleShare());
            KillShareBtnPress();
        }else{
            print( "nonsaveeee");
        }

    }

    IEnumerator HandleShare()
    {
        yield return new WaitForSeconds(0.1f);
        NatShare.Share(currPath, OnShare);
    }

    private void OnShare()
    {
        videoPlayer.Play();
        audioSrc.volume = 1;
    }

    private void ToggleRecordUI(bool showRecordingUI)
    {

        if (showRecordingUI)
        {
            //previewAspectFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
           // NatCam.StartPreview(DeviceCamera.FrontCamera ?? DeviceCamera.RearCamera, OnPreviewStart);

            videoPlayer.Stop();
           // audioSrc.Stop();
            audioSrc.volume = 0;



            //recordBtn.SetActive(true);
            //shareXBtn.SetActive(false);
            //shareBtn.SetActive(false);
            previewCanvas.SetActive(false);
            iconHolder.SetActive(true);

            saveable = false;
            previewing = false;



            haptics.TriggerSuccess();
            //previewCamera.gameObject.SetActive( true );
            // UICamera.gameObject.SetActive( false );
           // recordingCamera.GetComponent<Camera>().enabled = true;

        }
        else
        {

            saveable = true;
            previewing = true;
            //recordBtn.SetActive(false);
            //shareXBtn.SetActive(true);
            //shareBtn.SetActive(true);
            previewCanvas.SetActive(true);
            iconHolder.SetActive(false);


            //UICamera.gameObject.SetActive( true );
            //recordingCamera.GetComponent<Camera>().enabled = false; //.SetActive( false );
            
        }
    }



}


