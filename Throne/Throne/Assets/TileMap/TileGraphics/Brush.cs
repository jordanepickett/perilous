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
    OBJECTS,
    RAMPS
}
public enum CliffLevel
{
    FLATTEN,
    LEVEL_ONE,
    LEVEL_TWO
}

public delegate void ChangedBrushState(BrushState newState);
public delegate void ChangedCliffLevel(CliffLevel newState);

public class Brush : MonoBehaviour {

    public BrushState brushState = BrushState.TEXTURE;

    public CliffLevel cliffLevel = CliffLevel.LEVEL_ONE;

    public Slider brushSizeSlider;

    public Toggle[] brushToggles;

    public Toggle[] cliffToggles;

    public static Brush main;

    private int brushSize = 1;

    private int previousBrushSize = 1;

    public GameObject[] objectPool;

    public GameObject selectedGameObject;

    public event ChangedBrushState ChangedBrushState;

    public event ChangedCliffLevel ChangedCliffLevel;

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
        SetBrushToggleListeners();
        SetCliffLevelListeners();
        brushSizeSlider.onValueChanged.AddListener(delegate { SetBrushSize(); });
    }

    void SetBrushToggleListeners()
    {
        for (int i = 0; i < brushToggles.Length; i++)
        {
            Toggle toggle = brushToggles[i];
            toggle.onValueChanged.AddListener(delegate { SetState(toggle); });
        }
    }

    void SetCliffLevelListeners()
    {
        for (int i = 0; i < cliffToggles.Length; i++)
        {
            Toggle toggle = cliffToggles[i];
            toggle.onValueChanged.AddListener(delegate { SetCliffLevel(toggle); });
        }
    }

    void SetCliffLevel(Toggle toggle)
    {
        if (toggle.isOn)
        {
            cliffLevel = (CliffLevel)Array.IndexOf(cliffToggles, toggle);
            ChangedCliffLevel(cliffLevel);
            Debug.Log(cliffLevel);
        }
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
