using UnityEngine;

public class TrackCameraController : MonoBehaviour
{
	[SerializeField]
	[Range(0,1)]
	private float _pathOffset;

	[SerializeField]
	private float _moveSpeed = 5f;

	[SerializeField]
	private float _maxDistFromCar;

	[SerializeField]
	private CarController _targetController;

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private AnimationCurve _zoomCurve;

	private float _targetZoom;

	private void Start()
	{
		var controlPoints = GameManager.TrackManager.ControlPoints;
		var pos = Path.GetPos(0, controlPoints);
		pos.y += 500;
		transform.position = pos;
	}
	private void FixedUpdate()
	{
		MoveCameraTowardsTarget();
	}

	private void MoveCameraTowardsTarget()
	{
		var controlPoints = GameManager.TrackManager.ControlPoints;
		var p0 = Path.GetPos(_targetController.TargetPathTValue, controlPoints);
		var p1 = _targetController.transform.position;
		var targetDir = (p0 - p1).normalized;
		var newPos = _targetController.transform.position + targetDir * _moveSpeed * _targetController.GetVelocityPercentage();
		newPos.y = 500;
		transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
		ZoomCamera();
	}

	private void ZoomCamera()
	{
		var controller = GameManager.PlayerManager.player;
		if (_camera != null && controller != null)
		{
			Vector3 viewportPoint = _camera.WorldToViewportPoint(controller.transform.position);

			var newZoom = _zoomCurve.Evaluate(controller.GetAccelerationPercentage());
			_targetZoom = newZoom;

			float distanceToTopEdge = Mathf.Abs(1f - viewportPoint.y);
			float distanceToBottomEdge = viewportPoint.y;
			float distanceToLeftEdge = viewportPoint.x;
			float distanceToRightEdge = Mathf.Abs(1f - viewportPoint.x);

			float distanceToEdge = Mathf.Min(distanceToTopEdge, distanceToBottomEdge, distanceToLeftEdge, distanceToRightEdge);

			if (distanceToEdge < 0.2f)
			{
				_targetZoom += 10;
			}

			_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetZoom, Time.deltaTime);
		}
	}
}
