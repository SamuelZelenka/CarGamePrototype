using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireTackGenerator : MonoBehaviour
{
    [SerializeField]
    private CarController _carController;

    [SerializeField]
    private TrailRenderer[] _tires;

	[SerializeField]
	private ParticleSystem[] _smokeEmitters;

	private bool _isSliding;

	public void Clear()
	{
		foreach (var tire in _tires)
		{
			tire.Clear();
		}
		foreach(var smokeEmitter in _smokeEmitters)
		{
			smokeEmitter.Clear();
		}
	}
	void Update()
	{
		var wasSliding = _isSliding;
		_isSliding = _carController.IsSliding();
		if (wasSliding != _isSliding)
		{
			for (int i = 0; i < _tires.Length; i++)
			{
				_tires[i].emitting = _isSliding;
				var emission = _smokeEmitters[i].emission;
				emission.enabled = _isSliding;
			}
		}
	}
}
