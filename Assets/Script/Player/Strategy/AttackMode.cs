using UnityEngine;

public class AttackMode : IRotate
{
    PlayerController _controller;

    public AttackMode(PlayerController playerController)
    {
        _controller = playerController;
    }

    public void ChangeRotation()
    {
        if (_controller.CurrentTarget == null) return;

        float targetX = _controller.CurrentTarget.transform.position.x;
        float myX = _controller.transform.position.x;

        if (targetX > myX)
        {
            _controller.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (targetX < myX)
        {
            _controller.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
