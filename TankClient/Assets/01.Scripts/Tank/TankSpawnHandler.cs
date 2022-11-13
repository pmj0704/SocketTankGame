using GGM.Proto.Tank;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TankSpawnHandler : MonoBehaviour, IPacketHandler
{
    
    public void Process(IMessage packet)
    {
        S_init s_Init = packet as S_init;
        GameObject _tankPrefab = Resources.Load<GameObject>("Prefabs/Tank");

        NetworkObject tankObj = Instantiate(_tankPrefab).GetComponent<NetworkObject>();
        tankObj.id = s_Init.PlayerId;
        tankObj.transform.position = new Vector3(s_Init.SpawnPosition.X, s_Init.SpawnPosition.Y, 0);

        TankController tank = tankObj.GetComponent<TankController>();
        try
        {
            C_Enter cEnter = new C_Enter
            {
                Name = tank.PlayerName,
                Position =
                    new Position
                    {
                        Rotate =
                        tank.transform.rotation.z,
                        X = tank.transform.position.x,
                        Y = tank.transform.position.y,
                        TurretRotate = tank.TankTurret.rotation.z
                    }
            };
            NetworkManager.Instance.RegisterSend((ushort)MSGID.CEnter, cEnter);
        }
        catch(Exception e)
        {
            Debug.LogError("Error when Sending CEnter");
        }
    }
    }
