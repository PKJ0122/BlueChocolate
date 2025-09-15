using UnityEngine;

public class DungeonCamera : MonoBehaviour
{
    const float SMOOTHSPEED = 0.125f;
    
    Transform _player;
    Vector3 _offset = new Vector3(0, 0, -10);


    public void Awake()
    {
        _player = Player.Instance.transform;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = _player.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, SMOOTHSPEED);
        transform.position = smoothedPosition;
    }
}
