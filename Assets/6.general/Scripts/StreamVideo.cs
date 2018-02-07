using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class StreamVideo : MonoBehaviour {


    VideoPlayer videoPlayer;
    VideoSource videoSource;

    AudioSource audioSource;

    public VideoClip videoToPlay;
    public RawImage image;
    public Canvas interactiveCanvas;

    bool moveCanvas;
    float startTime;
    public float speed = 1.1f;
    public Transform startMarker;
    public Transform endMarker;

    private float journeyLength = 3f ;
    RectTransform recttr;

    float movingDir = -8f;

    //When play button pushed, interactive canvas moces to left to see video panel

    public void StartVideo()
    {

        StartCoroutine(playVideo());

    }

    // Use this for initialization
    void Start () {
        journeyLength = journeyLength * speed;
        recttr = interactiveCanvas.GetComponent<RectTransform>();
    }
    int i = 0;
    // Update is called once per frame

    void Update()
    {

        if (Input.GetKeyDown("a") && moveCanvas == false)
        {
            startTime = Time.time;
            movingDir = -8f;
      
            moveCanvas = true;

        }

        if (Input.GetKeyDown("s") && moveCanvas == false)
        {
            StopVideo();
            startTime = Time.time;
            movingDir = 8f;

            moveCanvas = true;

        }
    }

    void FixedUpdate () {

    //   //StartVideo();

    if (moveCanvas)
    {
            float distCovered = (Time.time - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
        if (recttr != null)
        {
            float a = recttr.position.x;
            recttr.anchoredPosition = new Vector2(Mathf.Lerp(recttr.anchoredPosition.x,recttr.anchoredPosition.x + movingDir, fracJourney), recttr.anchoredPosition.y);

            Debug.Log(recttr.anchoredPosition);
            Debug.Log(fracJourney);

            if (fracJourney >= 1)
            {

                moveCanvas = false;
                if (movingDir == -8f)
                {
                    StartVideo();
                }

            }

        }

            
      }

    }




    IEnumerator playVideo()
    {





        //Add VideoPlayer to the GameObject
        videoPlayer = gameObject.AddComponent<VideoPlayer>();

        //Add AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();

        //Disable Play on Awake for both Video and Audio
        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        audioSource.Pause();

        //We want to play from video clip not from url

        videoPlayer.source = VideoSource.VideoClip;

        // Vide clip from Url
        // videoPlayer.source = VideoSource.Url;
        //videoPlayer.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4";


        //Set Audio Output to AudioSource
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //Assign the Audio from Video to AudioSource to be played
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

       // Set video To Play then prepare Audio to prevent Buffering
        videoPlayer.clip = videoToPlay;
        videoPlayer.Prepare();

        //Wait until video is prepared
        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!videoPlayer.isPrepared)
        {
            Debug.Log("Preparing Video");
            //Prepare/Wait for 5 sceonds only
            yield return waitTime;
            //Break out of the while loop after 5 seconds wait
            break;
        }

        Debug.Log("Done Preparing Video");



        //Assign the Texture from Video to RawImage to be displayed
        image.texture = videoPlayer.texture;

        //Play Video
        videoPlayer.Play();

        //Play Sound
        audioSource.Play();

        Debug.Log("Playing Video");
        while (videoPlayer.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)videoPlayer.time));
            yield return null;
        }
        Debug.Log("Done Playing Video");
    }


    public void StopVideo()
    {
        videoPlayer.Stop();

    }
}
