using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HUDManager : MonoBehaviour
{
    PlayerController player;
    public int lineSelect;

    public GameObject renderTexture;
    private VideoPlayer introVideo;

    public GameObject subtitles;
    private TextMesh subtitlesText;

    public GameObject flashlightTutorial;


    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        introVideo = renderTexture.GetComponent<VideoPlayer>();
        subtitlesText = subtitles.GetComponent<TextMesh>();
    }

    void Update()
    {
        if (introVideo.isPaused) { introVideo.enabled = false; } // Desativa o vídeo introdutório assim que ele termina

        if (Input.GetKeyDown(KeyCode.F)) { flashlightTutorial.gameObject.SetActive(false); }
    }

    public void PlayerSubtitle(int lineSelect)
    {

    }
}
