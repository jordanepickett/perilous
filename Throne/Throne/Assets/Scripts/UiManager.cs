using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public GameObject prefab;

    public int numberToCreate;

	// Use this for initialization
	void Start () {
        Populate();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Populate()
    {
        GameObject newObj;

        for(int i = 0; i < numberToCreate; i++)
        {
            newObj = (GameObject)Instantiate(prefab, transform);
            newObj.GetComponent<Image>().color = Random.ColorHSV();
        }
    }
}
