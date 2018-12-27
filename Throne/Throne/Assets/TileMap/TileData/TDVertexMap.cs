using UnityEngine;

public class TDVertexMap
{

    TDVertex[] _vertexs;

    public TDVertexMap(int amount)
    {
        _vertexs = new TDVertex[amount];
        for (int i = 0; i < amount; i++)
        {
            _vertexs[i] = new TDVertex();
        }
    }

    public TDVertex GetVertex(int x)
    {
        return _vertexs[x];
    }

    public TDVertex GetVertexByVector3(int x, int z)
    {
        return _vertexs[z * _vertexs.Length + x];
    }
}

