using System;
using System.Numerics;
using System.Collections.Generic;

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
    public SerializedList<SerializedList<int>> playerItems;
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
        playerItems = new SerializedList<SerializedList<int>>(new List<SerializedList<int>>());
        for (int i = 0; i<34; i++)
        {
            playerItems.list.Add(new SerializedList<int>(new List<int>()));
            playerItems.list[i].list.Add(-1);
            playerItems.list[i].list.Add(0);
        }

        playerPosX = 1926.73f;
        playerPosY = 51.58f;
        playerPosZ = 915f;
    }

    public GameProfile(float playerPosX, float playerPosY, float playerPosZ, float playerRotX, float playerRotY, float playerRotZ, SerializedList<SerializedList<int>> playerItems, bool woodCampBeaten, bool copperCampBeaten, bool arminiusBeaten, float time, int day)
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
