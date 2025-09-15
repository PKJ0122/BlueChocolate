using UnityEngine;

public class ChaseMode : IRotate
{
    PlayerController _controller;

    public ChaseMode(PlayerController playerController)
    {
        _controller = playerController;
    }

    public void ChangeRotation()
    {
        // PlayerController가 관리하는 입력 값을 직접 참조
        Vector2 inputV = _controller.MoveInput;

        if (inputV.x > 0.1f) // Deadzone을 살짝 줌
        {
            _controller.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (inputV.x < -0.1f)
        {
            _controller.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}