using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{

	[SerializeField]
	DebugToggle[] _toggles = new DebugToggle[0];

	private CarController carController => GameSession.Player;
	private void Update()
	{
		foreach (var toggle in _toggles)
		{
			toggle.UpdateDebug(carController);
		}
	}
}