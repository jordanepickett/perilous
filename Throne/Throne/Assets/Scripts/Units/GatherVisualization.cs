using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherVisualization : MonoBehaviour {

    Renderer renderer;
    MeshRenderer mRenderer;

    public Color gold;
    public Color lumber;
	// Use this for initialization
	void Start () {
        gameObject.SetActive(true);
        renderer = GetComponent<Renderer>();
        mRenderer = GetComponent<MeshRenderer>();
        mRenderer.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetActive(GatherType type)
    {
        mRenderer.enabled = true;
        if(type == GatherType.GOLD)
        {
            renderer.material.SetColor("_Color", gold);
            Debug.Log("HERE");
        }
        else
        {
            renderer.material.SetColor("_Color", lumber);
        }
    }

    public void SetDeactive()
    {
        mRenderer.enabled = false;
    }
}
