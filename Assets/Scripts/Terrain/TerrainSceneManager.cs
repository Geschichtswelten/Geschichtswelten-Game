using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class TerrainSceneManager : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Terrain Data")]
    [Header("Read only")]
    [SerializeField] private int chunk;

    private Vector3 playerPosition;
    
   

    private void Update()
    {
        playerPosition = player.transform.position;
        GetChunk();
        
    }

    private void GetChunk()
    {
        //get relative player Position
        int p_x = (int) Mathf.Ceil(playerPosition.x / 100F);
        int p_z = (int) Mathf.Ceil(playerPosition.z / 100F);

        chunk = (20 * p_z) + p_x;
    }


}
