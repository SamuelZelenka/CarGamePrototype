using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public abstract class Environment : MonoBehaviour
{
	[SerializeField]
	private Camera _camera;
	[SerializeField]
	private string _environmentName;

	public string EnvironmentName => _environmentName;

	public virtual void Load()
	{
		GameManager.CameraManager.SetActiveCamera(_camera);
		gameObject.SetActive(true);
	}

	public virtual void Unload()
	{
		gameObject.SetActive(false);
	}
}
