using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
	[SerializeField]
	private string _startEnvironment;

	[SerializeField]
	private Environment[] _environmentData;
	private Dictionary<string, Environment> _environments = new Dictionary<string, Environment>();
	private string _currentEnvironment;

	private void Awake()
	{
		foreach (var environment in _environmentData)
			_environments.Add(environment.EnvironmentName, environment);
		
		LoadEnvironment(_startEnvironment);
	}

	public void LoadEnvironment(string environmentName)
	{
		var environment = _environments[environmentName];
		if (!_currentEnvironment.IsNullOrWhitespace())
		{
			_environments[_currentEnvironment].Unload();
		}
		_currentEnvironment = environment.EnvironmentName;
		environment.Load();
	}
}