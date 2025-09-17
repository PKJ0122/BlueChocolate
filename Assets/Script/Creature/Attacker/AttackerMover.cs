using UnityEngine;

public class AttackerMover : MonoBehaviour
{
    Transform _player;
    Vector3 _offset;


    void Awake()
    {
        _player = Player.Instance.transform;
        _offset = AttackerManager.Instance.Offsets[0];
    }

    void LateUpdate()
    {
        transform.position = _offset + _player.position;
    }

    public void Set(Vector3 offset)
    {
        _offset = offset;
    }
}
