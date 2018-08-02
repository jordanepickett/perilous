using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;

public class GameManager : NetworkBehaviour
{
    public Dictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();

    public List<Faction> factions = new List<Faction>();

    public static GameManager main;

    void Awake()
    {
        var playerFaction = gameObject.AddComponent<FactionA>();
        factions.Add(playerFaction);
        main = this;
        DontDestroyOnLoad(gameObject);
    }

    public Dictionary<int, PlayerController> AddPlayer(int id, PlayerController player)
    {
        players.Add(id, player);

        return players;
    }

    public PlayerController GetPlayer(int id)
    {
        return players[id];
    }
}

