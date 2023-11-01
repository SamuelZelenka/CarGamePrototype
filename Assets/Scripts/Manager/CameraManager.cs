using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
	private Camera _currentCamera;
	public void SetActiveCamera(Camera camera)
	{
		_currentCamera?.gameObject.SetActive(false);
		_currentCamera = camera;
		_currentCamera.gameObject.SetActive(true);
	}
}
