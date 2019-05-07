using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapEditorState
{
    MAIN_MENU,
    EDITING,
    SAVING,
    LOADING
}

public class MapEditorManager : MonoBehaviour {

    public static MapEditorManager main;

    protected MapEditorState state;
    protected CreateMapInfo mapInfo;

    public GameObject editorPanel;
    public GameObject mainMenuPanel;
    public TGMap tgMap;

    private int chunks = 4;
    public GameObject mapChunk;
    private List<GenerateMesh> mapChunks;

    public VertexType vType = VertexType.ONE;


    // Use this for initialization
    void Start () {
        //Application.targetFrameRate = 60;
        main = this;
        SetState(MapEditorState.MAIN_MENU);

        mapInfo = GetComponent<CreateMapInfo>();
        mapChunks = new List<GenerateMesh>();
	}

    public MapEditorState GetState()
    {
        return state;
    }

    public void SetState(MapEditorState state)
    {
        this.state = state;
        ChangeGUI();
    }

    void ChangeGUI()
    {
        switch(state)
        {
            case MapEditorState.MAIN_MENU:
                editorPanel.SetActive(false);
                mainMenuPanel.SetActive(true);
                break;
            case MapEditorState.EDITING:
                editorPanel.SetActive(true);
                mainMenuPanel.SetActive(false);
                break;
            default:
                editorPanel.SetActive(false);
                mainMenuPanel.SetActive(true);
                break;
        }
    }

    public void PrepareNewMap(TileMapDTO tileMapDto)
    {
        MainCameraTest camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MainCameraTest>();
        camera.SetCameraBounds(tileMapDto.GetSizeX(), tileMapDto.GetSizeZ());

        int chunks = tileMapDto.GetSizeX() / 32;
        int mapSizeZ = tileMapDto.GetSizeZ() / chunks;
        int mapSizeX = tileMapDto.GetSizeX() / chunks;
        tileMapDto.SetSizeX(mapSizeX);
        tileMapDto.SetSizeZ(mapSizeZ);
        for (int z = 0; z < chunks; z++)
        {
            for (int x = 0; x < chunks; x++)
            {
                GameObject tempObject = (GameObject)Instantiate(mapChunk, new Vector3(x * mapSizeX, 0, z * mapSizeZ), Quaternion.identity);
                tempObject.GetComponent<GenerateMesh>().CreateMesh(tileMapDto, x * mapSizeX, z * mapSizeZ);
                mapChunks.Add(tempObject.GetComponent<GenerateMesh>());
            }
        }

            //GameObject tgMap = Instantiate(Resources.Load("MapEditor/TileMap")) as GameObject;
        SetState(MapEditorState.EDITING);
        //tgMap.CreateNewTerrain(tileMapDto);
        ChangeTextureBrush();

    }

    public void ChangeTextureBrush()
    {
        GameObject bType = GameObject.Find("Toggle (2)");
        Vector3 bTypePos = bType.transform.position;

        for (int i = 0; i < tgMap.textureSprites.Length; i++)
        {
            GameObject selection = Instantiate(Resources.Load("MapEditor/TextureTileSet")) as GameObject;
            selection.transform.SetParent(GameObject.Find("TileSetHolder").transform, false);
            Vector3 pos = new Vector3(bTypePos.x, bTypePos.y - bType.GetComponent<RectTransform>().rect.height, bTypePos.z);
            int copy = i;
            selection.GetComponent<Button>().onClick.AddListener(delegate { SetBrush(copy); });
            selection.GetComponentInChildren<Text>().text = i.ToString();
        }
    }

    public void Save()
    {
        print(mapInfo.GetMapName());
        print(mapInfo.GetMapSizeX());
        print(mapInfo.GetMapSizeZ());
        foreach(GenerateMesh mapChunk in mapChunks)
        {
            print(mapChunk);
        }
    }

    void SetBrush(int index)
    {
        vType = (VertexType)index;
    }
}
