using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum BrushState
{
    CLIFFS,
    TEXTURE,
    VERTEX,
    OBJECTS
}

public delegate void ChangedBrushState(BrushState newState);

public class Brush : MonoBehaviour {

    public BrushState brushState = BrushState.TEXTURE;
    public Slider brushSizeSlider;

    public Toggle[] brushToggles;

    public static Brush main;

    private int brushSize = 1;

    private int previousBrushSize = 1;

    public GameObject[] objectPool;

    public GameObject selectedGameObject;

    public event ChangedBrushState ChangedBrushState;

    public int GetBrushSize()
    {
        return brushSize;
    }

    public void SetBrushSize()
    {
        Debug.Log(brushSizeSlider.value);
        previousBrushSize = brushSize;
        brushSize = (int)brushSizeSlider.value;
    }

    public int GetPreviousBrushSize()
    {
        return previousBrushSize;
    }

	// Use this for initialization
	void Start () {
        SetListeners();
        main = this;
        selectedGameObject = objectPool[0];
	}

    void SetListeners()
    {
        //brushToggle.GetComponent<Button>().onClick.AddListener(delegate { SetState(); });
        for(int i = 0; i < brushToggles.Length; i++)
        {
            Toggle toggle = brushToggles[i];
            toggle.onValueChanged.AddListener(delegate { SetState(toggle); });
        }
        brushSizeSlider.onValueChanged.AddListener(delegate { SetBrushSize(); });
    }

    void SetState(Toggle toggle)
    {
        if(toggle.isOn)
        {
            brushState = (BrushState)Array.IndexOf(brushToggles, toggle);
            ChangedBrushState(brushState);
            Debug.Log(brushState);
        }
    }
}
