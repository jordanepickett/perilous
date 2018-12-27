using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SaveTileMap : MonoBehaviour {

    TGMap map;

	// Use this for initialization
	void Start () {
        map = GetComponent<TGMap>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SaveMap()
    {
        AssetDatabase.CreateAsset(map.mesh, "Resources");
        AssetDatabase.SaveAssets();
    }
}
