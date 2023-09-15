using System.IO;
using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
	[Range(0, 500)]
	[SerializeField]
	private float _maxAcceleration = 15;

	[Range(0, 100)]
	[SerializeField]
	private float idleBrakeAmount = 10;

	[Range(0, 10)]
	[SerializeField]
	private float slidingBrakeAmount = 10;

	[Range(0, 100)]
	[SerializeField]
	private float _minAcceleration = 3;

	[Range(0,100)]
	[SerializeField]
	private float _acceleration = 1f;
	
	[Range(0,100)]
	[SerializeField]
	private float _deceleration = 1f;

	[Range(0, 100)]
	[SerializeField]
	private float turnSpeed = 10;

	[ReadOnly]
	[SerializeField]
	private float _currentAcceleration = 0;

	private float _currentSlideCorrection = 0;

	[SerializeField]
    private Rigidbody _rb;

	[SerializeField]
	private float distToTarget;

	[SerializeField]
	private Vector3 _target;

	[SerializeField]
	private Transform _targetVisual;

	[SerializeField]
	float _turnSpeed;
	[SerializeField]
	Transform _rightTire;

	[SerializeField]
	Transform _leftTire;

	private Quaternion _rotationToTarget;

	private float _targetPathTValue;

	private Vector3 _previousVelocity;
	private Vector3 _velocity;

	private bool _isAccelerating;

    void Start()
    {
		_targetPathTValue = 0;
		_rb = GetComponent<Rigidbody>();
		var path = GameSession.Path;
		_target = path.GetPos(_targetPathTValue);
		transform.position = _target;
	}

	void Update()
	{
		MoveTarget();
		MoveChaser();
	}

	private void FixedUpdate()
	{
		_currentSlideCorrection = -Vector3.Dot(transform.right, _velocity);
		var forwardDir = Vector3.Dot(transform.forward, _velocity);

		ApplySlidingFriction(_currentSlideCorrection);
		ApplyTireRotation(_currentSlideCorrection);

		ApplyAcceleration(forwardDir);

		CheckMaxAcceleration();
		CheckFullStopEstimation();

		ApplyVelocity();
		UpdateTargetPos();

		_rb.velocity = _velocity;
		_previousVelocity = _velocity;
	}

	public Vector3 GetForce() => transform.position + _rb.velocity / 10;
	public Vector3 GetTargetDir() => transform.position + (_target - transform.position);
	public Vector3 GetAcceleration() => transform.position + transform.forward * (_currentAcceleration / _maxAcceleration) * 5;
	public Vector3 GetSlidingCorrection() => transform.position + transform.right * _currentSlideCorrection / slidingBrakeAmount;

	public void AccelerateInput(bool isAccelerating)
	{
		_isAccelerating = isAccelerating;
	}

	private void MoveTarget()
	{
		var isTargetTooClose = Vector3.Distance(transform.position, _target) < distToTarget;
		var path = GameSession.Path;
		if (isTargetTooClose)
		{
			var nextPos = path.GetPos(_targetPathTValue);
			while (isTargetTooClose)
			{
				_targetPathTValue += 0.01f;
				_targetPathTValue = ClampTToPath(_targetPathTValue);
				nextPos = path.GetPos(_targetPathTValue);
				isTargetTooClose = Vector3.Distance(transform.position, nextPos) < distToTarget;
			}
			_target = nextPos;
		}
	}

	private float ClampTToPath(float t) => t % GameSession.Path.controlPoints.Length;

	private void MoveChaser()
	{
		SteerTowardsTarget();
	}

	private void SteerTowardsTarget()
	{
		Vector3 steerDirection = _target - transform.position;
		steerDirection.y = 0;

		_rotationToTarget = Quaternion.LookRotation(steerDirection);
		transform.rotation = Quaternion.Slerp(transform.rotation, _rotationToTarget, turnSpeed * Time.deltaTime);
	}

	private void ApplySlidingFriction(float slidingDirection)
	{
		_velocity += transform.right * slidingDirection * slidingBrakeAmount;
	}
	private void ApplyTireRotation(float slidingDirection)
	{
		const float MAX_ANGLE = 25;
		var angle = Mathf.Clamp(MAX_ANGLE * -slidingDirection, -MAX_ANGLE, MAX_ANGLE);

		if (!_isAccelerating || Mathf.Abs(slidingDirection) < 0.2f)
		{
			angle = 0;
		}
		var newRot = Quaternion.Euler(0, 0, angle);

		_rightTire.localRotation = newRot;
		_leftTire.localRotation = newRot;
	}

	private void ApplyAcceleration(float forwardDir)
	{
		if (_isAccelerating)
		{
			Accelerate();
		}
		else if (_currentAcceleration > 0)
		{
			Decelerate(forwardDir);
		}
		else if (forwardDir > 0.01f)
		{
			_velocity -= transform.forward * forwardDir * idleBrakeAmount;
		}

		_velocity += transform.forward * _currentAcceleration;
	}

	private void Accelerate()
	{
		if (_currentAcceleration < _minAcceleration)
		{
			_currentAcceleration = _minAcceleration;
		}
		_currentAcceleration = Mathf.Min(_maxAcceleration, _currentAcceleration + (_acceleration * Time.fixedDeltaTime));
	}

	private void Decelerate(float forwardDir)
	{
		_currentAcceleration = Mathf.Max(0, _currentAcceleration - (_deceleration * Time.fixedDeltaTime));
	}

	private void CheckMaxAcceleration()
	{
		if (_velocity.magnitude > _maxAcceleration)
		{
			_velocity = _velocity.normalized * _maxAcceleration;
		}
	}

	private void CheckFullStopEstimation()
	{
		if (_velocity.magnitude < 0.1f)
		{
			_velocity = Vector3.zero;
		}
	}

	private void ApplyVelocity()
	{
		_velocity = Vector3.Lerp(_previousVelocity, _velocity, Time.fixedDeltaTime);
		_velocity.y = 0;
		_rb.MovePosition(new Vector3(transform.position.x, _target.y + 0.5f,transform.position.z));
	}

	private void UpdateTargetPos()
	{
		var nextPos = Vector3.Slerp(_targetVisual.transform.position, _target, 25 * Time.fixedDeltaTime);
		_targetVisual.transform.position = nextPos;
	}
}
