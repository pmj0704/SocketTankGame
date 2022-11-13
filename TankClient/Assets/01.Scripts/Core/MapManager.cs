using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum MapCategory
{
    PATH = 0,
    BLOCK = 1,
    SAFEZONE = 2,
    NONE = 99
}

public class MapManager
{
    public static MapManager Instance;

    private Tilemap _mainMap;
    private Tilemap _collisionMap;
    private Tilemap _safezoneMap;

    public MapManager(Transform mapObject)
    {
        _mainMap = mapObject.Find("Background").GetComponent<Tilemap>();
        _collisionMap = mapObject.Find("Collision").GetComponent<Tilemap>();
        _safezoneMap = mapObject.Find("Safezone").GetComponent<Tilemap>();
    }

    public Vector3Int GetTilePosition(Vector3 worldPos)
    {
        return _mainMap.WorldToCell(worldPos);
    }

    public MapCategory GetTileCategory(Vector3Int tilePos)
    {
        TileBase tile = _collisionMap.GetTile(tilePos);
        if (tile != null) return MapCategory.BLOCK;

        tile = _safezoneMap.GetTile(tilePos);
        if (tile != null) return MapCategory.SAFEZONE;

        tile = _mainMap.GetTile(tilePos);
        if (tile != null) return MapCategory.PATH;
        else return MapCategory.NONE;
    }
}
