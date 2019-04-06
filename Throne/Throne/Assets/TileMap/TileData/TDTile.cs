
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

    public const int TOP_LEFT_VERTEX = 0;
    public const int TOP_RIGHT_VERTEX = 1;
    public const int BOT_RIGHT_VERTEX = 2;
    public const int BOT_LEFT_VERTEX = 3;

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

    public string GetVertexCliff(int baseHeight)
    {
        string fileName = "" + (char)('A' + (int)(vertices[0].height - baseHeight))
                        + (char)('A' + (int)(vertices[1].height - baseHeight))
                        + (char)('A' + (int)(vertices[2].height - baseHeight))
                        + (char)('A' + (int)(vertices[3].height - baseHeight));
        return fileName;
    }
}
