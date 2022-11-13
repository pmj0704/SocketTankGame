using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGM.Proto.Tank;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform _mainMap;

    [SerializeField]
    private string _connectionUrl;

    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple GameManager is running!");
        
        Instance = this;

        NetworkManager.Instance = gameObject.AddComponent<NetworkManager>();
        NetworkManager.Instance.Init(_connectionUrl);
        NetworkManager.Instance.Connection();

        MapManager.Instance = new MapManager(_mainMap);
    }

    private void OnDestroy()
    {
        NetworkManager.Instance.Disconnect();
    }

    //Debug code
    private void Update()
    {
        /*
        if(Input.GetMouseButtonDown(0))
        //{
        //    Vector3 pos = Input.mousePosition;
        //    Vector3 worldPos = Camera.main.ScreenToWorldPoint(pos);
        //    worldPos.z = 0;

        //    Vector3Int tilePos = MapManager.Instance.GetTilePosition(worldPos);
        //    MapCategory mc = MapManager.Instance.GetTileCategory(tilePos);

        //    Debug.Log(tilePos);
        //    Debug.Log(mc);

        //    //C_Pos cPos = new C_Pos { X = tilePos.x, Y = tilePos.y };

        //    //NetworkManager.Instance.RegisterSend((ushort)MSGID.CPos, cPos);
        }*/
    }
}
