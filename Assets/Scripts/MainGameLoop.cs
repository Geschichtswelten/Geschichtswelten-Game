using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainGameLoop : MonoBehaviour
{
    public GameObject doorLeft, doorRight;
    public BoxCollider endCollider;
    public GameObject endingCanvas;

    public bool killedArminius = true;
    public VideoClip[] videoClips;
    private VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        DontDestroyOnLoad(this);
    }

    public void EndingSequence()
    {
        doorLeft.transform.Rotate(0, -89, 0);
        doorRight.transform.Rotate(0, 89, 0);
        endCollider.gameObject.SetActive(true);
        Instantiate(endingCanvas, Vector3.zero, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {

            //playEndingVideo
            StartCoroutine(EndGame());

        }
    }

    private IEnumerator EndGame()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(394, LoadSceneMode.Single);
        loadScene.allowSceneActivation = true;
        JsonHandler.DeleteGameProfile();
        if (killedArminius)
        {
            videoPlayer.clip = videoClips[0];
        }
        else
        {
            videoPlayer.clip = videoClips[1];
        }
        videoPlayer.SetDirectAudioVolume(0, ButtonHandler.settings.dialogueVolume * ButtonHandler.settings.masterVolume);
        yield return new WaitUntil(()=>loadScene.isDone);
        //start Video
        yield return new WaitUntil(() => !videoPlayer.isPlaying);     //video.isDone
        var loadingScreen = GameObject.FindGameObjectWithTag("Finish");
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        loadScene = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        loadScene.allowSceneActivation = true;
        yield return new WaitUntil(()=>loadScene.isDone);
        Destroy(this.gameObject);
    }

}
