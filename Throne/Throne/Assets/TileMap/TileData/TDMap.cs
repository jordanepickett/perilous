
public class TDMap {

    TDTile[] _tiles;
    int _width;
    int _height;

    int[][] mapData;

    public TDMap(int width, int height)
    {
        _width = width;
        _height = height;

        _tiles = new TDTile[_width * _height];
        for(int i = 0; i < _tiles.Length; i++)
        {
            _tiles[i] = new TDTile();
        }
    }

    public TDTile GetTile(int x, int y)
    {
        if(x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return null;
        }

        return _tiles[y * _width + x];
    }

    public TDTile[] GetTiles()
    {
        return _tiles;
    }
}
