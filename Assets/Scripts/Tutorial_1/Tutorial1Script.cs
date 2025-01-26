using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial1Script : MonoBehaviour
{
    [Header("Tutorial References")]
    [SerializeField] private Stage stage;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject wrapperPlayer;
    public GameObject loadingScreen;
    [SerializeField] private AbstractEnemyBehaviour[] enemies;
    [SerializeField] private Inventory hotbarScript;
    [SerializeField] private Inventory equipmentScript;
    [Header("UI and Text")]
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private GameObject[] tips;
    [SerializeField] private string beginningString;
    [SerializeField] private string equipSwordString;
    [SerializeField] private string equipHelmetString;
    [SerializeField] private string firstEnemyAttack;
    [SerializeField] private string healString;
    [SerializeField] private string allEnemiesAttack;
    [SerializeField] private string allEnemiesDead;
    [SerializeField] private Animator animator;
    [Header("Audio")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private AudioClip tutClip;
    private AudioSource source;

    private PlayerBehaviour playerBehaviour;
    private WorldMusicScript worldMusicScript;
    private VideoPlayer videoPlayer;
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        worldMusicScript = FindAnyObjectByType<WorldMusicScript>();
        videoPlayer = GetComponent<VideoPlayer>();
        stage = Stage.start;
        source = GetComponent<AudioSource>();
        if (ButtonHandler.settings != null)
        {
            source.volume = ButtonHandler.settings.dialogueVolume;
        }
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        StartCoroutine(StartStage1());
    }

    private void OnEnable()
    {
        ButtonHandler.OnSettingsChanged += HandleVolumeChange;
    }

    private void OnDisable()
    {
        ButtonHandler.OnSettingsChanged -= HandleVolumeChange;
    }

    private void HandleVolumeChange()
    {
        source.volume = ButtonHandler.settings.dialogueVolume;
    }

    private IEnumerator StartStage1()
    {
        playerBehaviour.freeze = true;
        source.clip = audioClips[0];
        source.Play();
        text.text = beginningString;
        yield return new WaitUntil(() => !source.isPlaying);
        tips[0].SetActive(true);
        text.text = "";
        yield return new WaitUntil(() => playerBehaviour._inventoryOpen);
        tips[0].SetActive(false);
        source.clip = audioClips[1];
        source.Play();
        text.text = equipSwordString;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitUntil(() => !source.isPlaying);
        text.text = "";
        Cursor.lockState = CursorLockMode.Confined;
        tips[1].SetActive(true);
        yield return new WaitUntil(() => hotbarScript.GetItemIdForSlotInHotbar(0) == 25);
        tips[1].SetActive(false);
        source.clip = audioClips[2];
        source.Play();
        text.text = equipHelmetString;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitUntil(() => !source.isPlaying);
        text.text = "";
        tips[2].SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        yield return new WaitUntil(() => equipmentScript.GetItemIdForSlotInHotbar(0) == 26); //helmet ID
        playerBehaviour.CloseInventory();     
        tips[2].SetActive(false);
        stage = Stage.stage1;
        StartCoroutine(StartStage2());
    }

    public IEnumerator StartStage2()
    {
        source.clip = audioClips[3];
        source.Play();
        text.text = firstEnemyAttack;
        AsyncOperation loadVid = SceneManager.LoadSceneAsync(395, LoadSceneMode.Single);
        loadVid.allowSceneActivation = false;
        yield return new WaitUntil(() => !source.isPlaying);
        tips[3].SetActive(true);
        text.text = "";
        enemies[0]._target = player;
        playerBehaviour.freeze = false;
        playerBehaviour.state = PlayerBehaviour.Movementstate.walking;
        yield return new WaitUntil(() => enemies[0].dead);
        tips[3].SetActive(false);
        playerBehaviour.freeze = true;
        source.clip = audioClips[4];
        source.Play();
        text.text = healString;
        yield return new WaitUntil(() => !source.isPlaying);
        text.text = "";
        playerBehaviour.freeze = false;
        playerBehaviour.state = PlayerBehaviour.Movementstate.walking;
        playerBehaviour.freeze = true;
        source.clip = audioClips[5];
        source.Play();
        text.text = allEnemiesAttack;
        yield return new WaitUntil(() => !source.isPlaying);
        tips[4].SetActive(true);
        text.text = "";
        for (int i = 1; i < enemies.Length; i++)
        {
            enemies[i]._target = player;
        }
        playerBehaviour.freeze = false;
        yield return new WaitUntil(() => enemies[1].dead && enemies[2].dead && enemies[3].dead);
        tips[4].SetActive(false);
        animator.SetTrigger("Fade");
        yield return new WaitForSeconds(2.5f);
        loadVid.allowSceneActivation = true;
        source.Stop();
        yield return new WaitUntil(() => loadVid.isDone);
        loadingScreen = Instantiate(loadingScreen, Vector3.zero, Quaternion.identity);
        loadingScreen.SetActive(false);
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.SetDirectAudioVolume(0, ButtonHandler.settings.dialogueVolume * ButtonHandler.settings.masterVolume);
        worldMusicScript.ToggleMute();
        videoPlayer.Play();
        yield return new WaitUntil(() => !videoPlayer.isPlaying || Input.GetKeyDown(KeyCode.Space));
        videoPlayer.Stop();
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(2);
        AsyncOperation loadScene = SceneManager.LoadSceneAsync(392, LoadSceneMode.Additive);
        loadScene.allowSceneActivation = true;  //has to be set to false
        //ProgressBar

        GameProfile profile = null;
            profile = new GameProfile();
            JsonHandler.WriteGameProfile(profile);
            ButtonHandler.profile = profile;
            while (!loadScene.isDone)
            {
                
                yield return new WaitForSeconds(0.5f);
                loadScene.allowSceneActivation = true;
            }

            yield return new WaitForSeconds(5);
            worldMusicScript.StartingGame();
            SceneManager.UnloadSceneAsync(395);
            
    }

}

public enum Stage
{
    start, stage1, stage2, stage3
}
