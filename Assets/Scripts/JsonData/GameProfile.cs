using System;
using System.Numerics;

[Serializable]
public class GameProfile
{

   
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public float playerRotX;
    public float playerRotY;
    public float playerRotZ;

    //1. index: item id ; 2. index: amount
    public int[][] playerItems;
    public bool woodCampBeaten;
    public bool copperCampBeaten;
    public bool arminiusBeaten;

    //might want to save constructions

    //save world stuff
    public float time;
    public int day;

    public GameProfile()
    {
        time = 21600f; //6 am
        day = 0;
        playerItems = new int[34][];
        for (int i = 0; i<playerItems.Length; i++)
        {
            playerItems[i] = new int[2];
            playerItems[i][0] = -1;
            playerItems[i][1] = 0;
        }
    }

    public GameProfile(float playerPosX, float playerPosY, float playerPosZ, float playerRotX, float playerRotY, float playerRotZ, int[][] playerItems, bool woodCampBeaten, bool copperCampBeaten, bool arminiusBeaten, float time, int day)
    {
        this.playerPosX = playerPosX;
        this.playerPosY = playerPosY;
        this.playerPosZ = playerPosZ;
        this.playerRotX = playerRotX;
        this.playerRotY = playerRotY;
        this.playerRotZ = playerRotZ;
        this.playerItems = playerItems;
        this.woodCampBeaten = woodCampBeaten;
        this.copperCampBeaten = copperCampBeaten;
        this.arminiusBeaten = arminiusBeaten;
        this.time = time;
        this.day = day;
    }
}
