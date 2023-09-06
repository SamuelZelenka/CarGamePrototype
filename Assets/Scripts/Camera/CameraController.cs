using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private float _moveSpeed = 5f; 

	private void FixedUpdate()
	{
		MoveCameraTowardsTarget();
	}

	private void MoveCameraTowardsTarget()
	{
		if (_target == null)
		{
			Debug.LogWarning("Camera target is not assigned.");
			return;
		}

		var pos = _target.position;
		pos.y += 500;

		float interpolationFactor = _moveSpeed * Time.deltaTime;

		Vector3 newPosition = Vector3.Lerp(transform.position, pos, interpolationFactor);
		transform.position = newPosition;
	}
}
