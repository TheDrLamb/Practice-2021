
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyAIDirector : MonoBehaviour
{
    List<EnemyAIDirector_Room> rooms;

    private void Start()
    {
        rooms = GetComponentsInChildren<EnemyAIDirector_Room>().ToList();
    }

    private void Update()
    {
        foreach(EnemyAIDirector_Room room in rooms)
        {
            //Simply Assign the enemies to the first player in the room.
            if (room.playerOccupied)
            {
                foreach(GameObject enemy in room.enemies)
                {
                    EnemyInputController enemyInput = enemy.GetComponent<EnemyInputController>();
                    if(enemyInput.target == null)
                    {
                        enemyInput.alerted = true;
                        enemyInput.target = room.players[0].transform;
                    }
                }
            }
        }
    }
}
