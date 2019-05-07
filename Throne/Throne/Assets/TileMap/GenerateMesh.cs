using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateMesh : TGMap {

    public void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void CreateMesh(TileMapDTO tileMapDTO, int offsetX, int offsetZ)
    {
        CreateTerrainTexture(tileMapDTO);

        CreateSpriteTiles();
        //BuildNewMesh(tileMapDto);
        //ChangeTextureBrush();

        sizeX = tileMapDTO.GetSizeX();
        sizeZ = tileMapDTO.GetSizeZ();

        map = new TDMap(sizeX, sizeZ, offsetX, offsetZ);

        int numTiles = sizeX * sizeZ;
        //int numTris = numTiles * 6;
        int numTris = numTiles * 2;

        int vSizeX = sizeX + 1;
        int vSizeZ = sizeZ + 1;
        vMapSize = vSizeX;
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
                //map.AddTile((int)(x * tileSize + offsetX), (int)(z * tileSize + offsetZ));
                //vertexMap.GetVertex(z * vSizeX + x).SetVertices(x * tileSize, 0, z * tileSize);
                TDTile tile = map.GetTile((int)(x * tileSize + offsetX), (int)(z * tileSize + offsetZ));
                if (tile != null)
                {
                    tile.vertices[TDTile.BOT_LEFT_VERTEX].SetVertices(x * tileSize, 0, z * tileSize);
                    tile.vertices[TDTile.BOT_RIGHT_VERTEX].SetVertices(x * tileSize + 1, 0, z * tileSize);
                    tile.vertices[TDTile.TOP_RIGHT_VERTEX].SetVertices(x * tileSize + 1, 0, z * tileSize + 1);
                    tile.vertices[TDTile.TOP_LEFT_VERTEX].SetVertices(x * tileSize, 0, z * tileSize + 1);
                }
                normals[z * vSizeX + x] = Vector3.up;
                Uvs[z * vSizeX + x] = new Vector2((float)x / sizeX, (float)z / sizeZ);
            }
        }

        for (z = 0; z < sizeZ; z++)
        {
            for (x = 0; x < sizeX; x++)
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
}
