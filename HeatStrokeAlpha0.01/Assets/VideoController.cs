using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    public VideoPlayer[] videos;
    private int currentVideoIndex = 0;

    private void Start()
    {
        PlayNextVideo();
    }


    private void PlayNextVideo()
    {
        if (currentVideoIndex < videos.Length)
        {
            VideoPlayer videoPlayer = videos[currentVideoIndex];
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Play();

            // Stop and clean up the previous video player
            if (currentVideoIndex > 0)
            {
                VideoPlayer previousVideoPlayer = videos[currentVideoIndex - 1];
                previousVideoPlayer.Stop();
                previousVideoPlayer.loopPointReached -= OnVideoEnd;
            }

            currentVideoIndex++;
        }
        else
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        vp.loopPointReached -= OnVideoEnd;
        PlayNextVideo();
    }
}
