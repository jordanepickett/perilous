using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barebones.MasterServer;
using UnityEngine.UI;

public class DisplaySpawnerStatus : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
        text = gameObject.GetComponent<Text>();
        Msf.Server.Spawners.SpawnerRegistered += OnSpawnerRegistered;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnSpawnerRegistered(SpawnerController obj)
    {
        text.text = "Started Spawner";
    }

    void OnDestroy()
    {
        Msf.Server.Spawners.SpawnerRegistered -= OnSpawnerRegistered;
    }
}
