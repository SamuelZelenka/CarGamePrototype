using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AccelerateButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private Chaser _chaser;

	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite _unpressed;

	[SerializeField]
	private Sprite _pressed;

	public void OnPointerDown(PointerEventData eventData)
	{
		_chaser.AccelerateInput(true);
		_image.sprite = _pressed;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_chaser.AccelerateInput(false);
		_image.sprite = _unpressed;
	}
}