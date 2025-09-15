using UnityEngine;

public class ChaseMode : IRotate
{
    PlayerController _controller;
    Vector2 _inputV;

    public ChaseMode(PlayerController playerController)
    {
        JoystickController.Instance.OnJoystickMoved += v =>
        {
            _inputV = v;
        };
        _controller = playerController;
    }

    public void ChangeRotation()
    {
        if (_inputV.x > 0)
        {
            _controller.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (_inputV.x < 0)
        {
            _controller.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
