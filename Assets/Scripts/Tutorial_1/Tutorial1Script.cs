using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tutorial1Script : MonoBehaviour
{
    [Header("Tutorial References")]
    [SerializeField] private Stage stage;
    [SerializeField] private GameObject player;
    [SerializeField] private AbstractEnemyBehaviour[] enemies;
    [SerializeField] private Inventory hotbarScript;
    [SerializeField] private Inventory equipmentScript;
    [Header("UI and Text")]
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private GameObject[] tips;
    [SerializeField] private Image[] backgrounds;
    [SerializeField] private string beginningString;
    [SerializeField] private string equipSwordString;
    [SerializeField] private string equipHelmetString;

    [Header("Audio")]
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource source;

    private PlayerBehaviour playerBehaviour;
    

    void Start()
    {
        stage = Stage.start;
        source = GetComponent<AudioSource>();
        source.volume = ButtonHandler.settings.dialogueVolume * ButtonHandler.settings.masterVolume;
        playerBehaviour = player.GetComponent<PlayerBehaviour>();
        StartCoroutine(StartStage1());
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
        Time.timeScale = 0f;
        backgrounds[0].enabled = true;
        source.clip = audioClips[1];
        source.Play();
        text.text = equipSwordString;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitUntil(() => !source.isPlaying);
        text.text = "";
        Cursor.lockState = CursorLockMode.Confined;
        tips[1].SetActive(true);
        yield return new WaitUntil(() => hotbarScript.getItemIdForSlot(0) == 1);
        tips[1].SetActive(false);
        source.clip = audioClips[2];
        source.Play();
        text.text = equipHelmetString;
        Cursor.lockState = CursorLockMode.Locked;
        yield return new WaitUntil(() => !source.isPlaying);
        text.text = "";
        tips[2].SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        yield return new WaitUntil(() => equipmentScript.getItemIdForSlot(0) == 2); //helmet ID
        //playerBehaviour.CloseInventory();     //NOT IMPLEMENTED
        tips[2].SetActive(false);
        Time.timeScale = 1f;
        enemies[0]._target = player;
        playerBehaviour.freeze = false;
        stage = Stage.stage1;
    }

    public IEnumerator StartStage2()
    {
        yield return null;
    }
}

public enum Stage
{
    start, stage1, stage2, stage3
}
