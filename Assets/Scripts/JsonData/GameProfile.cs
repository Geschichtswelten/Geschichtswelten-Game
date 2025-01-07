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

    //1. index: item id ; 2. index: amount; 3. index: inventory Position
    public int[] playerItems;
    public bool woodCampBeaten;
    public bool copperCampBeaten;
    public bool arminiusBeaten;

    //might want to save constructions

    //save world stuff
    public float timeH, timeM, timeS;
    public int day;

    public GameProfile()
    {
        timeH = 6f;
        timeM = 0f;
        timeS = 0f;
        day = 0;
    }
}
