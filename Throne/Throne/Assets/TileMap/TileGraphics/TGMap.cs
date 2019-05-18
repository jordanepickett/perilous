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

    public const int TILE_RESOLUTION = 128;
    public int[] PRIORITY = new int[8] { 0, 1, 2, 3, 4, 5, 17, 18};
    public int[] SECONDARY = new int[10] { 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

    public float tileSize = 1.0f;
    public Renderer renderer;

    public Texture2D[] textureSprites;
    protected TDMap map;
    protected TDVertexMap vertexMap;
    protected Vector3[] verts;
    protected Vector3[] displacedVerts;
    protected Vector3[] vertexVelocities;
    public float springForce = 20f;
    protected float damping = 1f;
    protected Vector3[][] splitVerts;

    protected int textWidth;
    protected int textHeight;
    protected Texture2D texture;
    RenderTexture rt;
    public int vMapSize;
    public Texture2D alphaTexture;
    public Camera c;

    Color[][] tiles;
    Color[][] grass;
    protected Color[][][] spriteTiles;
    protected Color[][][] variantTiles;

    protected Texture2D[][] spriteTextures;
    protected Texture2D[][] variantTextures;

    //public VertexType vType = VertexType.ONE;

    public Mesh mesh;
    public Mesh workingMesh;
    public MeshCollider meshCollider;
    protected Vector3[] normals;

    protected int sizeX;
    protected int sizeZ;

    public void CreateNewTerrain(TileMapDTO tileMapDto)
    {
        CreateSpriteTiles();
        //CreateTerrainTexture(tileMapDto);

        BuildNewMesh(tileMapDto);
        ChangeTextureBrush();
    }

    protected void CreateSpriteTiles()
    {
        spriteTiles = new Color[textureSprites.Length][][];
        variantTiles = new Color[textureSprites.Length][][];

        spriteTextures = new Texture2D[textureSprites.Length][];
        variantTextures = new Texture2D[textureSprites.Length][];

        for (int i = 0; i < textureSprites.Length; i++)
        {
            spriteTiles[i] = ChopUpTiles(textureSprites[i], i);
        }

        //print(spriteTextures.Length);
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
        SetTerrainTexture();
        List<Vector3> test = new List<Vector3>();
        //FIXME
        int sizeY = tileMapDTO.GetSizeZ();
        int sizeX = tileMapDTO.GetSizeX();
        for (int y = 0; y < sizeY; y++)
        {

            for(int x = 0; x < sizeX; x++)
            {
                test.Add(new Vector3(x, 0, y));
                //Color[] colorArray = GetSpriteTile(0, 0);
                //Color[] colorArray = spriteTiles[0][0];

                //texture.filterMode = FilterMode.Point;
                //texture.alphaIsTransparency = true;

                //texture.SetPixels(x * TILE_RESOLUTION, y * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION, colorArray);
            }
        }

        AddTextureToTerrain(test, VertexType.TWO);

        //texture.Apply();
    }

    void SetTerrainTexture()
    {
        //Find the Standard Shader
        //Material myNewMaterial = new Material(Shader.Find("Standard"));
        ////Set Texture on the material
        //myNewMaterial.SetFloat("_Mode", 1);
        //myNewMaterial.SetTexture("_MainTex", texture);
        //myNewMaterial.SetFloat("_Glossiness", .0f);
        //myNewMaterial.SetFloat("_Cutoff", .1f);
        //////myNewMaterial.SetFloat("_Mode", 2);
        //myNewMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //myNewMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //myNewMaterial.SetInt("_ZWrite", 0);
        ////myNewMaterial.DisableKeyword("_ALPHATEST_ON");
        //myNewMaterial.renderQueue = 3000;
        ////Apply to GameObject
        //GetComponent<MeshRenderer>().material = myNewMaterial;
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.sharedMaterial.mainTexture = texture;
        //Renderer m_Renderer = GetComponent<Renderer>();

        //renderer.material.SetTexture("_MainTex", texture);
        //renderer.material.EnableKeyword("_ALPHABLEND_ON");
        //renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

        rt = new RenderTexture(sizeX * TILE_RESOLUTION, sizeZ * TILE_RESOLUTION, 32, RenderTextureFormat.ARGB32);           //Create RenderTexture 512x512 pixels in size.
        GetComponent<Renderer>().material.SetTexture("_MainTex", rt);   //Assign my RenderTexure to be the main texture of my object.
        //Graphics.Blit(texture, rt);                     //Blit my starting texture to my RenderTexture.
        //c = GameObject.FindGameObjectWithTag("RenderCamera").GetComponent<Camera>();
        //c.targetTexture = rt;

        //c.Render();

        Color[] colors = new Color[TILE_RESOLUTION * TILE_RESOLUTION];
        for(int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(0, 0, 0, 0f);
        }
        alphaTexture = new Texture2D(TILE_RESOLUTION, TILE_RESOLUTION);
        alphaTexture.SetPixels(colors);
        alphaTexture.Apply();

    }

    public Color[] AlphaBlend(Color[] firstColors, Color[] secondColors, float alpha = 1)
    {
        int count = firstColors.Length;
        Color[] srcColor = new Color[count];
        for(int i = 0; i < count; i++)
        {
            //Color a = firstColors[i];
            //Color b = secondColors[i];
            //float srcF = b.a;
            //float destF = 1f - srcF;
            ////float alpha = srcF + destF * a.a;
            ////float alpha = 1;
            //Color r = (b * srcF + a * a.a * destF) / alpha;
            //r.a = alpha;
            //srcColor[i] = r;
            srcColor[i] = Color.Lerp(firstColors[i], secondColors[i], secondColors[i].a);
        }

        return srcColor;
    }

    Color[][] ChopUpTiles(Texture2D terrainTiles, int tileIndex)
    {
        int numTilesPerRow = terrainTiles.width / TILE_RESOLUTION;
        int numRows = terrainTiles.height / TILE_RESOLUTION;
        int variantIndex = 0;
        if(numTilesPerRow == 8)
        {
            numTilesPerRow = 4;
            variantIndex = 8;
        }

        Color[][] tiles = new Color[numTilesPerRow * numRows][];
        Texture2D[] textures = new Texture2D[numTilesPerRow * numRows];

        for (int y = 0; y < numRows; y++)
        {
            for(int x = 0; x < numTilesPerRow; x++)
            {
                tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels(x * TILE_RESOLUTION, ((numRows - 1) - y) * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION);

                Texture2D t = new Texture2D(TILE_RESOLUTION, TILE_RESOLUTION);
                t.SetPixels(tiles[y * numTilesPerRow + x]);
                t.Apply();
                textures[y * numTilesPerRow + x] = t;
            }
        }

        spriteTextures[tileIndex] = textures;

        Color[][] variant = new Color[numTilesPerRow * numRows + 2][];
        Texture2D[] vTextures = new Texture2D[numTilesPerRow * numRows + 2];

        if (variantIndex >= 4)
        {
            for (int y = 0; y < numRows; y++)
            {
                int test = 0;
                for (int x = 4; x < variantIndex; x++)
                {
                    variant[y * numTilesPerRow + test] = terrainTiles.GetPixels(x * TILE_RESOLUTION, ((numRows - 1) - y) * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION);

                    Texture2D t = new Texture2D(TILE_RESOLUTION, TILE_RESOLUTION);
                    t.SetPixels(variant[y * numTilesPerRow + test]);
                    t.Apply();
                    vTextures[y * numTilesPerRow + test] = t;

                    test++;
                }
            }

            variant[16] = tiles[0];
            variant[17] = tiles[15];

            Texture2D t1 = new Texture2D(TILE_RESOLUTION, TILE_RESOLUTION);
            t1.SetPixels(tiles[0]);
            vTextures[16] = t1;

            Texture2D t2 = new Texture2D(TILE_RESOLUTION, TILE_RESOLUTION);
            t2.SetPixels(tiles[15]);
            vTextures[17] = t2;


            variantTiles[tileIndex] = variant;

            variantTextures[tileIndex] = vTextures;
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

    public void AddTextureToTerrain(List<Vector3> vList, VertexType brushType, float alpha = 1)
    {
        //List<Color[]> colorsList = new List<Color[]>();
        //List<int[]> tileXList = new List<int[]>();
        //List<int[]> tileZList = new List<int[]>();

        foreach (var t in vList)
        {
            Vector3 vertex = transform.InverseTransformPoint(t);
            int xPos = (int)vertex.x;
            int zPos = (int)vertex.z;
            //Texture2D tex = new Texture2D(TILE_RESOLUTION, TILE_RESOLUTION);
            //tex.alphaIsTransparency = true;

            TDTile[] tilesArray = GetVertexTiles(xPos, zPos);
            //int[,] tilePos = GetTilePositions(xPos, zPos);
            //int[,] tilePos = new int[4, 2] { { xPos - 1, zPos }, { xPos, zPos }, { xPos - 1, zPos + 1 }, { xPos, zPos + 1 } };
            int[,] tilePos = new int[4, 2] { { xPos - 1 , zPos - 1 }, { xPos, zPos - 1 }, { xPos - 1, zPos }, { xPos, zPos } };

            //if (CheckTileVerticyTypes(tilesArray))
            //{
                SetTileVertexType(tilesArray, brushType);

                for (int i = 0; i < tilesArray.Length; i++)
                {
                    TDTile tile = tilesArray[i];
                    if (tile == null || tile.type != TileType.GROUND)
                    {
                        continue;
                    }

                    List<VertexType> vTypes = tile.GetVertexTypes();
                    //Color[][] cArray = new Color[vTypes.Count][];
                    Texture2D[] tArray = new Texture2D[vTypes.Count];

                    for (int x = 0; x < vTypes.Count; x++)
                    {
                        if (x == 0)
                        {
                            //cArray[x] = GetSpriteTile((int)vTypes[x], 0);
                            //cArray[x] = spriteTiles[(int)vTypes[x]][0];

                            tArray[x] = GetSpriteTexture((int)vTypes[x], 0);
                            //tex.SetPixels(tArray[x].GetPixels());

                        }
                        else
                        {
                            //cArray[x] = GetSpriteTile((int)vTypes[x], SpriteIndex(vTypes[x], tile.vertices));
                            //cArray[x] = spriteTiles[(int)vTypes[x]][SpriteIndex(vTypes[x], tile.vertices)];
                            tArray[x] = GetSpriteTexture((int)vTypes[x], SpriteIndex(vTypes[x], tile.vertices));
                        }
                        //if(i == 2)
                    //{
                        RenderTexture.active = rt;                      //Set my RenderTexture active so DrawTexture will draw to it.
                        GL.PushMatrix();                                //Saves both projection and modelview matrices to the matrix stack.
                                                                        //GL.LoadPixelMatrix(0, texture.width, texture.height, 0);            //Setup a matrix for pixel-correct rendering.
                        GL.LoadPixelMatrix(0, rt.width, 0, rt.height);
                        //Draw my stampTexture on my RenderTexture positioned by posX and posY.
                        //Graphics.DrawTexture(new Rect(tilePos[i, 0] - TILE_RESOLUTION, (rt.height - tilePos[i, 1]) - TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION), tArray[x]);
                       
                        Graphics.DrawTexture(new Rect(tilePos[i, 0] * TILE_RESOLUTION, tilePos[i, 1] * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION), tArray[x]);
                        GL.PopMatrix();                                //Restores both projection and modelview matrices off the top of the matrix stack.
                        RenderTexture.active = null;                    //De-activate my Ren 
                    //}
                }

                  

                    ////Color[] colors = new Color[TILE_RESOLUTION * TILE_RESOLUTION];

                    //Color[] colors = BlendedTile(cArray, alpha);
                    //tile.sprite = colors;
                    ////tex.Apply();

                    ////colorsList.Add(colors);
                    ////tileXList.Add(tilePos[i, 0);
                    //texture.SetPixels(tilePos[i, 0] * TILE_RESOLUTION, tilePos[i, 1] * TILE_RESOLUTION, TILE_RESOLUTION, TILE_RESOLUTION, colors);
                }

            //}
        }
        //renderer.material.SetTexture("_MainTex", tex);
        //tex.Apply();
        //texture.Apply();
    }

    Color[] GetSpriteTile(int vertexIndex, int index)
    {
        if(index == 0)
        {
            if (variantTiles[vertexIndex] != null)
            {
                int variantChance = UnityEngine.Random.Range(0, 100);
                if (variantChance >= 95)
                {
                    return variantTiles[vertexIndex][SECONDARY[UnityEngine.Random.Range(0, SECONDARY.Length -1)]];
                }
                return variantTiles[vertexIndex][PRIORITY[UnityEngine.Random.Range(0, PRIORITY.Length - 1)]];
            }

            int chance = UnityEngine.Random.Range(0, 100);
            if (chance >= 50)
            {
                return spriteTiles[vertexIndex][15];
            }
            return spriteTiles[vertexIndex][0];
        }
        return spriteTiles[vertexIndex][index];
    }

    Texture2D GetSpriteTexture(int vertexIndex, int index)
    {
        if (index == 0)
        {
            if (variantTiles[vertexIndex] != null)
            {
                int variantChance = UnityEngine.Random.Range(0, 100);
                if (variantChance >= 95)
                {
                    return variantTextures[vertexIndex][SECONDARY[UnityEngine.Random.Range(0, SECONDARY.Length - 1)]];
                }
                return variantTextures[vertexIndex][PRIORITY[UnityEngine.Random.Range(0, PRIORITY.Length - 1)]];
            }

            int chance = UnityEngine.Random.Range(0, 100);
            if (chance >= 50)
            {
                return spriteTextures[vertexIndex][15];
            }
            return spriteTextures[vertexIndex][0];
        }
        return spriteTextures[vertexIndex][index];
    }

    // 0 = bottom left
    // 1 = bottom right
    // 2 = top left
    // 3 = top right
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

    void SetTileVertexType(TDTile[] tiles, VertexType brushType)
    {
        if(tiles[0] != null)
        {
            tiles[0].vertices[TDTile.BOT_RIGHT_VERTEX].type = brushType;
        }
        if (tiles[1] != null)
        {
            tiles[1].vertices[TDTile.BOT_LEFT_VERTEX].type = brushType;
        }
        if (tiles[2] != null)
        {
            tiles[2].vertices[TDTile.TOP_RIGHT_VERTEX].type = brushType;
        }
        if (tiles[3] != null)
        {
            tiles[3].vertices[TDTile.TOP_LEFT_VERTEX].type = brushType;
        }
    }

    float Mix(float x, float y, float a)
    {
        return x * (1 - a) + y * a;
    }

    bool IsAlreadyACliff(TDTile[] tiles, Vector3 test, int height)
    {
        if (tiles.ElementAtOrDefault(0) != null)
        {
            return tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].height == test.y + height;
        }
        if (tiles.ElementAtOrDefault(1) != null)
        {
            return tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].height == test.y + height;
        }
        if (tiles.ElementAtOrDefault(2) != null)
        {
            return tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].height == test.y + height;
        }
        if (tiles.ElementAtOrDefault(3) != null)
        {
            return tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].height == test.y + height;
        }

        return true;
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

        //int[,] tilePos = GetTilePositions(xPos, zPos);
        int[,] tilePos = new int[4, 2] { { xPos - 1, zPos - 1 }, { xPos, zPos - 1 }, { xPos - 1, zPos }, { xPos, zPos } };
        int[,] cliffPos = new int[4, 2] { { worldXPos - 1, worldZPos - 1 }, { worldXPos + 1, worldZPos - 1 }, { worldXPos - 1, worldZPos + 1 }, { worldXPos + 1, worldZPos + 1 } };

        if(IsAlreadyACliff(tilesArray, test, height))
        {
            return;
        }

        newVert = GetDisplacedVert(xPos, zPos);
        if(height == 0)
        {
            newVert.y = height;
        }
        else
        {
            if (newVert.y < test.y + height)
            {
                newVert.y += height;
            }
            else
            {
                newVert.y = test.y + height;
            }
        }
        displacedVerts[zPos * vMapSize + xPos] = newVert;

        SetTileCliffType(tilesArray, height);

        Vector3 tileVertices = new Vector3(test.x, newVert.y, test.z);
        SetTileVertices(tilesArray, tileVertices, true);

        for (int i = 0; i < tilesArray.Length; i++)
        {
            TDTile tile = tilesArray[i];
            if (tile == null)
            {
                continue;
            }

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
            
            if (isTileHidden)
            {
                tile.type = TileType.CLIFF;
                

                float xInst = (float)(cliffPos[i, 0] + xPos) / 2;

                float zInst = (float)(cliffPos[i, 1] + zPos) / 2;
                int[] numbers = new int[] { tile.vertices[0].height, tile.vertices[1].height, tile.vertices[2].height, tile.vertices[3].height };
                int baseHeight = numbers.Min();
                //print(tile.GetVertexCliff(baseHeight));
                string cliffText = tile.GetVertexCliff(baseHeight);
                if (cliffText == "AAAA")
                {
                    tile.type = TileType.GROUND;
                    isTileHidden = false;
                    print(tile.cliffModel);
                }
                else
                {
                    GameObject wall = Instantiate(Resources.Load("Cliffs/Grass/" + cliffText), new Vector3(xInst, baseHeight, zInst), transform.rotation) as GameObject;
                    wall.transform.parent = gameObject.transform;
                    tile.cliffModel = wall;
                }
            }
            else
            {
                tile.type = TileType.GROUND;
            }
            List<VertexType> vTypes = tile.GetVertexTypes();
            SetTileVertexType(tilesArray, VertexType.ONE);

            RenderTexture.active = rt;                      //Set my RenderTexture active so DrawTexture will draw to it.
            GL.PushMatrix();                                //Saves both projection and modelview matrices to the matrix stack.
                                                            //GL.LoadPixelMatrix(0, texture.width, texture.height, 0);            //Setup a matrix for pixel-correct rendering.
            GL.LoadPixelMatrix(0, rt.width, 0, rt.height);
            if(isTileHidden)
            {
                Graphics.CopyTexture(alphaTexture, 0, 0, 0, 0, TILE_RESOLUTION, TILE_RESOLUTION, rt, 0, 0, tilePos[i, 0] * TILE_RESOLUTION, tilePos[i, 1] * TILE_RESOLUTION);
            }
            else
            {
                Graphics.CopyTexture(GetSpriteTexture(0,0), 0, 0, 0, 0, TILE_RESOLUTION, TILE_RESOLUTION, rt, 0, 0, tilePos[i, 0] * TILE_RESOLUTION, tilePos[i, 1] * TILE_RESOLUTION);
            }

            GL.PopMatrix();                                //Restores both projection and modelview matrices off the top of the matrix stack.
            RenderTexture.active = null;                    //De-activate my Ren 
                                                            //}
                                                            //}

            for (int x = 0; x < tile.vertices.Length; x++)
            {
                Vector3 point = new Vector3(tile.vertices[x].vertex.x, tile.vertices[x].vertex.y, tile.vertices[x].vertex.z);

                if (tile.cliffModel != null)
                {
                    tile.cliffModel.GetComponentInChildren<ModifyCliff>().ModifyVerteces(transform.TransformPoint(point.x, point.y, point.z), false);
                }
            }
        }

        //texture.Apply();
    }

    // 0 = bottom left
    // 1 = bottom right
    // 2 = top left
    // 3 = top right
    public void AddRamp(Vector3 point)
    {
        TDTile[] tilesArray = GetVertexTiles((int)point.x, (int)point.z);
        bool north = false;

        if (TDTile.IsVerticalRamp(tilesArray))
        {
            north = true;
            Vector3 newVert = Vector3.zero;
            Vector3 test = transform.InverseTransformPoint(point);
            int xPos = (int)test.x;
            int zPos = (int)test.z;
            int[,] tilePos = GetTilePositions(xPos, zPos);
            newVert = GetVertsAtPoint(xPos, zPos);

            if (newVert != Vector3.zero)
            {
                SetTileVertices(tilesArray, GetVertsAtPoint(xPos, zPos), true, true);

                bool addHeight = false;
                for (int i = 0; i < tilesArray.Length; i++)
                {
                    tilesArray[i].SetRampLine(north);

                    if (tilesArray[i].type != TileType.RAMP)
                    {
                        addHeight = true;
                    }
                }

                if(addHeight)
                {
                    newVert.y += 0.5f;
                }

                displacedVerts[zPos * vMapSize + xPos] = newVert;

                for(int i = 0; i < tilesArray.Length; i++)
                {
                    TDTile tile = tilesArray[i];
                    if (tile.cliffModel != null)
                    {
                        Destroy(tile.cliffModel);
                        tile.cliffModel = null;
                    }

                    List<VertexType> vTypes = tile.GetVertexTypes();
                    SetTileVertexType(tilesArray, VertexType.ONE);
                    if (tile.isRamp())
                    {
                        Graphics.CopyTexture(GetSpriteTexture(0, 0), 0, 0, 0, 0, TILE_RESOLUTION, TILE_RESOLUTION, rt, 0, 0, tilePos[i, 0] * TILE_RESOLUTION, tilePos[i, 1] * TILE_RESOLUTION);
                    }
                    else
                    {
                        Graphics.CopyTexture(alphaTexture, 0, 0, 0, 0, TILE_RESOLUTION, TILE_RESOLUTION, rt, 0, 0, tilePos[i, 0] * TILE_RESOLUTION, tilePos[i, 1] * TILE_RESOLUTION);
                    }
                }
            }

            if(north)
            {
                int[] numbers1 = new int[] { tilesArray[0].vertices[TDTile.BOT_LEFT_VERTEX].height, tilesArray[0].vertices[TDTile.TOP_LEFT_VERTEX].height, tilesArray[2].vertices[TDTile.TOP_LEFT_VERTEX].height, tilesArray[2].vertices[TDTile.TOP_RIGHT_VERTEX].height };
                int baseHeight = numbers1.Min();
                print(TDTile.GetRampModel1(baseHeight, tilesArray[2].vertices[TDTile.TOP_LEFT_VERTEX], tilesArray[2].vertices[TDTile.TOP_RIGHT_VERTEX], tilesArray[0].vertices[TDTile.BOT_RIGHT_VERTEX], tilesArray[0].vertices[TDTile.BOT_LEFT_VERTEX]));

                int[] numbers2 = new int[] { tilesArray[3].vertices[TDTile.TOP_LEFT_VERTEX].height, tilesArray[3].vertices[TDTile.TOP_RIGHT_VERTEX].height, tilesArray[1].vertices[TDTile.BOT_RIGHT_VERTEX].height, tilesArray[1].vertices[TDTile.TOP_LEFT_VERTEX].height };
                int baseHeight2 = numbers2.Min();
                print(TDTile.GetRampModel1(baseHeight, tilesArray[3].vertices[TDTile.TOP_LEFT_VERTEX], tilesArray[3].vertices[TDTile.TOP_RIGHT_VERTEX], tilesArray[1].vertices[TDTile.BOT_RIGHT_VERTEX], tilesArray[1].vertices[TDTile.BOT_LEFT_VERTEX]));

                //string ramp = TDTile.GetRampModel1(baseHeight, tilesArray[3].vertices[TDTile.TOP_LEFT_VERTEX], tilesArray[3].vertices[TDTile.TOP_RIGHT_VERTEX], tilesArray[1].vertices[TDTile.BOT_RIGHT_VERTEX], tilesArray[1].vertices[TDTile.BOT_LEFT_VERTEX]);

                try
                {
                    GameObject ramp = Instantiate(Resources.Load("CliffRamps/" + TDTile.GetRampModel1(baseHeight, tilesArray[3].vertices[TDTile.TOP_LEFT_VERTEX], tilesArray[3].vertices[TDTile.TOP_RIGHT_VERTEX], tilesArray[1].vertices[TDTile.BOT_RIGHT_VERTEX], tilesArray[1].vertices[TDTile.BOT_LEFT_VERTEX])),
                        new Vector3(point.x, baseHeight, point.z), transform.rotation)
                        as GameObject;
                    ramp.transform.parent = gameObject.transform;
                    tilesArray[3].cliffModel = ramp;
                    tilesArray[1].cliffModel = ramp;
                } catch(Exception e)
                {
                    print(e);
                }
            }

            //Vector3[] newVerts = verts;

            //mesh.vertices = newVerts;

            //mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
            //meshCollider.sharedMesh = mesh;
        }
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

        newVert = GetDisplacedVert(xPos, zPos);
        if(newVert != Vector3.zero)
        {
            if (lowerHeight)
            {
                newVert.y -= 0.02f;
            }
            else
            {
                newVert.y += 0.02f;
            }

            displacedVerts[zPos * vMapSize + xPos] = newVert;

            SetTileVertices(tilesArray, GetDisplacedVert(xPos, zPos));

            foreach (TDTile tile in tilesArray)
            {
                if (tile != null && tile.cliffModel != null)
                {
                    tile.cliffModel.GetComponentInChildren<ModifyCliff>().ModifyVerteces(transform.TransformPoint(GetDisplacedVert(xPos, zPos)), lowerHeight);
                }
            }

            //Vector3[] newVerts = verts;

            mesh.vertices = displacedVerts;

            //mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
            //meshCollider.sharedMesh = mesh;
        }
    }

    void SetTileVertices(TDTile[] tiles, Vector3 point, bool setRamp = false, bool rampStatus = false)
    {
        if(tiles.ElementAtOrDefault(0) != null)
        {
            tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].SetVertices(point.x, point.y, point.z);
            if(setRamp)
            {
                tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].ramp = rampStatus;
            }

        }
        if (tiles.ElementAtOrDefault(1) != null)
        {
            tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].SetVertices(point.x, point.y, point.z);
            if (setRamp)
            {
                tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].ramp = rampStatus;
            }
        }

        if (tiles.ElementAtOrDefault(2) != null)
        {
            tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].SetVertices(point.x, point.y, point.z);
            if (setRamp)
            {
                tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].ramp = rampStatus;
            }
        }
        if (tiles.ElementAtOrDefault(3) != null)
        {
            tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].SetVertices(point.x, point.y, point.z);
            if (setRamp)
            {
                tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].ramp = rampStatus;
            }
        }
    }

    void SetTileCliffType(TDTile[] tiles, int y)
    {
        if (tiles.ElementAtOrDefault(0) != null)
        {
            if(y == 0)
            {
                tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].height = y;
            }
            else
            {
                tiles[0].vertices[TDTile.TOP_RIGHT_VERTEX].height += y;
            }
        }

        if (tiles.ElementAtOrDefault(1) != null)
        {
            if (y == 0)
            {
                tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].height = y;
            }
            else
            {
                tiles[1].vertices[TDTile.TOP_LEFT_VERTEX].height += y;
            }
        }

        if (tiles.ElementAtOrDefault(2) != null)
        {
            if (y == 0)
            {
                tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].height = y;
            }
            else
            {
                tiles[2].vertices[TDTile.BOT_RIGHT_VERTEX].height += y;
            }
        }

        if (tiles.ElementAtOrDefault(3) != null)
        {
            if (y == 0)
            {
                tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].height = y;
            }
            else
            {
                tiles[3].vertices[TDTile.BOT_LEFT_VERTEX].height += y;
            }
        }

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

    public void ApplyMeshCollider()
    {
        meshCollider.sharedMesh = mesh;
    }

    public void DeformMesh(Vector3 point, float force)
    {
        for (int i = 0; i < displacedVerts.Length; i++)
        {
            AddForceToVertex(i, point, force);
        }
    }

    void AddForceToVertex(int i, Vector3 point, float force)
    {
        print(displacedVerts[i]);
        if(Vector3.Distance(point, displacedVerts[i]) <= Brush.main.GetBrushSize())
        {
            Vector3 pointToVertex = displacedVerts[i] - point;
            float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
            float velocity = attenuatedForce * Time.deltaTime;
            vertexVelocities[i].y += pointToVertex.normalized.y * velocity;
        }
        else
        {
            vertexVelocities[i] = vertexVelocities[i];
        }
        //Vector3 pointToVertex = displacedVerts[i] - point;
        //float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        //float velocity = attenuatedForce * Time.deltaTime;
        //vertexVelocities[i].y += pointToVertex.normalized.y * velocity;
        //vertexVelocities[i].y += pointToVertex.normalized.y + 0.02f;
    }

    void Update()
    {
        //for (int i = 0; i < displacedVerts.Length; i++)
        //{
        //    UpdateVertex(i);
        //}
        if(Brush.main.brushState != BrushState.TEXTURE)
        {
            mesh.vertices = displacedVerts;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            //meshCollider.sharedMesh = mesh;
        }
    }

    void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        Vector3 displacement = displacedVerts[i] - displacedVerts[i];
        velocity -= displacement * springForce * Time.deltaTime;
        velocity *= 1f - damping * Time.deltaTime;
        vertexVelocities[i] = velocity;
        displacedVerts[i] += velocity * Time.deltaTime;
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

    Vector3 GetVertsAtPoint(int x, int z)
    {
        return verts[z * vMapSize + x];
    }

    Vector3 GetDisplacedVert(int x, int z)
    {
        return displacedVerts[z * vMapSize + x];
    }
}
