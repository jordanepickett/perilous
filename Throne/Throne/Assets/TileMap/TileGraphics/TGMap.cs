using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class TGMap : MonoBehaviour {

    public int sizeX = 100;
    public int sizeZ = 50;
    public float tileSize = 1.0f;

    public Texture2D terrainTiles;
    public Texture2D grassSprite;
    public Texture2D[] textureSprites;
    public int grassResolution;
    public int tileResolution;
    TDMap map;
    TDVertexMap vertexMap;
    Vector3[] verts;
    Vector3[][] splitVerts;

    int textWidth;
    int textHeight;
    Texture2D texture;

    Color[][] tiles;
    Color[][] grass;
    Color[][][] spriteTiles;

    public VertexType vType = VertexType.ONE;
    public GameObject brushButton;

    public Mesh mesh;

    public GameObject wall;

    // Use this for initialization
    void Start () {
        textWidth = sizeX * grassResolution;
        textHeight = sizeZ * grassResolution;
        texture = new Texture2D(textWidth, textHeight);

        //tiles = ChopUpTiles();
        //grass = ChopUpGrass();

        spriteTiles = new Color[textureSprites.Length][][];
        
        for(int i = 0; i < textureSprites.Length; i++)
        {
            spriteTiles[i] = ChopUpTiles(textureSprites[i]);
        }

        BuildNewMesh();
        ChangeTextureBrush();
    }

    public void BuildNewMesh()
    {
        map = new TDMap(sizeX, sizeZ);

        int numTiles = sizeX * sizeZ;
        int numTris = numTiles * 6;

        int vSizeX = sizeX;
        int vSizeZ = sizeZ;
        int numVerts = vSizeX * vSizeZ * 4;

        // Generate the mesh data
        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] Uvs = new Vector2[numVerts];

        vertexMap = new TDVertexMap(numVerts);

        for (int p = 0; p < vertices.Length; p++)
        {
            vertexMap.GetVertex(p).type = VertexType.ONE;
        }

        int[] triangles = new int[numTris];

        int x, z, iVertCount = 0;
        for (z = 0; z < vSizeZ; z++)
        {
            for (x = 0; x < vSizeX; x++)
            {
                vertices[iVertCount + 0] = new Vector3(x, 0, z);
                vertices[iVertCount + 1] = new Vector3(x + 1, 0, z);
                vertices[iVertCount + 2] = new Vector3(x + 1, 0, z + 1);
                vertices[iVertCount + 3] = new Vector3(x, 0, z + 1);

                normals[iVertCount + 0] = Vector3.up;
                normals[iVertCount + 1] = Vector3.up;
                normals[iVertCount + 2] = Vector3.up;
                normals[iVertCount + 3] = Vector3.up;
                iVertCount += 4;

                //normals[z * vSizeX + x] = Vector3.up;
                //Uvs[z * vSizeX + x] = new Vector2((float)x / sizeX, (float)z / sizeZ);
            }
        }

        int iIndexCount = 0; iVertCount = 0;
        for (int i = 0; i < numTiles; i++)
        {
            triangles[iIndexCount + 0] += (iVertCount + 0);
            triangles[iIndexCount + 2] += (iVertCount + 1);
            triangles[iIndexCount + 1] += (iVertCount + 2);

            triangles[iIndexCount + 3] += (iVertCount + 0);
            triangles[iIndexCount + 5] += (iVertCount + 2);
            triangles[iIndexCount + 4] += (iVertCount + 3);

            iVertCount += 4; iIndexCount += 6;
        }


        // Create a new Mesh and populate with the data
        mesh = new Mesh();
        mesh.vertices = vertices;
        verts = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;

        int iVertCount1 = 0;

        //for (int zz = 0; zz < sizeZ; zz++)
        //{
        //    for (int xx = 0; xx < sizeX; xx++)
        //    {
        //        Uvs[iVertCount1 + 0] = new Vector2(0, 0); //Top left of tile in atlas
        //        Uvs[iVertCount1 + 1] = new Vector2(.01f, 0); //Top right of tile in atlas
        //        Uvs[iVertCount1 + 2] = new Vector2(.01f, .01f); //Bottom right of tile in atlas
        //        Uvs[iVertCount1 + 3] = new Vector2(0, .01f); //Bottom left of tile in atlas
        //        iVertCount1 += 4;
        //    }
        //}

        for (int i = 0; i < Uvs.Length; i++)
        {
            Uvs[i] = new Vector2(vertices[i].x / sizeX, vertices[i].z / sizeZ);
        }

        mesh.uv = Uvs;


        // Assign our mesh to our filter/renderer/collider
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

        SplitVerts();
        BuildTexture();
    }

    public void BuildTexture()
    {
        for (int y = 0; y < sizeZ; y++)
        {

            for(int x = 0; x < sizeX; x++)
            {
                //Color[] colorArray = new Color[tileResolution * tileResolution];
                ////Color[] p = tiles[ 0 ].Concat(grass[map.GetTile(x, y).type]).ToArray();
                //for (int z = 0; z < tileResolution; z++)
                //{
                //    for (int l = 0; l < tileResolution; l++)
                //    {
                //        int pixelIndex = z + (l * tileResolution);
                //        srcArray[0] = tiles[3];
                //        srcArray[1] = grass[map.GetTile(x, y).type];

                //        for (int i = 0; i < srcArray.Length; i++)
                //        {
                //            Color srcPixel = srcArray[i][pixelIndex];
                //            if (srcPixel.a > 0)
                //            {
                //                colorArray[pixelIndex] = srcPixel;
                //            }
                //        }
                //    }
                //}
                //Color[] colorArray = tiles[TDVertex.FULL];
                Color[] colorArray = spriteTiles[0][0];
                //Color[] colorArray = AlphaBlend(tiles[3], grass[map.GetTile(x, y).type]);

                texture.filterMode = FilterMode.Trilinear;
                //Color[] p = grass[13];
                texture.SetPixels(x * grassResolution, y * grassResolution, grassResolution, grassResolution, colorArray);
                //texture.SetPixels(x * grassResolution, y * grassResolution, grassResolution, grassResolution, p);
            }
        }

        texture.Apply();

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sharedMaterial.mainTexture = texture;
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
        int numTilesPerRow = terrainTiles.width / tileResolution;
        int numRows = terrainTiles.height / tileResolution;

        Color[][] tiles = new Color[numTilesPerRow * numRows][];

        for(int y = 0; y < numRows; y++)
        {
            for(int x = 0; x < numTilesPerRow; x++)
            {
                tiles[y * numTilesPerRow + x] = terrainTiles.GetPixels(x * tileResolution, ((numRows - 1) - y) * tileResolution, tileResolution, tileResolution);
            }
        }

        return tiles;
    }

    //Color[][] ChopUpGrass()
    //{
    //    int numTilesPerRow = grassSprite.width / grassResolution;

    //    int numRows = grassSprite.height / grassResolution;


    //    Color[][] tiles = new Color[numTilesPerRow * numRows][];
    //    for (int y = 0; y < numRows; y++)
    //    {
    //        for (int x = 0; x < numTilesPerRow; x++)
    //        {            
    //            tiles[y * numTilesPerRow + x] = grassSprite.GetPixels(x * grassResolution, y * grassResolution, grassResolution, grassResolution);
    //        }
    //    }

    //    return tiles;
    //}

    public void AddRandomTexture(Vector3 test)
    {
        int xPos = (int)test.x;
        int zPos = (int)test.z;
        TDTile botLeft = map.GetTile(xPos - 1, zPos - 1);
        TDTile botRight = map.GetTile(xPos, zPos - 1);
        TDTile topLeft = map.GetTile(xPos - 1, zPos);
        TDTile topRight = map.GetTile(xPos, zPos);
        TDTile[] tilesArray = new TDTile[] { botLeft, botRight, topLeft, topRight };
        int[,] tilePos = new int[4, 2] { { xPos - 1, zPos - 1}, { xPos, zPos - 1 }, { xPos - 1, zPos }, { xPos, zPos } };

        if(botLeft.vertices[1].type != vType)
        {
            botLeft.vertices[1].type = vType;
            botRight.vertices[0].type = vType;
            topLeft.vertices[2].type = vType;
            topRight.vertices[3].type = vType;

            for (int i = 0; i < tilesArray.Length; i++)
            {
                TDTile tile = tilesArray[i];
                if(tile.type != TileType.GROUND)
                {
                    return;
                }
                List<VertexType> vTypes = tile.GetVertexTypes();
                Color[][] cArray = new Color[vTypes.Count][];
                for(int x = 0; x < vTypes.Count; x++)
                {
                    if(x == 0)
                    {
                        cArray[x] = spriteTiles[(int)vTypes[x]][0];
                    }
                    else
                    {
                        cArray[x] = spriteTiles[(int)vTypes[x]][SpriteIndex(vTypes[x], tile.vertices)];
                    }
                }
                //Color[] p = texture.GetPixels(tilePos[i, 0] * grassResolution, tilePos[i, 1] * grassResolution, grassResolution, grassResolution);
                Color[] colors = BlendedTile(cArray);
                //Color[] colorArray1 = AlphaBlend(p, spriteTiles[(int)VertexType.THREE][GetSpriteImageIndex(tilesArray[i].vertices)]);

                texture.SetPixels(tilePos[i, 0] * grassResolution, tilePos[i, 1] * grassResolution, grassResolution, grassResolution, colors);
            }

            texture.Apply();
        }
    }

    public void AddRandomTextureList(List<Vector3> vList, float alpha = 1)
    {
        foreach(var t in vList)
        {
            Vector3 test = transform.TransformPoint(t);
            int xPos = (int)test.x;
            int zPos = (int)test.z;

            TDTile botLeft = map.GetTile(xPos - 1, zPos - 1);
            TDTile botRight = map.GetTile(xPos, zPos - 1);
            TDTile topLeft = map.GetTile(xPos - 1, zPos);
            TDTile topRight = map.GetTile(xPos, zPos);
            TDTile[] tilesArray = new TDTile[] { botLeft, botRight, topLeft, topRight };
            int[,] tilePos = new int[4, 2] { { xPos - 1, zPos - 1 }, { xPos, zPos - 1 }, { xPos - 1, zPos }, { xPos, zPos } };

            if (botLeft.vertices[1].type != vType)
            {
                botLeft.vertices[1].type = vType;
                botRight.vertices[0].type = vType;
                topLeft.vertices[2].type = vType;
                topRight.vertices[3].type = vType;

                for (int i = 0; i < tilesArray.Length; i++)
                {
                    TDTile tile = tilesArray[i];
                    if (tile.type != TileType.GROUND)
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
                    //Color[] p = texture.GetPixels(tilePos[i, 0] * grassResolution, tilePos[i, 1] * grassResolution, grassResolution, grassResolution);
                    Color[] colors = BlendedTile(cArray, alpha);
                    //Color[] colorArray1 = AlphaBlend(p, spriteTiles[(int)VertexType.THREE][GetSpriteImageIndex(tilesArray[i].vertices)]);

                    texture.SetPixels(tilePos[i, 0] * grassResolution, tilePos[i, 1] * grassResolution, grassResolution, grassResolution, colors);
                }

                texture.Apply();
            }
        }
    }

    public void ModifyVertices(Vector3 t)
    {
        float radius = 0.001f;
        Vector3 newVert = Vector3.zero;
        Vector3 test = transform.TransformPoint(t);
        int xPos = (int)test.x;
        int zPos = (int)test.z;

        for(int h = 0; h < splitVerts.Length; h++)
        {
            for (int index = 0; index < splitVerts[h].Length; index++)
            {
                float distance = Vector3.Distance(splitVerts[h][index], test);

                //if vert is within the radius 
                if (distance < radius)
                {
                    newVert = splitVerts[h][index];
                    Debug.Log(index);
                    newVert.y = 1;
                    splitVerts[h][index] = newVert;

                }

            }
        }

        List<Vector3> haha = new List<Vector3>();
        for (int z = 0; z < splitVerts.Length; z++)
        {
            for(int y = 0; y < splitVerts[z].Length; y++)
            {
                haha.Add(splitVerts[z][y]);
            }
        }

        Vector3[] newVerts = haha.ToArray();
        //Debug.Log("Finished");

        mesh.vertices = newVerts;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        TDTile botLeft = map.GetTile(xPos - 1, zPos - 1);
        TDTile botRight = map.GetTile(xPos, zPos - 1);
        TDTile topLeft = map.GetTile(xPos - 1, zPos);
        TDTile topRight = map.GetTile(xPos, zPos);
        TDTile[] tilesArray = new TDTile[] { botLeft, botRight, topLeft, topRight };
        int[,] tilePos = new int[4, 2] { { xPos - 1, zPos - 1 }, { xPos, zPos - 1 }, { xPos - 1, zPos }, { xPos, zPos } };
        int[,] cliffPos = new int[4, 2] { { xPos - 1, zPos - 1 }, { xPos + 1, zPos - 1 }, { xPos - 1, zPos + 1 }, { xPos + 1, zPos + 1} };

        if (botLeft.vertices[1].layerHeight == VertexLayerHeight.ONE)
        {
            return;
        }

        botLeft.vertices[1].layerHeight = VertexLayerHeight.ONE;
        botRight.vertices[0].layerHeight = VertexLayerHeight.ONE;
        topLeft.vertices[2].layerHeight = VertexLayerHeight.ONE;
        topRight.vertices[3].layerHeight = VertexLayerHeight.ONE;

        for (int i = 0; i < tilesArray.Length; i++)
        {
            TDTile tile = tilesArray[i];

            bool isTileHidden = false;
            for (int x = 0; x < tile.vertices.Length; x++)
            {

                if(tile.vertices[x].layerHeight != VertexLayerHeight.ONE)
                {
                    isTileHidden = true;
                }
            }


            if (tile.cliffModel)
            {
                Destroy(tile.cliffModel);
            }

            //Color[] p = texture.GetPixels(tilePos[i, 0] * grassResolution, tilePos[i, 1] * grassResolution, grassResolution, grassResolution);
            Color[] colors = new Color[grassResolution * grassResolution];
            if (isTileHidden)
            {
                tile.type = TileType.CLIFF;
                for (int z = 0; z < colors.Length; z++)
                {
                    colors[i] = new Color(0, 0, 0, 0.01f);
                }

                //Debug.Log(tile.GetVertexCliff());
                float xInst = (float)(cliffPos[i, 0] + xPos) / 2;

                float zInst = (float)(cliffPos[i, 1] + zPos) / 2;

                GameObject wall = Instantiate(Resources.Load("Cliffs/Grass/" + tile.GetVertexCliff()), new Vector3(xInst, 0, zInst), transform.rotation) as GameObject;
                tile.cliffModel = wall;
            }
            else
            {
                tile.type = TileType.GROUND;
                colors = spriteTiles[0][0];
            }

            //Color[] colorArray1 = AlphaBlend(p, spriteTiles[(int)VertexType.THREE][GetSpriteImageIndex(tilesArray[i].vertices)]);

            texture.SetPixels(tilePos[i, 0] * grassResolution, tilePos[i, 1] * grassResolution, grassResolution, grassResolution, colors);
        }

        texture.Apply();
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
            //Debug.Log("HERE");
            Color[] blend = AlphaBlend(cArray[0], cArray[1]);
            return AlphaBlend(blend, cArray[2], alpha);
            //return AlphaBlend(cArray[1], cArray[2]);
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

        //for(int i = 0; i < vertices.Length; i++)
        //{
        //    index += vertices[i].value * DoesVertexEqual(vertices[i].type, type);
        //    if(type == VertexType.ONE)
        //    {
        //        Debug.Log(DoesVertexEqual(vertices[i].type, type));
        //        Debug.Log("VERTEX TYPE: " + vertices[i].type);
        //    }
        //}

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

    public Vector3 NearestVertexTo(Vector3 point)
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
                vList.Add(vertex);
            }
        }
        // convert nearest vertex back to world space
        return transform.TransformPoint(nearestVertex);
    }

    public List<Vector3> NearestVertecesTo(Vector3 point)
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

        Vector3 test = new Vector3(nearestVertex.x + 1, 0, nearestVertex.z);
        Vector3 test1 = new Vector3(nearestVertex.x + 1, 0, nearestVertex.z + 1);
        Vector3 test4 = new Vector3(nearestVertex.x + 1, 0, nearestVertex.z - 1);
        Vector3 test2 = new Vector3(nearestVertex.x, 0, nearestVertex.z + 1);
        Vector3 test5 = new Vector3(nearestVertex.x, 0, nearestVertex.z - 1);
        Vector3 test3 = new Vector3(nearestVertex.x - 1, 0, nearestVertex.z);
        Vector3 test6 = new Vector3(nearestVertex.x - 1, 0, nearestVertex.z + 1);
        Vector3 test7 = new Vector3(nearestVertex.x - 1, 0, nearestVertex.z - 1);
        vList.Add(nearestVertex);
        vList.Add(test);
        vList.Add(test1);
        vList.Add(test2);
        vList.Add(test3);
        vList.Add(test4);
        vList.Add(test5);
        vList.Add(test6);
        vList.Add(test7);

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
        for (int i = 0; i < textureSprites.Length; i++)
        {
            GameObject selection = Instantiate(brushButton);
            selection.transform.SetParent(GameObject.Find("Canvas").transform);
            Vector3 pos = GameObject.Find("Button").transform.position;
            pos.y -= i * 50;
            selection.transform.position = pos;
            int copy = i;
            selection.GetComponent<Button>().onClick.AddListener(delegate { SetBrush(copy); });
            selection.GetComponentInChildren<Text>().text = i.ToString();
        }
    }

    void SetBrush(int index)
    {
        vType = (VertexType)index;
    }

    void SplitVerts()
    {
        splitVerts = verts
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / 1000)
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToArray();

        //for (int i = 0; i < chunks.Length; i++)
        //{
        //    foreach (var item in chunks[i])
        //        Console.WriteLine("chunk:{0} {1}", i, item);
        //}
    }
}
