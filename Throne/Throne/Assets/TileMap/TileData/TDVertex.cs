public enum VertexType
{
    ONE,
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX
}

public enum VertexLayerHeight
{
    NEGATIVE_THREE,
    NEGATIVE_TWO,
    NEVAGTIVE_ONE,
    ZERO,
    ONE,
    TWO,
    THREE
}

public struct Vertex3
{
    public float x;
    public float y;
    public float z;
}

public class TDVertex {

    public VertexType type = VertexType.ONE;
    public VertexLayerHeight layerHeight = VertexLayerHeight.ZERO;
    public int value;
    public int height = 0;
    public Vertex3 vertex;
    public bool ramp = false;

    public TDVertex()
    {
        vertex.x = 0;
        vertex.y = 0;
        vertex.z = 0;
    }

    public void SetVertices(float x, float y, float z)
    {
        vertex.x = x;
        vertex.y = y;
        vertex.z = z;
    }

    public Vertex3 GetVertices()
    {
        return vertex;
    }
}
