
using System.Collections.Generic;
using UnityEngine;

public class TDMap {

    TDTile[] _tiles;
    Dictionary<int, TDTile> tileDic;
    int _width;
    int _height;
    int _offsetX;
    int _offsetZ;

    int[][] mapData;

    public TDMap(int width, int height, int offsetX, int offsetZ)
    {
        _width = width;
        _height = height;
        _offsetX = offsetX;
        _offsetZ = offsetZ;

        _tiles = new TDTile[_width * _height];
        for (int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i] = new TDTile();
        }
        //tileDic = new Dictionary<int, TDTile>();
    }

    public void AddTile(int x, int z)
    {
        tileDic.Add(z * _width + x, new TDTile());
    }

    public TDTile GetTile(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return null;
        }
        //return tileDic[y * _width + x];

        return _tiles[y * _width + x];
    }

    public Dictionary<int, TDTile> GetTiles()
    {
        return tileDic;
    }
}
