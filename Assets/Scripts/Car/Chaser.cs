using Unity.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Chaser : MonoBehaviour
{
	[Range(0, 500)]
	[SerializeField]
	private float _maxAcceleration = 15;

	[Range(0, 100)]
	[SerializeField]
	private float reverseBrakeAmount = 10;

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

	[SerializeField]
    private Rigidbody _rb;

    [SerializeField]
    private Target _target;

	private Vector3 _previousVelocity;
	private Vector3 _velocity;

	private bool _isAccelerating;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
	}

	void Update()
	{
		MoveChaser();
	}

	public void AccelerateInput(bool isAccelerating)
	{
		_isAccelerating = isAccelerating;
	}

	private void MoveChaser()
	{
		UpdateDistanceToTarget();
		SteerTowardsTarget();
	}

	//It's a bit weird to set the distance to the player from the player to the target. Need to have a second look on how target position as a whole
	private void UpdateDistanceToTarget()
	{
		var distToTarget = Vector3.Distance(transform.position, _target.transform.position);
		_target.SetDistance(distToTarget);
	}

	private void SteerTowardsTarget()
	{
		Vector3 steerDirection = _target.transform.position - transform.position;
		steerDirection.y = 0;

		Quaternion targetRotation = Quaternion.LookRotation(steerDirection);
		transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
	}

	private void ApplyBackwardsBreaking(float forwardTravelDirection)
	{
		if (forwardTravelDirection < -0.1f)
		{
			_velocity += transform.forward * -forwardTravelDirection * reverseBrakeAmount;
		}
	}

	private void ApplySlidingFriction(float slidingDirection)
	{
		_velocity += transform.right * slidingDirection * slidingBrakeAmount;
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
		_currentAcceleration -= _deceleration * Time.fixedDeltaTime;
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

		if (_target.transform.position.y > transform.position.y + 0.01f)
			_velocity.y = 1;
		else if (_target.transform.position.y < transform.position.y - 0.01f)
			_velocity.y = -1;
		else
			_velocity.y = 0;
	}

	private void FixedUpdate()
	{
		var slidingDir = -Vector3.Dot(transform.right, _velocity);
		var forwardDir = Vector3.Dot(transform.forward, _velocity);

		ApplyBackwardsBreaking(forwardDir);
		ApplySlidingFriction(slidingDir);

		ApplyAcceleration(forwardDir);

		CheckMaxAcceleration();
		CheckFullStopEstimation();

		ApplyVelocity();

		_rb.velocity = _velocity;
		_previousVelocity = _velocity;
	}
}
