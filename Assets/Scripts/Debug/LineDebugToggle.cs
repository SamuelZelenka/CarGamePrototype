using UnityEngine;

public abstract class LineDebugToggle : DebugToggle
{
	[SerializeField]
	protected LineRenderer _lineRenderer;
	public override void UpdateDebug(CarController chaser)
	{
		_lineRenderer.enabled = toggle.isOn;
	}
}


