using UnityEngine;

public class ThrottleDebug : LineDebugToggle
{
	public override void UpdateDebug(CarController carController)
	{
		base.UpdateDebug(carController);
		if (toggle.isOn)
		{
			var p1 = carController.transform.position + carController.transform.right * 0.4f;
			var p2 = carController.GetAcceleration() + carController.transform.right * 0.4f;
			_text.text = "Throttle: " + Vector3.Magnitude(p1 - p2).ToString();
			_lineRenderer.SetPositions(new Vector3[] { p1, p2 });
		}
	}
}
