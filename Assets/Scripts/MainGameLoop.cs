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
    private GameObject _arminius;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        DontDestroyOnLoad(this);
    }

    public void EndingSequence(GameObject obj)
    {
        Time.timeScale = 0f;
        doorLeft.transform.Rotate(0, -89, 0);
        doorRight.transform.Rotate(0, 89, 0);
        endCollider.gameObject.SetActive(true);
        Instantiate(endingCanvas, Vector3.zero, Quaternion.identity);
        _arminius = obj;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void KilledArminius(GameObject obj)
    {
        Destroy(_arminius);
        StartCoroutine(del(obj));
    }

    IEnumerator del(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(4.5f);
        Time.timeScale = 1f;
        Destroy(obj);
    }

    public void Entered () {

            //playEndingVideo
            StartCoroutine(EndGame());

        
    }

    private IEnumerator EndGame()
    {
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(394, LoadSceneMode.Single);
        loadScene.allowSceneActivation = true;
        JsonHandler.DeleteGameProfile();
        if (killedArminius && videoClips.Length > 0)
        {
            videoPlayer.clip = videoClips[0];
        }
        else
        {
            videoPlayer.clip = videoClips[1];
        }
        videoPlayer.SetDirectAudioVolume(0, ButtonHandler.settings.dialogueVolume * ButtonHandler.settings.masterVolume);
        ButtonHandler.InvokeOnEndGame();
        yield return new WaitUntil(()=>loadScene.isDone);
        //start Video
        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.Play();
        yield return new WaitForSecondsRealtime(6);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);     //video.isDone
        loadScene = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        loadScene.allowSceneActivation = true;
        yield return new WaitUntil(()=>loadScene.isDone);
        Destroy(this.gameObject);
    }

}
