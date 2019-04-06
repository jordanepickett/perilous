using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TGMap : MonoBehaviour, ModifableInterface {

    public const int TILE_RESOLUTION = 64;

    public float tileSize = 1.0f;

    public Texture2D[] textureSprites;
    protected TDMap map;
    protected TDVertexMap vertexMap;
    protected Vector3[] verts;
    protected Vector3[][] splitVerts;

    protected int textWidth;
    protected int textHeight;
    protected Texture2D texture;

    Color[][] tiles;
    Color[][] grass;
    protected Color[][][] spriteTiles;

    //public VertexType vType = VertexType.ONE;

    public Mesh mesh;
    public Mesh workingMesh;
    public MeshCollider meshCollider;
    protected Vector3[] normals;

    protected int sizeX;
    protected int sizeZ;

    public void CreateNewTerrain(TileMapDTO tileMapDto)
    {
        CreateTerrainTexture(tileMapDto);

        CreateSpriteTiles();
        BuildNewMesh(tileMapDto);
        ChangeTextureBrush();
    }

    protected void CreateSpriteTiles()
    {
        spriteTiles = new Color[textureSprites.Length][][];

        for (int i = 0; i < textureSprites.Length; i++)
        {
            spriteTiles[i] = ChopUpTiles(textureSprites[i]);
        }
    }

    protected void CreateTerrainTexture(TileMapDTO tileMapDTO)
    {
        textWidth = tileMapDTO.GetSizeX() * TILE_RESOLUTION;
        textHeight = tileMapDTO.GetSizeZ() * TILE_RESOLUTION;
        texture = new Texture2D(textWidth, textHeight);
    }

    public void BuildNewMesh(TileMapDTO tileMapDTO)
    {
        sizeX = tileMapDTO.GetSizeX();
        sizeZ = tileMapDTO.GetSizeZ();

        map = new TDMap(sizeX, sizeZ, 1, 1);

        int numTiles = sizeX * sizeZ;
        //int numTris = numTiles * 6;
        int numTris = numTiles * 2;

        int vSizeX = sizeX + 1;
        int vSizeZ = sizeZ + 1;
        //int numVerts = vSizeX * vSizeZ * 4;
        int numVerts = vSizeX * vSizeZ;

        // Generate the mesh data
        Vector3[] vertices = new Vector3[numVerts];
        normals = new Vector3[numVerts];
        Vector2[] Uvs = new Vector2[numVerts];

        vertexMap = new TDVertexMap(numVerts);

        for (int p = 0; p < vertices.Length; p++)
        {
            vertexMap.GetVertex(p).type = VertexType.ONE;
        }

        int[] triangles = new int[numTris * 3];

        int x, z, iVertCount = 0;
        for (z = 0; z < vSizeZ; z++)
        {
            for (x = 0; x < vSizeX; x++)
            {
                //vertices[iVertCount + 0] = new Vector3(x, 0, z);
                //vertices[iVertCount + 1] = new Vector3(x + 1, 0, z);
                //vertices[iVertCount + 2] = new Vector3(x + 1, 0, z + 1);
                //vertices[iVertCount + 3] = new Vector3(x, 0, z + 1);

                //normals[iVertCount + 0] = Vector3.up;
                //normals[iVertCount + 1] = Vector3.up;
                //normals[iVertCount + 2] = Vector3.up;
                //normals[iVertCount + 3] = Vector3.up;
                //iVertCount += 4;
                vertices[z * vSizeX + x] = new Vector3(x * tileSize, 0, z * tileSize);
                //vertexMap.GetVertex(z * vSizeX + x).SetVertices(x * tileSize, 0, z * tileSize);
                TDTile tile = map.GetTile(x, z);
                if(tile != null)
                {
                    //tile.vertices[TDTile.BOT_LEFT_VERTEX].SetVertices(x * tileSize, 0, z * tileSize);
                    //tile.vertices[TDTile.BOT_RIGHT_VERTEX].SetVertices(x * tileSize + 1, 0, z * tileSize);
                    //tile.vertices[TDTile.TOP_RIGHT_VERTEX].SetVertices(x * tileSize + 1, 0, z * tileSize + 1);
                    //tile.vertices[TDTile.TOP_LEFT_VERTEX].SetVertices(x * tileSize, 0, z * tileSize + 1);
                }
                normals[z * vSizeX + x] = Vector3.up;
                Uvs[z * vSizeX + x] = new Vector2((float)x / sizeX, (float)z / sizeZ);
            }
        }

        for(z = 0; z < sizeZ; z++)
        {
            for(x = 0; x < sizeX; x++)
            {
                int squareIndex = z * sizeX + x;
                int triOffset = squareIndex * 6;
                triangles[triOffset + 0] = z * vSizeX + x + 0;
                triangles[triOffset + 1] = z * vSizeX + x + vSizeX + 0;
                triangles[triOffset + 2] = z * vSizeX + x + vSizeX + 1;

                triangles[triOffset + 3] = z * vSizeX + x + 0;
                triangles[triOffset + 4] = z * vSizeX + x + vSizeX + 1;
                triangles[triOffset + 5] = z * vSizeX + x + 1;
            }
        }

        //int iIndexCount = 0; iVertCount = 0;
        //for (int i = 0; i < numTiles; i++)
        //{
        //    triangles[iIndexCount + 0] += (iVertCount + 0);
        //    triangles[iIndexCount + 2] += (iVertCount + 1);
        //    triangles[iIndexCount + 1] += (iVertCount + 2);

        //    triangles[iIndexCount + 3] += (iVertCount + 0);
        //    triangles[iIndexCount + 5] += (iVertCount + 2);
        //    triangles[iIndexCount + 4] += (iVertCount + 3);

        //    iVertCount += 4; iIndexCount += 6;
        //}


        // Create a new Mesh and populate with the data
        mesh = new Mesh();
        mesh.vertices = vertices;
        verts = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        //for (int i = 0; i < Uvs.Length; i++)
        //{
        //    Uvs[i] = new Vector2(vertices[i].x / sizeX, vertices[i].z / sizeZ);
        //}

        mesh.uv = Uvs;


        // Assign our mesh to our filter/renderer/collider
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        //SplitVerts();
        BuildTexture(tileMapDTO);
    }

    public void BuildTexture(TileMapDTO tileMapDTO)
    {

        //FIXME
        int sizeY = tileMapDTO.GetSizeZ();
        int sizeX = tileMapDTO.GetSizeX();
        for (int y = 0; y < sizeY; y++)
        {

            for(int x = 0; x < sizeX; x++)
            {
                Color[] colorArray = spriteTiles[0][0];

                texture.filterMode = FilterMode.Bilinear;

                texture.SetPixels(x * TILE_RESOLUTION, y * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION, colorArray);
            }
        }

        texture.Apply();

        SetTerrainTexture(texture);
    }

    void SetTerrainTexture(Texture texture)
    {
        //Find the Standard Shader
        Material myNewMaterial = new Material(Shader.Find("Standard"));
        //Set Texture on the material
        myNewMaterial.SetFloat("_Mode", 1);
        myNewMaterial.SetTexture("_MainTex", texture);
        myNewMaterial.SetFloat("_Glossiness", .0f);
        myNewMaterial.SetFloat("_Cutoff", .7f);
        myNewMaterial.SetFloat("_Mode", 2);
        myNewMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        myNewMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        myNewMaterial.SetInt("_ZWrite", 0);
        myNewMaterial.DisableKeyword("_ALPHATEST_ON");
        myNewMaterial.EnableKeyword("_ALPHABLEND_ON");
        myNewMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        myNewMaterial.renderQueue = 3000;
        //Apply to GameObject
        GetComponent<MeshRenderer>().material = myNewMaterial;
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.sharedMaterial.mainTexture = texture;
    }

    public Color[] AlphaBlend(Color[] firstColors, Color[] secondColors, float alpha = 1)
    {
        int count = firstColors.Length;
        Color[] srcColor = new Color[count];
        for(int i = 0; i < count; i++)
        {
            Color a = firstColors[i];
            Color b = secondColors[i];
            float srcF = b.a;
            float destF = 1f - srcF;
            //float alpha = srcF + destF * a.a;
            //float alpha = 1;
            Color r = (b * srcF + a * a.a * destF) / alpha;
            r.a = alpha;
            srcColor[i] = r;
        }

        return srcColor;
    }

    Color[][] ChopUpTiles(Texture2D terrainTiles)
    {
        int numTilesPerRow = terrainTiles.width / TILE_RESOLUTION;
        int numRows = terrainTiles.height / TILE_RESOLUTION;

        Color[][] tiles = new Color[numTilesPerRow * numRows][];

        for(int y = 0; y < numRows; y++)
        {
            for(int x = 0; x < numTilesPerRow; x++)
            {
                tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels(x * TILE_RESOLUTION, ((numRows - 1) - y) * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION);
            }
        }

        return tiles;
    }

    bool CheckTileVerticyTypes(TDTile[] tilesArray)
    {
        if ((tilesArray[0] != null && tilesArray[0].vertices[TDTile.TOP_RIGHT_VERTEX].type != MapEditorManager.main.vType) ||
            (tilesArray[1] != null && tilesArray[1].vertices[TDTile.TOP_LEFT_VERTEX].type != MapEditorManager.main.vType) ||
            (tilesArray[2] != null && tilesArray[2].vertices[TDTile.BOT_LEFT_VERTEX].type != MapEditorManager.main.vType) ||
            (tilesArray[3] != null && tilesArray[3].vertices[TDTile.BOT_RIGHT_VERTEX].type != MapEditorManager.main.vType))
        {
            return true;
        }
        return false;
    }

    public void AddTextureToTerrain(List<Vector3> vList, float alpha = 1)
    {
        foreach(var t in vList)
        {
            Vector3 vertex = t;
            int xPos = (int)vertex.x;
            int zPos = (int)vertex.z;

            TDTile[] tilesArray = GetVertexTiles(xPos, zPos);
            int[,] tilePos = GetTilePositions(xPos, zPos);

            if (CheckTileVerticyTypes(tilesArray))
            {
                SetTileVertexType(tilesArray);

                for (int i = 0; i < tilesArray.Length; i++)
                {
                    TDTile tile = tilesArray[i];
                    if (tile == null || tile.type != TileType.GROUND)
                    {
                        return;
                    }
                    List<VertexType> vTypes = tile.GetVertexTypes();
                    Color[][] cArray = new Color[vTypes.Count][];
                    
                    for (int x = 0; x < vTypes.Count; x++)
                    {
                        if (x == 0)
                        {
                            cArray[x] = spriteTiles[(int)vTypes[x]][0];
                        }
                        else
                        {
                            cArray[x] = spriteTiles[(int)vTypes[x]][SpriteIndex(vTypes[x], tile.vertices)];
                        }
                    }

                    Color[] colors = BlendedTile(cArray, alpha);
                    //Color[] colors = cArray[0];
                    texture.SetPixels(tilePos[i, 0] * TILE_RESOLUTION, tilePos[i, 1] * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION, colors);
                   

                }

                texture.Apply();
            }
        }
    }

    TDTile[] GetVertexTiles(int xPos, int zPos)
    {
        TDTile botLeft = map.GetTile(xPos - 1, zPos - 1);
        TDTile botRight = map.GetTile(xPos, zPos - 1);
        TDTile topLeft = map.GetTile(xPos - 1, zPos);
        TDTile topRight = map.GetTile(xPos, zPos);
        TDTile[] tilesArray = new TDTile[] { botLeft, botRight, topLeft, topRight };

        return tilesArray;
    }

    int[,] GetTilePositions(int xPos, int zPos)
    {
        int[,] tilePos = new int[4, 2] { { xPos - 1, zPos - 1 }, { xPos, zPos - 1 }, { xPos - 1, zPos }, { xPos, zPos } };

        return tilePos;
    }

    void SetTileVertexType(TDTile[] tiles)
    {
        if(tiles[0] != null)
        {
            tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].type = MapEditorManager.main.vType;
        }
        if (tiles[1] != null)
        {
            tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].type = MapEditorManager.main.vType;
        }
        if (tiles[2] != null)
        {
            tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].type = MapEditorManager.main.vType;
        }
        if (tiles[3] != null)
        {
            tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].type = MapEditorManager.main.vType;
        }
    }

    float Mix(float x, float y, float a)
    {
        return x * (1 - a) + y * a;
    }
    public void AddCliff(Vector3 t, int height)
    {
        float radius = 0.1f;
        Vector3 newVert = Vector3.zero;
        Vector3 test = transform.InverseTransformPoint(new Vector3(t.x, Mathf.FloorToInt(t.y), t.z));
        int xPos = (int)test.x;
        int zPos = (int)test.z;

        Vector3 wPos = transform.TransformPoint(t);
        int worldXPos = (int)wPos.x;
        int worldZPos = (int)wPos.z;

        TDTile[] tilesArray = GetVertexTiles(xPos, zPos);

        int[,] tilePos = GetTilePositions(xPos, zPos);
        int[,] cliffPos = new int[4, 2] { { worldXPos - 1, worldZPos - 1 }, { worldXPos + 1, worldZPos - 1 }, { worldXPos - 1, worldZPos + 1 }, { worldXPos + 1, worldZPos + 1 } };

        if (tilesArray[0].vertices[TDTile.TOP_RIGHT_VERTEX].height == test.y + height)
        {
            return;
        }

        //for (int h = 0; h < splitVerts.Length; h++)
        //{
        //    for (int index = 0; index < splitVerts[h].Length; index++)
        //    {
        //        //float distance = Vector3.Distance(splitVerts[h][index], test);
        //        float distance = new Vector3(splitVerts[h][index].x - test.x, 0, splitVerts[h][index].z - test.z).magnitude;

        //        //if vert is within the radius 
        //        if (distance < radius)
        //        {
        //            newVert = splitVerts[h][index];
        //            if(newVert.y < test.y + 1)
        //            {
        //                newVert.y += 1;
        //            }
        //            else
        //            {
        //                newVert.y = test.y + 1;
        //            }
        //            splitVerts[h][index] = newVert;

        //        }

        //    }
        //}

        for (int index = 0; index < verts.Length; index++)
        {
            //float distance = Vector3.Distance(splitVerts[h][index], test);
            float distance = new Vector3(verts[index].x - test.x, 0, verts[index].z - test.z).magnitude;

            //if vert is within the radius 
            if (distance < radius)
            {
                newVert = verts[index];
                if (newVert.y < test.y + height)
                {
                    newVert.y += height;
                }
                else
                {
                    newVert.y = test.y + height;
                }
                verts[index] = newVert;

            }
        }

            //    List<Vector3> combinedVerts = new List<Vector3>();
            //for (int z = 0; z < splitVerts.Length; z++)
            //{
            //    for(int y = 0; y < splitVerts[z].Length; y++)
            //    {
            //        combinedVerts.Add(splitVerts[z][y]);
            //    }
            //}

            Vector3[] newVerts = verts;

        mesh.vertices = newVerts;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshCollider.sharedMesh = mesh;

        SetTileCliffType(tilesArray, height);

        Vector3 tileVertices = new Vector3(test.x, (int)test.y + height, test.z);
        SetTileVertices(tilesArray, tileVertices);

        for (int i = 0; i < tilesArray.Length; i++)
        {
            TDTile tile = tilesArray[i];

            bool isTileHidden = false;
            for (int x = 0; x < tile.vertices.Length; x++)
            {
                if(tile.vertices[x].height != test.y + height)
                {
                    isTileHidden = true;
                }
            }


            if (tile.cliffModel)
            {
                Destroy(tile.cliffModel);
                tile.cliffModel = null;
            }

            Color[] colors = new Color[TILE_RESOLUTION * TILE_RESOLUTION];
            if (isTileHidden)
            {
                tile.type = TileType.CLIFF;
                for (int z = 0; z < colors.Length; z++)
                {
                    colors[i] = new Color(0, 0, 0, 0.01f);
                }

                float xInst = (float)(cliffPos[i, 0] + xPos) / 2;

                float zInst = (float)(cliffPos[i, 1] + zPos) / 2;
                int[] numbers = new int[] { tile.vertices[0].height, tile.vertices[1].height, tile.vertices[2].height, tile.vertices[3].height };
                int baseHeight = numbers.Min();
                GameObject wall = Instantiate(Resources.Load("Cliffs/Grass/" + tile.GetVertexCliff(baseHeight)), new Vector3(xInst, baseHeight, zInst), transform.rotation) as GameObject;
                tile.cliffModel = wall;
            }
            else
            {
                tile.type = TileType.GROUND;
                colors = spriteTiles[0][0];
            }

            texture.SetPixels(tilePos[i, 0] * TILE_RESOLUTION, tilePos[i, 1] * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION, colors);

            for (int x = 0; x < tile.vertices.Length; x++)
            {
                Vector3 point = new Vector3(tile.vertices[x].vertex.x, tile.vertices[x].vertex.y, tile.vertices[x].vertex.z);

                if (tile.cliffModel != null)
                {
                    tile.cliffModel.GetComponentInChildren<ModifyCliff>().ModifyVerteces(point, false);
                }
            }
        }

        texture.Apply();
    }

    void ModifyCliff(GameObject wall, TDTile tile)
    {
        Vector3 bottomLeft = new Vector3(tile.vertices[0].vertex.x, tile.vertices[0].vertex.y, tile.vertices[0].vertex.z);
        Vector3 bottomRight = new Vector3(tile.vertices[1].vertex.x, tile.vertices[1].vertex.y, tile.vertices[1].vertex.z);
        Vector3 topRight = new Vector3(tile.vertices[2].vertex.x, tile.vertices[2].vertex.y, tile.vertices[2].vertex.z);
        Vector3 topLeft = new Vector3(tile.vertices[3].vertex.x, tile.vertices[3].vertex.y, tile.vertices[3].vertex.z);
        float sampleHeight = bottomLeft.y;
        if(bottomRight.y == sampleHeight && topRight.y == sampleHeight && topLeft.y == sampleHeight)
        {
            return;
        }
        MeshRenderer meshRenderer = wall.GetComponentInChildren<MeshRenderer>();
        MeshFilter meshFilter = wall.GetComponentInChildren<MeshFilter>();
        //Mesh mesh = new Mesh();
        //Vector3[] vertices = new Vector3[meshFilter.mesh.vertices.Length];
        //for (int i = 0; i < meshFilter.mesh.vertices.Length; i++)
        //{
        //    vertices[i] = meshFilter.mesh.vertices[i];
        //    //Vector3 worldPt = wall.transform.TransformPoint(verts[i]);
        //    print(vertices[i]);
        //}
        //mesh.vertices = vertices;
        //print(vertices[0]);
        //meshFilter.mesh = mesh;

        //print(meshFilter);

        //for (int h = 0; h < vertices.Length; h++)
        //{
        //    float distance = Vector3.Distance(vertices[h], test);
        //    Debug.Log(vertices[h]);
        //    //if vert is within the radius 
        //    if (distance < radius)
        //    {
        //        newVert = vertices[h];
        //        newVert.y += 0.02f;
        //        vertices[h] = newVert;

        //    }
        //}

        //var mesh = meshFilter.mesh;

        //var edges = GetMeshEdges(mesh);

        //for (int i = 0; i < edges.Length; i++)
        //{
        //    print(i + ": " + edges[i].v1 + ", " + edges[i].v2);
        //}

        //Vector3 pos = QuadLerp(topRight, topLeft, bottomRight, bottomLeft, 0, 0);
        //Debug.Log(pos);

        Vector3[] verts = meshFilter.mesh.vertices;
        for (int i = 0; i < verts.Length; i++)
        {
            float top = Mix(topRight.y, topLeft.y, -(verts[i].z));
            //Debug.Log(top);
            float bottom = Mix(bottomRight.y, bottomLeft.y, -(verts[i].z));
            //Debug.Log(bottom);
            float value = Mix(bottom, top, verts[i].z);
            //Vector3 t = new Vector3(verts[i].x, pos.y, verts[i].z);
            //Vector3 worldPt = transform.InverseTransformPoint(verts[i]);
            //verts[i] = t;
            //verts[i] = new Vector3(verts[i].x, verts[i].y, verts[i].z + 0.02f);
            //Debug.Log(t);
        }
        print(bottomRight.y);
        verts[0].z = bottomRight.y;
        print(verts[0].z);

        meshFilter.mesh.vertices = verts;

    }

    public Vector3 QuadLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float u, float v)
    {
        Vector3 abu = Vector3.Lerp(a, b, u);
        Vector3 dcu = Vector3.Lerp(d, c, u);
        return Vector3.Lerp(abu, dcu, v);
    }

    public void ModifyVerteces(Vector3 t, bool lowerHeight)
    {
        float radius = Brush.main.GetBrushSize();
        Vector3 newVert = Vector3.zero;
        Vector3 test = transform.InverseTransformPoint(t);
        int xPos = (int)test.x;
        int zPos = (int)test.z;

        TDTile[] tilesArray = GetVertexTiles(xPos, zPos);

        //for (int h = 0; h < splitVerts.Length; h++)
        //{
        //    for (int index = 0; index < splitVerts[h].Length; index++)
        //    {
        //        float distance = Vector3.Distance(splitVerts[h][index], test);

        //        //if vert is within the radius 
        //        if (distance < radius)
        //        {
        //            newVert = splitVerts[h][index];
        //            newVert.y += 0.02f;
        //            splitVerts[h][index] = newVert;

        //        }

        //    }
        //}
        int index;
        for (index = 0; index < verts.Length; index++)
        {
            float distance = Vector3.Distance(verts[index], test);
            //print(verts[index]);
            //if vert is within the radius 
            if (distance < radius)
            {
                newVert = verts[index];
                if(lowerHeight)
                {
                    newVert.y -= 0.02f;
                }
                else
                {
                    newVert.y += 0.02f;
                }
                verts[index] = newVert;

                SetTileVertices(tilesArray, verts[index]);

                foreach (TDTile tile in tilesArray)
                {
                    if (tile.cliffModel != null)
                    {
                        tile.cliffModel.GetComponentInChildren<ModifyCliff>().ModifyVerteces(transform.TransformPoint(verts[index]), lowerHeight);
                    }
                }
            }
        }

        //List<Vector3> combinedVerts = new List<Vector3>();
        //for (int z = 0; z < splitVerts.Length; z++)
        //{
        //    for (int y = 0; y < splitVerts[z].Length; y++)
        //    {
        //        combinedVerts.Add(splitVerts[z][y]);
        //    }
        //}

        //Vector3[] newVerts = combinedVerts.ToArray();

        Vector3[] newVerts = verts;

        mesh.vertices = newVerts;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshCollider.sharedMesh = mesh;

    }

    void SetTileVertices(TDTile[] tiles, Vector3 point)
    {
        tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].SetVertices(point.x, point.y, point.z);
        tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].SetVertices(point.x, point.y, point.z);
        tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].SetVertices(point.x, point.y, point.z);
        tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].SetVertices(point.x, point.y, point.z);
    }

    void SetTileCliffType(TDTile[] tiles, int y)
    {
        tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].height += y;
        //tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].vertex.y = y;

        tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].height += y;
        //tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].vertex.y = y;

        tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].height += y;
        //tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].vertex.y = y;

        tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].height += y;
        //tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].vertex.y = y;

    }

    Color[] BlendedTile(Color[][] cArray, float alpha = 1)
    {
        if(cArray.Length == 1)
        {
            return cArray[0];
        }

        if(cArray.Length == 2)
        {
            return AlphaBlend(cArray[0], cArray[1], alpha);
        }

        if (cArray.Length == 3)
        {
            Color[] blend = AlphaBlend(cArray[0], cArray[1]);
            return AlphaBlend(blend, cArray[2], alpha);
        }

        //if(cArray.Length == 4)
        //{
        //    Color[] blend = AlphaBlend(cArray[3], cArray[2]);
        //    Color[] blend1 = AlphaBlend(blend, cArray[1]);
        //    return AlphaBlend(blend1, cArray[0]);
        //}

        return cArray[0];
    }

    int SpriteIndex(VertexType type, TDVertex[] vertices)
    {
        int index = 0;

        index = vertices[0].value * DoesVertexEqual(vertices[0].type, type) +
            vertices[1].value * DoesVertexEqual(vertices[1].type, type) +
            vertices[2].value * DoesVertexEqual(vertices[2].type, type) +
            vertices[3].value * DoesVertexEqual(vertices[3].type, type);

        return index;
    }

    int DoesVertexEqual(VertexType type, VertexType checkAgainst)
    {
        if(type == checkAgainst)
        {
            return 1;
        }
        return 0;
    }

    public List<Vector3> NearestVertecesTo(Vector3 point)
    {
        // convert point to local space
        point = transform.InverseTransformPoint(point);

        //float minDistanceSqr = Mathf.Infinity;
        //Vector3 nearestVertex = Vector3.zero;
        List<Vector3> vList = new List<Vector3>();
        // scan all vertices to find nearest
        //foreach (Vector3 vertex in verts)
        //{
        //    Vector3 diff = point - vertex;
        //    float distSqr = diff.sqrMagnitude;
        //    if (distSqr < minDistanceSqr)
        //    {
        //        minDistanceSqr = distSqr;
        //        nearestVertex = vertex;
        //        Debug.Log(nearestVertex);
        //    }
        //}

        vList.Clear();

        for(int i = 1; i < Brush.main.GetBrushSize(); i++)
        {
            Vector3 test = new Vector3(point.x + i, 0, point.z);
            Vector3 test1 = new Vector3(point.x + i, 0, point.z + i);
            Vector3 test4 = new Vector3(point.x + i, 0, point.z - i);
            Vector3 test2 = new Vector3(point.x, 0, point.z + i);
            Vector3 test5 = new Vector3(point.x, 0, point.z - 1);
            Vector3 test3 = new Vector3(point.x - i, 0, point.z);
            Vector3 test6 = new Vector3(point.x - i, 0, point.z + i);
            Vector3 test7 = new Vector3(point.x - i, 0, point.z - i);
            vList.Add(test);
            vList.Add(test1);
            vList.Add(test2);
            vList.Add(test3);
            vList.Add(test4);
            vList.Add(test5);
            vList.Add(test6);
            vList.Add(test7);
        }
        vList.Add(point);

        return vList;
    }

    public int NearestVertexIndexTo(Vector3 point)
    {
        // convert point to local space
        point = transform.InverseTransformPoint(point);

        float minDistanceSqr = Mathf.Infinity;
        Vector3 nearestVertex = Vector3.zero;
        List<Vector3> vList = new List<Vector3>();
        // scan all vertices to find nearest
        foreach (Vector3 vertex in verts)
        {
            Vector3 diff = point - vertex;
            float distSqr = diff.sqrMagnitude;
            if (distSqr < minDistanceSqr)
            {
                minDistanceSqr = distSqr;
                nearestVertex = vertex;
            }
        }

        //Vector3 test = new Vector3(nearestVertex.x + 1, 0, nearestVertex.z);
        //Vector3 test1 = new Vector3(nearestVertex.x + 1, 0, nearestVertex.z + 1);
        //Vector3 test4 = new Vector3(nearestVertex.x + 1, 0, nearestVertex.z - 1);
        //Vector3 test2 = new Vector3(nearestVertex.x, 0, nearestVertex.z + 1);
        //Vector3 test5 = new Vector3(nearestVertex.x, 0, nearestVertex.z - 1);
        //Vector3 test3 = new Vector3(nearestVertex.x - 1, 0, nearestVertex.z);
        //Vector3 test6 = new Vector3(nearestVertex.x - 1, 0, nearestVertex.z + 1);
        //Vector3 test7 = new Vector3(nearestVertex.x - 1, 0, nearestVertex.z - 1);
        //vList.Add(nearestVertex);
        int test = Array.IndexOf(verts, nearestVertex);
        //vList.Add(test);
        //vList.Add(test1);
        //vList.Add(test2);
        //vList.Add(test3);
        //vList.Add(test4);
        //vList.Add(test5);
        //vList.Add(test6);
        //vList.Add(test7);

        return test;
    }

    public void ChangeTextureBrush()
    {
        GameObject bType = GameObject.Find("Toggle (2)");
        Vector3 bTypePos = bType.transform.position;

        for (int i = 0; i < textureSprites.Length; i++)
        {
            GameObject selection = Instantiate(Resources.Load("MapEditor/TextureTileSet")) as GameObject;
            selection.transform.SetParent(GameObject.Find("TileSetHolder").transform, false);
            Vector3 pos = new Vector3(bTypePos.x, bTypePos.y - bType.GetComponent<RectTransform>().rect.height, bTypePos.z);
            int copy = i;
            selection.GetComponent<Button>().onClick.AddListener(delegate { SetBrush(copy); });
            selection.GetComponentInChildren<Text>().text = i.ToString();
        }
    }

    void SetBrush(int index)
    {
        //vType = (VertexType)index;
    }

    void SplitVerts()
    {
        splitVerts = verts
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / 1000)
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToArray();
    }
}
