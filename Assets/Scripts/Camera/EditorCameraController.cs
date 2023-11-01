using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraController : MonoBehaviour
{
	private bool _isMovable;
	private Vector3 touchStart;
	[SerializeField]
	private float _zoomSpeed;
	[SerializeField]
	private float _zoomOutMin;
	[SerializeField]
	private float _zoomOutMax;


	public void SetMovableState(bool isMovable) => _isMovable = isMovable;
	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}

		if (Input.touchCount == 2)
		{
			Touch touchZero = Input.GetTouch(0);
			Touch touchOne = Input.GetTouch(1);

			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

			float difference = currentMagnitude - prevMagnitude;

			Zoom(difference * _zoomSpeed * Time.deltaTime);
		}

		else if (Input.GetMouseButton(0) && _isMovable)
		{
			Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Camera.main.transform.position += direction;
		}
		Zoom(Input.GetAxis("Mouse ScrollWheel"));
	}

	private void Zoom(float increment)
	{
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _zoomOutMin, _zoomOutMax);
	}
}
