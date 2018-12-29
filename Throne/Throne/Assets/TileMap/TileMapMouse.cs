using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TGMap))]
public class TileMapMouse : MonoBehaviour {

    TGMap map;

    Collider collider;
    Renderer renderer;
    TGMap tileMap;

    Vector3 currentTileCoord;
    public Transform selectionCube;
    public Brush brush;


    private void Start()
    {
        map = GetComponent<TGMap>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        tileMap = GetComponent<TGMap>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (collider.Raycast(ray, out hit, Mathf.Infinity))
        {
            int x = Mathf.FloorToInt(hit.point.x / tileMap.tileSize);
            int z = Mathf.FloorToInt(hit.point.z / tileMap.tileSize);
            int y = Mathf.CeilToInt(hit.point.y);
            Debug.Log(hit.point);
            //Debug.Log("Tile: " + x + ", " + z);

            currentTileCoord.x = x;
            currentTileCoord.z = z;
            //currentTileCoord.y = y;

            selectionCube.transform.position = currentTileCoord * 1;

            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if(brush.brushState == BrushState.CLIFFS)
                { 
                    BrushCliff();
                }
                else
                {
                    BrushTexture();
                }
            }
        }
    }

    void BrushTexture()
    {
        List<Vector3> test = map.NearestVertecesTo(currentTileCoord);
        map.AddRandomTextureList(test);
    }

    void BrushCliff()
    {
        Debug.Log(currentTileCoord);
        int index = map.NearestVertexIndexTo(currentTileCoord);
        map.ModifyVertices(currentTileCoord);
    }
}
