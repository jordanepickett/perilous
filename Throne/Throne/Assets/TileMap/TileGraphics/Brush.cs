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
    public Slider brushSizeSlider;
    public static Brush main;

    private int brushSize;

    public int GetBrushSize()
    {
        return brushSize;
    }

    public void SetBrushSize()
    {
        Debug.Log(brushSizeSlider.value);
        brushSize = (int)brushSizeSlider.value;
    }

	// Use this for initialization
	void Start () {
        SetListeners();
        main = this;
	}

    void SetListeners()
    {
        brushToggle.GetComponent<Button>().onClick.AddListener(delegate { SetState(); });
        brushSizeSlider.onValueChanged.AddListener(delegate { SetBrushSize(); });
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
