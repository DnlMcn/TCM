using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HUDManager : MonoBehaviour
{
    public GameObject renderTexture;
    private VideoPlayer introVideo;

    void Start()
    {
        introVideo = renderTexture.GetComponent<VideoPlayer>();
    }

    void Update()
    {
        if (introVideo.isPaused) { introVideo.enabled = false; } // Desativa o vídeo introdutório assim que ele termina
    }
}
