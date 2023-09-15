public class TargetDebug : VisualToggleDebug
{
	public override void UpdateDebug(CarController chaser)
	{
		_meshRenderer.enabled = toggle.isOn;
	}
}

