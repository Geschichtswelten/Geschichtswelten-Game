using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameLoop : MonoBehaviour
{
    public GameObject doorLeft, doorRight;
    public BoxCollider endCollider;

    public void EndingSequence()
    {
        doorLeft.transform.Rotate(0, -89, 0);
        doorRight.transform.Rotate(0, 89, 0);
        endCollider.gameObject.SetActive(true);
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
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        loadScene.allowSceneActivation = false;
        JsonHandler.DeleteGameProfile();
        //start Video
        yield return new WaitUntil(() => true);     //video.isDone
        loadScene.allowSceneActivation = true;
    }

}
