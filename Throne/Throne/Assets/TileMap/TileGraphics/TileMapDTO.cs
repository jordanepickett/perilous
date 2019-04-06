using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class TileMapDTO {

    const string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    const int NAME_LENGTH = 15;
    private static System.Random random = new System.Random();

    protected string name;
    protected int sizeX;
    protected int sizeZ;
    
    public TileMapDTO()
    {
        sizeX = 64;
        sizeZ = 64;
        name = new string(Enumerable.Repeat(CHARS, NAME_LENGTH)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public TileMapDTO(string name, int sizeX, int sizeZ)
    {
        this.name = name;
        this.sizeX = sizeX;
        this.sizeZ = sizeZ;
    }

    public string GetName()
    {
        return name;
    }

    public int GetSizeX()
    {
        return sizeX;
    }

    public int GetSizeZ()
    {
        return sizeZ;
    }

    public void SetSizeX(int sizeX)
    {
        this.sizeX = sizeX;
    }

    public void SetSizeZ(int sizeZ)
    {
        this.sizeZ = sizeZ;
    }
}
