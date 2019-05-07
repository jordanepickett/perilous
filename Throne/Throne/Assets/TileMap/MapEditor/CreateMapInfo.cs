using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateMapInfo : MonoBehaviour {

    public InputField mapName;

    public InputField sizeX;

    public InputField sizeZ;

    public Button CreateNewMap;

    public string GetMapName()
    {
        return mapName.text;
    }

    public string GetMapSizeX()
    {
        return sizeX.text;
    }

    public string GetMapSizeZ()
    {
        return sizeZ.text;
    }

    private void Start()
    {
        SetListeners();
    }

    public TileMapDTO GetNewMapInfo()
    {
        string name = mapName.text;
        int sizeX = int.Parse(this.sizeX.text);
        int sizeZ = int.Parse(this.sizeZ.text);

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
