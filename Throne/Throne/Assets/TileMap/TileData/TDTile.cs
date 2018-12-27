
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType
{
    GROUND,
    CLIFF,
    UNWALKABLE
}

public class TDTile {

    public TileType type = TileType.GROUND;

    public TDVertex[] vertices = new TDVertex[4];

    public GameObject cliffModel;

    public TDTile()
    {
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new TDVertex();
            vertices[i].type = VertexType.ONE;
        }

        SetVertexValues();
    }

    void SetVertexValues()
    {
        vertices[0].value = 8;
        vertices[1].value = 4;
        vertices[2].value = 1;
        vertices[3].value = 2;
    }

    public List<VertexType> GetVertexTypes()
    {
        List<VertexType> vTypes = new List<VertexType>();

        for(int i = 0; i < vertices.Length; i++)
        {
            if(!vTypes.Contains(vertices[i].type))
            {
                vTypes.Add(vertices[i].type);
            }
        }

        var sortedList = vTypes
       .OrderByDescending(x => (int)(x))
       .ToList();
        return sortedList;
    }

    public string GetVertexCliff()
    {
        //char fileName = (char)('A' + vertices[0].layerHeight)
        //                    + (char)('A' + vertices[1].layerHeight)
        //                    + (char)('A' + vertices[2].layerHeight)
        //                    + (char)('A' + vertices[3].layerHeight);
        string fileName = "" + (char)('A' + (int)(vertices[0].layerHeight - 3))
                        + (char)('A' + (int)(vertices[1].layerHeight - 3))
                        + (char)('A' + (int)(vertices[2].layerHeight - 3))
                        + (char)('A' + (int)(vertices[3].layerHeight - 3));
        return fileName;
    }
}
