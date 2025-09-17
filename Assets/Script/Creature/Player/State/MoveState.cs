using UnityEngine;

public class MoveState : IState
{
    readonly PlayerController _controller;

    public MoveState(PlayerController controller)
    {
        _controller = controller;
    }

    void IState.Enter()
    {
    }

    void IState.Update()
    {
        _controller?.Move();
    }

    void IState.Exit()
    {
        _controller?.StopMove();
    }
}
