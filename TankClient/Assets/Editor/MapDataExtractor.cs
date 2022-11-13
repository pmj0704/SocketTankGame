using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;
using System.IO;

public class MapDataExtractor
{
    // % = ctrl, # = shift, & = alt
    [MenuItem("Tools/ExtractMap %&e")]
    private static void ExtractMapData()
    {
        GameObject tilemap = GameObject.Find("Tilemap");

        if (tilemap == null)
        {
            Debug.LogError("There is no tilemap in hierarchy");
            return;
        }

        Tilemap tmCollision = tilemap.transform.Find("Collision").GetComponent<Tilemap>();
        Tilemap tmSafezone = tilemap.transform.Find("Safezone").GetComponent<Tilemap>();
        
        tmCollision.CompressBounds(); //Å¸ÀÏ¸ÊÀÇ Âî²¨±â¸¦ Á¦°ÅÇÑ´Ù.
        tmSafezone.CompressBounds();
        using (StreamWriter writer = File.CreateText($"Assets/Resources/Map/{tilemap.name}.txt"))
        {
            BoundsInt mapBound = tmCollision.cellBounds;

            writer.WriteLine(mapBound.xMin);
            writer.WriteLine(mapBound.xMax);
            writer.WriteLine(mapBound.yMin);
            writer.WriteLine(mapBound.yMin);

            //À§¿¡¼­ºÎÅÍ ¾Æ·¡·Î ¸ÊÀ» ½ºÄµÇÑ´Ù.
            for(int y = mapBound.yMax - 1; y >= mapBound.yMin; y--)
            {
                for(int x = mapBound.xMin; x < mapBound.xMax; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    TileBase tile = tmCollision.GetTile(tilePos);
                    TileBase safe = tmSafezone.GetTile(tilePos);

                    if(tile != null)
                    {
                        writer.Write("1");
                    }
                    else if(safe != null)
                    {
                        writer.Write("2");
                    }
                    else
                    {
                        writer.Write("0");
                    }    
                }
                writer.WriteLine("");//ÇÑÁÙ ³»¸®±â
            }
        }
        //writer.Flush();  using Å»Ãâ¿¡ ÇØÁÜ
    }
}
