using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int id;

    public int X;
    public int Y;

    // ---> Total Width and/or Height of a room will always be Odd as the origin is 0,0, i.e. Total Width is 1 + Width * 2
    public int Width; //Number of tiles from origin to the Right or Left Edge of the room
    public int Height; //Number of tile from origin to the Top or Bottom Edge of the room

    public Room() {
        id = 0;
        X = 0;
        Y = 0;
        Width = 0;
        Height = 0;
    }

    public Room(int _id, int _X, int _Y, int _Width, int _Height)
    {
        id = _id;
        X = _X;
        Y = _Y;
        Width = _Width;
        Height = _Height;
    }
}
