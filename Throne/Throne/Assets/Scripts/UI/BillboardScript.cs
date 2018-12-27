using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardScript : MonoBehaviour {

    public Camera gCamera;

    GameObject localPlayer;

	// Use this for initialization
	void Start () {
        //gCamera = GetComponent<RtsObjectController>().GetPlayer().GetComponent<PlayerController>().gCamera;
	}
	
	// Update is called once per frame
	void Update () {
        if(localPlayer == null)
        {
            var players = PlayersContainer.GetPlayers();
            foreach (var player in players)
            {
                if (player && player.GetComponent<PlayerController>().isLocalPlayer)
                {
                    localPlayer = player;
                    gCamera = localPlayer.GetComponent<PlayerController>().gCamera;
                }
                else
                {
                    Debug.Log("NOT LOCAL PLAYER");
                }
            }
        }
        if (gCamera)
        {
            transform.LookAt(transform.position + gCamera.transform.rotation * Vector3.back, gCamera.transform.rotation * Vector3.up);
        }
	}
}
