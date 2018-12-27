using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum BrushState
{
    CLIFFS,
    TEXTURE
}

public class Brush : MonoBehaviour {

    public BrushState brushState = BrushState.TEXTURE;
    public Button brushToggle;

	// Use this for initialization
	void Start () {
        SetListener();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SetListener()
    {
        brushToggle.GetComponent<Button>().onClick.AddListener(delegate { SetState(); });
    }

    void SetState()
    {
        if(brushState == BrushState.TEXTURE)
        {
            brushState = BrushState.CLIFFS;
            brushToggle.GetComponentInChildren<Text>().text = "Texture";
        }
        else
        {
            brushState = BrushState.TEXTURE;
            brushToggle.GetComponentInChildren<Text>().text = "Cliffs";
        }
    }
}
