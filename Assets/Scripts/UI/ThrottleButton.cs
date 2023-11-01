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
		if (!GameManager.RaceManager.isRacing)
		{
			GameManager.RaceManager.isRacing = true;
		}
		if (!GameManager.RaceManager.isRaceDone)
		{
			var carController = GameManager.PlayerManager.player;
			carController.AccelerateInput(true);
			_image.sprite = _pressed;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		var carController = GameManager.PlayerManager.player;
		carController.AccelerateInput(false);
		_image.sprite = _unpressed;
	}
}
