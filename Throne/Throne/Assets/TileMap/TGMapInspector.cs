using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TGMap))]
public class TGMapInspector : Editor {

	public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        if(GUILayout.Button("Regenerate"))
        {
            TGMap tileMap = (TGMap)target;
            tileMap.BuildNewMesh();
        }
    }
}
