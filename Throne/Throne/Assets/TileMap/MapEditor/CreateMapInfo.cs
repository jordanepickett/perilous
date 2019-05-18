using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMapInfo : MonoBehaviour {

    public InputField mapName;

    public Dropdown sizeX;

    public Dropdown sizeZ;

    public Button CreateNewMap;

    public string GetMapName()
    {
        return mapName.text;
    }

    public string GetMapSizeX()
    {
        return sizeX.options[sizeX.value].text;
    }

    public string GetMapSizeZ()
    {
        return sizeZ.options[sizeZ.value].text;
    }

    private void Start()
    {
        SetListeners();
    }

    public TileMapDTO GetNewMapInfo()
    {
        string name = mapName.text;
        int sizeX = int.Parse(this.sizeX.options[this.sizeX.value].text);
        int sizeZ = int.Parse(this.sizeZ.options[this.sizeZ.value].text);

        return new TileMapDTO(name, sizeX, sizeZ);
    }

    void SetListeners()
    {
        CreateNewMap.onClick.AddListener(delegate { PrepareNewMap(); });
    }

    void PrepareNewMap()
    {
        MapEditorManager.main.PrepareNewMap(GetNewMapInfo());
    }
}
