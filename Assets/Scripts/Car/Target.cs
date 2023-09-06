using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float _targetDist;

    [SerializeField]
    private float _targetIncrementDist;

    [SerializeField]
    private Path _path;

    private float _t; // represents current T value on the path spline

    private float _distToPlayer;
    void Start()
    {
        _t = 0;
    }

    void Update()
    {
        if (IsChaserTooClose())
            MoveTargetForward();

        ClampTToPath();

        var pathPos = _path.GetPos(_t);

        SetPosition(pathPos);

		if (IsTGreaterThanPathLength())
            ResetT();
    }
    public void SetDistance(float distance) => _distToPlayer = distance;

    private bool IsChaserTooClose() => _distToPlayer < _targetDist;
    private void MoveTargetForward() => _t += _targetIncrementDist * Time.deltaTime;
    private void ClampTToPath() => _t = _t % _path.controlPoints.Length;
    private void SetPosition(Vector3 pos) => transform.position = pos;
	private bool IsTGreaterThanPathLength() => _t > _path.controlPoints.Length;
	private void ResetT() => _t = 0;
}
