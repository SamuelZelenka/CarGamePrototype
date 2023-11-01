using UnityEngine;

public class DebugPanel : MonoBehaviour
{
	[SerializeField]
	DebugToggle[] _toggles = new DebugToggle[0];

	private CarController carController => GameManager.PlayerManager.player;
	private void Update()
	{
		foreach (var toggle in _toggles)
		{
			toggle.UpdateDebug(carController);
		}
	}
}