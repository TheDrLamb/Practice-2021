using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    public List<Room> Map;
    public int seed = 421;
    public int MapRadius = 15;
    public int NumberOfRooms = 10;
    public int maxRoomSize = 16; //Max Room width will actually be this + 1.
    public int separation = 3;

    public void Initialize() {
        //Generate a List of Randomly Sized Rooms Randomly "Placed" inside a given radius
        Random.InitState(421); //Give seed to RNG
        Map = new List<Room>();

        for (int i = 0; i < NumberOfRooms; i++)
        {
            int rX, rY; //The x and y components of the random point in the circle of radius MapRadius
            rX = rY = 0;
            Vector2 origin = Random.insideUnitCircle;
            rX = Mathf.RoundToInt(origin.x * MapRadius);
            rY = Mathf.RoundToInt(origin.y * MapRadius);

            int rW, rH;
            rW = rH = 0;
            rW = Random.Range(maxRoomSize / 5, maxRoomSize / 2);
            rH = Random.Range(maxRoomSize / 5, maxRoomSize / 2);

            Map.Add(new Room( i, rX, rY, rW, rH));
        }
    }

    public void Relax() { 
        //Steer each room away from overlapping rooms
        //Continue until all rooms are separated by some given minimum distance.

        //Forevery room in the map, check it for overlapping with every other room
        //if they overlap determine which direction the primary room needs to move away from the compared room (A.X - B.X , A.Y - B.Y).Normalized
    }

    bool CheckOverlap(Room A, Room B) {
        //Determine if two given rooms are overlapping (Ignores Separation Variable)
        //Create two lists of vector 2's that represent all the tiles of a room
        //Compare the two lists, if any given Vector2 is in both then they overlap
        bool check = false;
        List<Vector2> roomA = GetRoomTiles(A, separation);
        List<Vector2> roomB = GetRoomTiles(B);

        foreach (Vector2 a in roomA) {
            foreach (Vector2 b in roomB) {
                if (a == b) {
                    check = true;
                    break;
                }
            }
            if (check) break;
        }

        return check;
    }

    List<Vector2> GetRoomTiles(Room room, int padding = 0) {
        List<Vector2> tiles = new List<Vector2>();

        for (int i = -room.Width - padding; i <= room.Width + padding; i++)
        {
            for (int j = -room.Height - padding; j < room.Height + padding; j++)
            {
                tiles.Add(new Vector2(i + room.X, j + room.Y));
            }
        }

        return tiles;
    }


    [CustomEditor(typeof(MapGenerator))]
    public class MapGeneratorEditorExtension : Editor
    {
        public override void OnInspectorGUI()
        {
            MapGenerator script = (MapGenerator)target;
            if (GUILayout.Button("Make Map"))
            {
                script.Initialize();
            }
            DrawDefaultInspector();
        }
    }
}
