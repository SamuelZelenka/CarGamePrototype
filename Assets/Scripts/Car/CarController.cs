using Sirenix.Utilities;
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
	private float _idleBrakeAmount = 10;

	[Range(0, 10)]
	[SerializeField]
	private float _slidingBrakeAmount = 10;

	[SerializeField]
	private float _maxVelocity;

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
	private float _slidingForceThreshold;

	[SerializeField]
    private Rigidbody _rb;

	[SerializeField]
	private float distToTarget;

	[SerializeField]
	private Vector3 _target;

	[SerializeField]
	Transform _rightTire;

	[SerializeField]
	Transform _leftTire;

	[SerializeField]
	TireTackGenerator _tireTrackGenerator;

	private Quaternion _rotationToTarget;

	private float _targetPathTValue;

	public float TargetPathTValue => _targetPathTValue;


	private Vector3 _previousVelocity;
	private Vector3 _velocity;

	private bool _isAccelerating;

	private void Awake()
	{
		GameManager.PlayerManager.player = this;
	}

	public void Init()
    {

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

		_rb.velocity = _velocity;
		_previousVelocity = _velocity;
	}

	public float GetVelocityPercentage()
	{
		return _velocity.magnitude / _maxVelocity;
	}

	public float GetAccelerationPercentage()
	{
		return _currentAcceleration / _maxAcceleration;
	}
	public Vector3 GetForce() => transform.position + _rb.velocity / 10;
	public Vector3 GetTargetDir() => transform.position + (_target - transform.position);
	public Vector3 GetAcceleration() => transform.position + transform.forward * (_currentAcceleration / _maxAcceleration) * 5;
	public Vector3 GetSlidingCorrection() => transform.position + transform.right * _currentSlideCorrection / _slidingBrakeAmount;

	public bool IsSliding() => (transform.right * _currentSlideCorrection / _slidingBrakeAmount).sqrMagnitude > _slidingForceThreshold;

	public void AccelerateInput(bool isAccelerating)
	{
		_isAccelerating = isAccelerating;
	}
	public void ResetCar()
	{
		_tireTrackGenerator.Clear();
		_targetPathTValue = 0;
		_rb = _rb != null ? _rb : GetComponent<Rigidbody>();
		_rb.velocity = Vector3.zero;
		_rb.Sleep();
		_velocity = Vector3.zero;
		_previousVelocity = Vector3.zero;
		_currentSlideCorrection = 0;
		_currentAcceleration = 0;
		_target = Path.GetPos(_targetPathTValue, GameManager.TrackManager.ControlPoints);
		transform.position = _target;
	}
	private void MoveTarget()
	{
		var isTargetTooClose = Vector3.Distance(transform.position, _target) < distToTarget;
		if (isTargetTooClose)
		{
			var controlPoints = GameManager.TrackManager.ControlPoints;
			var nextPos = Path.GetPos(_targetPathTValue, controlPoints);
			while (isTargetTooClose)
			{
				_targetPathTValue += 0.01f;
				_targetPathTValue = ClampTToPath(_targetPathTValue);
				nextPos = Path.GetPos(_targetPathTValue, controlPoints);
				isTargetTooClose = Vector3.Distance(transform.position, nextPos) < distToTarget;
			}
			_target = nextPos;
		}
	}

	private float ClampTToPath(float t) => t % GameManager.TrackManager.ControlPoints.Count;

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
		_velocity += transform.right * slidingDirection * _slidingBrakeAmount;
	}

	private void ApplyTireRotation(float slidingDirection)
	{
		const float MAX_ANGLE = 25;
		var angle = Mathf.Clamp(MAX_ANGLE * -slidingDirection * 0.2f, -MAX_ANGLE, MAX_ANGLE);

		if (!_isAccelerating || Mathf.Abs(slidingDirection) < 0.2f)
		{
			angle = 0;
		}
		var oldRot = _rightTire.localRotation;
		var newRot = Quaternion.Euler(0, 0, angle);

		_rightTire.localRotation = Quaternion.Lerp(oldRot, newRot, Time.deltaTime * turnSpeed) ;
		_leftTire.localRotation = Quaternion.Lerp(oldRot, newRot, Time.deltaTime * turnSpeed);
	}

	private void ApplyAcceleration(float forwardDir)
	{
		if (_isAccelerating && !IsSliding())
		{
			Accelerate();
			//if (!IsOnRoad())
			//{
			//	_currentAcceleration = _minAcceleration;
			//}
		}
		else if (_currentAcceleration > 0)
		{
			Decelerate(forwardDir);
		}
		else if (forwardDir > 0.01f)
		{
			_velocity -= transform.forward * forwardDir * _idleBrakeAmount;
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

	private RaycastHit hit;

	private bool IsOnRoad()
	{
		bool isOnRoad = true;

		Debug.DrawRay(_leftTire.transform.position, _leftTire.transform.forward);
		Physics.Raycast(_leftTire.transform.position, -_leftTire.transform.up, out hit);
		if (!hit.transform)
			isOnRoad = false;


		Debug.DrawRay(_rightTire.transform.position, _rightTire.transform.forward);

		Physics.Raycast(_rightTire.transform.position, -_rightTire.transform.up, out hit);
		if (!hit.transform)
			isOnRoad = false;

		Debug.Log(hit.transform);

		return isOnRoad;
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
		_velocity = Vector3.ClampMagnitude(_velocity, _maxVelocity);
		_velocity = Vector3.Lerp(_previousVelocity, _velocity, Time.fixedDeltaTime);
		_velocity.y = 0;
		_rb.MovePosition(new Vector3(transform.position.x, _target.y + 0.5f,transform.position.z));
	}
}
