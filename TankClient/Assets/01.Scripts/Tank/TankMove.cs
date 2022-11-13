using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : MonoBehaviour
{
    public TankMoveSO _moveSO;

    private TankController _controller;

    public void Init(TankController controller)
    {
        _controller = controller;
    }    

    public void CheckInput()
    {
        CheckMove();
        CheckRotate();
    }

    private void CheckMove()
    {
        Vector2 velocity = _controller.Rigidbody.velocity;

        if(Input.GetKey(KeyCode.W))
        {
            velocity += (Vector2)_controller.TankBody.up * _moveSO.Acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            velocity += (Vector2)_controller.TankBody.up * -1 * _moveSO.Acceleration * Time.deltaTime;
        }
        else
        {
            velocity -= velocity * _moveSO.DeAccleration * Time.deltaTime;
        }

        velocity = Vector2.ClampMagnitude(velocity, _moveSO.MaxSpeed);
        _controller.Rigidbody.velocity = velocity;
    }

    private void CheckRotate()
    {
        float angle = 0;
        if (Input.GetKey(KeyCode.D))
        {
            angle = -_moveSO.RotateSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            angle = _moveSO.RotateSpeed * Time.deltaTime;
        }
        _controller.TankBody.rotation *= Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void StopImmediately()
    {
        _controller.Rigidbody.velocity = Vector2.zero;
    }

}
