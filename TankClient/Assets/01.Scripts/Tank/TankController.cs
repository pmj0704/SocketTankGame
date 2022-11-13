using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : NetworkObject
{
    public bool IsRemote = false;

    private Rigidbody2D _rigidbody;
    public Rigidbody2D Rigidbody => _rigidbody;

    private TankMove _tankMove;
    private Transform _tankBody;
    public Transform TankBody => _tankBody;

    private TurretController _turretController;
    private Transform _tankTurret;
    public Transform TankTurret => _tankTurret;

    public string PlayerName;
    public bool IsEnemy;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _tankBody = transform.Find("Body");
        _tankTurret = transform.Find("Turret");
        _tankMove = GetComponent<TankMove>();

        _turretController = GetComponent<TurretController>();
    }

    private void Start()
    {
        _tankMove.Init(this); //이니셜라이즈
    }

    private void Update()
    {
        if(IsRemote == false)
        {
            _tankMove.CheckInput(); //입력 감지
        }
    }

}
