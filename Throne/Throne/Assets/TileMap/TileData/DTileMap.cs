
public class DTileMap  {

    int sizeX;
    int sizeY;

    int[,] mapData;

    public DTileMap(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;

        mapData = new int[sizeX, sizeY];

        mapData[5, 5] = 2;
    }

    public int GetTileAt(int x, int y)
    {
        return mapData[x, y];
    }
}
