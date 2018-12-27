using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersContainer : MonoBehaviour {

    public static List<GameObject> players = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPlayer(GameObject player)
    {
        players.Add(player);
    }

    public void RemovePlayer(GameObject player)
    {
        players.Remove(player);
    }

    public static List<GameObject> GetPlayers()
    {
        return players;
    }
}
