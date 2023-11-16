using UnityEngine;
using UnityEngine.UI;

public class EditModeToggle : MonoBehaviour
{
	[SerializeField]
	private RunTimePathEditor _pathEditor;

	[SerializeField]
	private Button _editButton;
	[SerializeField]
	private Button _placeButton;
	[SerializeField]
	private Button _removeButton;

	public void SetEditMode(int mode)
	{
		_pathEditor.SetEditMode((PathEditMode)mode);
		UpdateButtonColor(_editButton, PathEditMode.Edit);
		UpdateButtonColor(_placeButton, PathEditMode.Place);
		UpdateButtonColor(_removeButton, PathEditMode.Remove);
	}

	private void UpdateButtonColor(Button button, PathEditMode mode)
	{

		if (button != null)
		{
			if (mode == _pathEditor.GetEditMode())
			{
				button.image.color = new Color32(0xEA, 0x62, 0x62, 0xFF);
			}
			else
			{
				button.image.color = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
			}
		}
	}
}
