using UnityEngine;

public class ControlPoint : MonoBehaviour
{
	private Vector3 offset;
	private bool isDragging = false;

	RunTimePathEditor _pathEditor;
	EditorCameraController _cameraController;

	public void SetCameraController(EditorCameraController cameraController)
	{
		_cameraController = cameraController;
	}
	public void SetPathEditor(RunTimePathEditor editor)
	{
		_pathEditor = editor;
	}

	private void OnMouseDown()
	{
		_cameraController.SetMovableState(false);
		if (Input.GetMouseButtonDown(0))
		{
			switch (_pathEditor.GetEditMode())
			{
				case PathEditMode.Edit:
					OnEditTouch();
					break;
				case PathEditMode.Remove:
					OnRemoveTouch();
					break;
				default:
					break;
			}
		}
	}

	private void OnMouseDrag()
	{
		if (isDragging)
		{
			// Move the Transform based on the mouse position.
			Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
			transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
		}
	}

	private void OnMouseUp()
	{
		_cameraController.SetMovableState(true);
		isDragging = false;
	}


	private void OnEditTouch()
	{
		if (_pathEditor.GetEditMode() != PathEditMode.Edit)
			return;

		offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
		isDragging = true;
	}
	private void OnRemoveTouch()
	{
		if (GameManager.TrackManager.ControlPoints.Count <= 4)
			return;

		_pathEditor.RemoveControlPoint(transform);
		Destroy(gameObject);
	}
}
