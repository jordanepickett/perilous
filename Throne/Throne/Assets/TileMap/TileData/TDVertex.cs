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

public class TDVertex {

    public VertexType type = VertexType.ONE;
    public VertexLayerHeight layerHeight = VertexLayerHeight.ZERO;
    public int value;
}
