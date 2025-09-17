using UnityEngine;

public class AttackerManager : SingletonMonoBase<AttackerManager>
{
    [SerializeField] Vector3[] _offsets;
    public Vector3[] Offsets => _offsets;
}
