
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType
{
    GROUND,
    CLIFF,
    RAMP,
    UNWALKABLE
}

public class TDTile {

    public const int TOP_LEFT_VERTEX = 0;
    public const int TOP_RIGHT_VERTEX = 1;
    public const int BOT_RIGHT_VERTEX = 2;
    public const int BOT_LEFT_VERTEX = 3;

    public TileType type = TileType.GROUND;

    public TDVertex[] vertices = new TDVertex[4];

    public Color[] sprite;

    public GameObject cliffModel;

    public TDTile()
    {
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new TDVertex();
            //vertices[i].type = VertexType.THREE;
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

    public string GetRampModel(int baseHeight)
    {
        string fileName = "" + (char)((vertices[0].ramp ? 'L' : 'A') + (int)(vertices[0].height - baseHeight) * (vertices[0].ramp ? -4 : 1))
                + (char)((vertices[1].ramp ? 'L' : 'A') + (int)(vertices[1].height - baseHeight) * (vertices[1].ramp ? -4 : 1))
                + (char)((vertices[2].ramp ? 'L' : 'A') + (int)(vertices[2].height - baseHeight) * (vertices[2].ramp ? -4 : 1))
                + (char)((vertices[3].ramp ? 'L' : 'A') + (int)(vertices[3].height - baseHeight) * (vertices[3].ramp ? -4 : 1));
        return fileName;
    }

    public static string GetRampModel1(int baseHeight, TDVertex topLeft, TDVertex topRight, TDVertex botRight, TDVertex botLeft)
    {
        string fileName = "" + (char)((topLeft.ramp ? 'L' : 'A') + (int)(topLeft.height - baseHeight) * (topLeft.ramp ? -4 : 1))
                + (char)((topRight.ramp ? 'L' : 'A') + (int)(topRight.height - baseHeight) * (topRight.ramp ? -4 : 1))
                + (char)((botRight.ramp ? 'L' : 'A') + (int)(botRight.height - baseHeight) * (botRight.ramp ? -4 : 1))
                + (char)((botLeft.ramp ? 'L' : 'A') + (int)(botLeft.height - baseHeight) * (botLeft.ramp ? -4 : 1));
        return fileName;
    }

    public bool isRamp()
    {
        bool ramp = true;
        for(int i = 0; i < vertices.Length; i++)
        {
            if(!vertices[i].ramp)
            {
                ramp = false;
            }
        }

        return ramp;
    }

    public void SetRampLine(bool north)
    {
        if(north)
        {
            if(vertices[BOT_RIGHT_VERTEX].ramp)
            {
                vertices[TOP_RIGHT_VERTEX].ramp = true;
            }
            if (vertices[BOT_LEFT_VERTEX].ramp)
            {
                vertices[TOP_LEFT_VERTEX].ramp = true;
            }
            if (vertices[TOP_RIGHT_VERTEX].ramp)
            {
                vertices[BOT_RIGHT_VERTEX].ramp = true;
            }
            if (vertices[TOP_LEFT_VERTEX].ramp)
            {
                vertices[BOT_LEFT_VERTEX].ramp = true;
            }
        }
    }

    public static bool IsVerticalRamp(TDTile[] tilesArray)
    {
        if((tilesArray[2].cliffModel != null && tilesArray[3] != null && tilesArray[0].type != TileType.CLIFF && tilesArray[1].type != TileType.CLIFF) 
            || (tilesArray[0].cliffModel != null && tilesArray[1].cliffModel != null && tilesArray[2].type != TileType.CLIFF && tilesArray[3].type != TileType.CLIFF))
        {
            return true;
        }

        return false;
    }
}
