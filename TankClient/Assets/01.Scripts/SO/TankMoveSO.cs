using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Tank/MoveSO")]  
public class TankMoveSO : ScriptableObject
{
    [Range(1f, 10f)]
    public float MaxSpeed;

    [Range(0.1f, 100f)]
    public float Acceleration = 50f, DeAccleration = 50f;

    [Range(0.1f, 720f)]
    public float RotateSpeed = 50f;

}
