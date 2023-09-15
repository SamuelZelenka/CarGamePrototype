using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class DebugToggle : MonoBehaviour
{
	[SerializeField]
	public Toggle toggle;
	[SerializeField]
	protected TMP_Text _text;
	public abstract void UpdateDebug(CarController chaser);
}
