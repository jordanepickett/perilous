using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileMapMouse : MonoBehaviour {

    public TGMap map;

    Collider collider;
    Renderer renderer;
    TGMap tileMap;

    Vector3 currentTileCoord;
    public Transform selectionCube;
    public Brush brush;
    List<ModifableInterface> maps;
    Vector3 test;

    private GameObject objectToBePlaced;

    public static bool newSession = true;

    public float force = 10f;
    public float forceOffset = 0.1f;


    private void Start()
    {
        //map = GetComponent<TGMap>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        tileMap = GetComponent<TGMap>();
        maps = new List<ModifableInterface>();

        Brush.main.ChangedBrushState += OnChangedBrushState;
        Brush.main.ChangedCliffLevel += OnChangedCliffLevelState;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        ScrollMouse();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            int x = Mathf.RoundToInt(hit.point.x / 1);
            int z = Mathf.RoundToInt(hit.point.z / 1);
            float y = hit.point.y;

            currentTileCoord.x = x;
            currentTileCoord.z = z;
            //print("X POS: " + currentTileCoord.x + " Z POS: " + currentTileCoord.z);

            FirstClick(hit, y);
            //Debug.Log(currentTileCoord);

            selectionCube.transform.position = new Vector3(currentTileCoord.x, y, currentTileCoord.z) * 1;

            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Brush.main.brushState == BrushState.VERTEX || Brush.main.brushState == BrushState.CLIFFS || Brush.main.brushState == BrushState.TEXTURE)
                {
                    //maps.Clear();
                    //SetMaps();
                }
                switch (brush.brushState)
                {
                    case (BrushState.CLIFFS):
                        BrushCliff();
                        break;
                    case (BrushState.TEXTURE):
                        if (Vector3.Distance(test, currentTileCoord) >= 1f)
                        {
                            BrushTexture();
                            test = currentTileCoord;
                        }
                        break;
                    case (BrushState.VERTEX):
                        VertexModifier();
                        //print(hit.point);
                        //print(test);
                        //Vector3 point = hit.point;
                        //point += hit.normal * forceOffset;
                        //map.DeformMesh(point, force);
                        break;
                    case (BrushState.OBJECTS):
                        ObjectPlacement();
                        break;
                    case (BrushState.RAMPS):
                        PlaceRamp();
                        break;
                    default:
                        BrushTexture();
                        break;
                }
            }

            if (Brush.main.brushState == BrushState.OBJECTS && objectToBePlaced != null)
            {
                RaycastHit hitPlace;
                Ray rayHit = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (map.GetComponent<Collider>().Raycast(rayHit, out hitPlace, 2.0f * 10))
                {
                    Debug.Log("Hit point: " + hitPlace.point);
                    objectToBePlaced.transform.position = new Vector3(
                        Mathf.Round(hitPlace.point.x), hitPlace.point.y + 0.3f, Mathf.Round(hitPlace.point.z));

                }
            }

            if(Input.GetMouseButtonUp(0))
            {
                if(Brush.main.brushState != BrushState.TEXTURE)
                {
                    map.ApplyMeshCollider();
                }
            }
        }
    }

    void OnChangedBrushState(BrushState state)
    {
        if(state == BrushState.OBJECTS)
        {
            objectToBePlaced = Instantiate(Brush.main.selectedGameObject);
        }
        else
        {
            objectToBePlaced = null;
        }
    }

    void OnChangedCliffLevelState(CliffLevel state)
    {

    }

    void BrushTexture()
    {
        int radius = Brush.main.GetBrushSize();
        Vector3 startPoint = new Vector3(currentTileCoord.x - radius, currentTileCoord.y, currentTileCoord.z - radius);
        List<Vector3> vectorList = new List<Vector3>();
        for (int z = 0; z < radius; z++)
        {
            for (int x = 0; x < radius; x++)
            {
                Vector3 point = new Vector3(currentTileCoord.x + x, currentTileCoord.y, currentTileCoord.z + z);
                SetMaps(point);
                vectorList.Add(point);
            }
        }

        foreach (var map in maps)
        {
            map.AddTextureToTerrain(vectorList, MapEditorManager.main.vType);
        }
    }

    void BrushCliff()
    {
        int radius = Brush.main.GetBrushSize();
        Vector3 startPoint = new Vector3(currentTileCoord.x - radius, currentTileCoord.y, currentTileCoord.z - radius);
        for (int z = 0; z < radius; z++)
        {
            for (int x = 0; x < radius; x++)
            {
                Vector3 point = new Vector3(currentTileCoord.x + x, currentTileCoord.y, currentTileCoord.z + z);
                SetMaps(point);
                foreach (var map in maps)
                {
                    map.AddCliff(point, CliffHeight());
                }
            }
        }
    }

    void PlaceRamp()
    {
        map.AddRamp(new Vector3(currentTileCoord.x, 0, currentTileCoord.z));
    }

    int CliffHeight()
    {
        //int height = 1;
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    height = -1;
        //}

        //return height;
        int level = (int)Brush.main.cliffLevel;
        return level;
    }

    void VertexModifier()
    {
        bool lowerHeight = false;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            lowerHeight = true;
        }
        int radius = Brush.main.GetBrushSize();
        Vector3 startPoint = new Vector3(currentTileCoord.x - radius, currentTileCoord.y, currentTileCoord.z - radius);
        for(int z = 0; z < radius; z++)
        {
            for(int x = 0; x < radius; x++)
            {
                Vector3 point = new Vector3(currentTileCoord.x + x, currentTileCoord.y, currentTileCoord.z + z);
                SetMaps(point);
                foreach (var map in maps)
                {
                    map.ModifyVerteces(point, lowerHeight);
                }
            }
        }
    }

    void ObjectPlacement()
    {
        if (objectToBePlaced != null)
        {
            // Ray ray = GetComponent<EventsManager>().gameCamera.ScreenPointToRay(Input.mousePosition);
            //Plane plane = new Plane(Vector3.up, new Vector3(0,1.5f,0));

            //    RaycastHit hitInfo;
            //    if (Physics.Raycast(ray, out hitInfo))
            //    {
            //        //Vector3 point = ray.GetPoint(rayDistance);
            //        var terrainHeight = Terrain.activeTerrain.SampleHeight(hitInfo.point);
            //        objectWaiting.transform.position = new Vector3(
            //            Mathf.Round(hitInfo.point.x), terrainHeight + 0.3f, Mathf.Round(hitInfo.point.z));
            //    }
            //}
            GameObject setObject = objectToBePlaced;
            Instantiate(setObject);

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (map.GetComponent<Collider>().Raycast(ray, out hit, 2.0f * 10))
            {
                Debug.Log("Hit point: " + hit.point);
                setObject.transform.position = new Vector3(
                    Mathf.Round(hit.point.x), hit.point.y + 0.3f, Mathf.Round(hit.point.z));

            }
        }
    }

    void SetMaps(Vector3 point)
    {
        maps.Clear();
        Vector3 center = point;
        Collider[] hitColliders = Physics.OverlapSphere(center, 0.9f);
        int i = 0;
        for (i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].GetComponent<ModifableInterface>() != null)
            {
                maps.Add((hitColliders[i].GetComponent<ModifableInterface>()));
            }

            if (hitColliders[i].GetComponentInChildren<ModifableInterface>() != null)
            {
                //maps.Add((hitColliders[i].GetComponentInChildren<ModifableInterface>()));
            }
        }
    }

    void ScrollMouse()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - .3f, transform.position.z + .3f);
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z - .3f);
        }
    }

    void FirstClick(RaycastHit hit, float y)
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (hit.collider.gameObject.GetComponent<TGMap>())
            {
                map = hit.collider.gameObject.GetComponent<TGMap>();
            }
            currentTileCoord.y = y;

            test = currentTileCoord;
            if (Brush.main.brushState == BrushState.TEXTURE)
            {
                BrushTexture();
            }
        }
    }
}
