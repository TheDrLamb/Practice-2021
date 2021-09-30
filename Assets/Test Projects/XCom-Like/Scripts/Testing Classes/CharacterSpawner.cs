using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Temporary Class to add a Character to the map
public class CharacterSpawner : MonoBehaviour
{
    public GameObject unit;
    public GridMapController gridMap;
    public PlayerController player;

    void Spawn() {
        //Add a unit to 0,0 and assign their Cell to their Unit data
        //Center the camera on the Unit
        GameObject newUnit = Instantiate(unit);
        newUnit.GetComponent<UnitController>().unit = new Unit(2, 3);
        newUnit.GetComponent<UnitController>().SetTile(gridMap.GetTile(0, 0));
        newUnit.GetComponentInChildren<UnitUIController>().unitController = newUnit.GetComponent<UnitController>();
        newUnit.transform.parent = gridMap.transform;
        newUnit.transform.localPosition = new Vector3(0.5f, 0.0f, 0.5f);
        player.SetUnit(newUnit.GetComponent<UnitController>());
    }

    [CustomEditor(typeof(CharacterSpawner))]
    public class MapGeneratorEditorExtension : Editor
    {
        public override void OnInspectorGUI()
        {
            CharacterSpawner script = (CharacterSpawner)target;
            if (GUILayout.Button("Spawn Character"))
            {
                script.Spawn();
            }
            DrawDefaultInspector();
        }
    }
}
