using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ThrottleButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField]
	private Image _image;

	[SerializeField]
	private Sprite _unpressed;

	[SerializeField]
	private Sprite _pressed;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (!GameSession.Instance.isRacing)
		{
			GameSession.Instance.isRacing = true;
		}
		if (!GameSession.Instance.isRaceDone)
		{
			var carController = GameSession.Player;
			carController.AccelerateInput(true);
			_image.sprite = _pressed;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		var carController = GameSession.Player;
		carController.AccelerateInput(false);
		_image.sprite = _unpressed;
	}
}
