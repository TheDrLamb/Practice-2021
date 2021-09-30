using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAIDirector_Room : MonoBehaviour
{
    //Attributes
    public Transform[] spawnPoints;
    public bool playerOccupied;

    //Tags
    public const string playerTag = "Player";
    public const string enemyTag = "Enemy";

    //Agents
    public List<GameObject> enemies { get; protected set; }
    public List<GameObject> players { get; protected set; }

    private void Awake()
    {
        enemies = new List<GameObject>();
        players = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case playerTag:
                players.Add(other.gameObject);
                playerOccupied = players.Count > 0;
                break;
            case enemyTag:
                enemies.Add(other.gameObject);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (other.tag)
        {
            case playerTag:
                players.Remove(other.gameObject);
                playerOccupied = players.Count > 0;
                break;
            case enemyTag:
                enemies.Remove(other.gameObject);
                break;
        }
    }
}
