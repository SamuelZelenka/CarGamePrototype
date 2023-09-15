using UnityEngine;

public class SlideForceDebug : LineDebugToggle
{
	public override void UpdateDebug(CarController chaser)
	{
		base.UpdateDebug(chaser);
		if (toggle.isOn)
		{
			var p1 = chaser.transform.position;
			var p2 = chaser.GetSlidingCorrection();
			_text.text = "Sliding: " + Vector3.Magnitude(p1 - p2).ToString();
			_lineRenderer.SetPositions(new Vector3[] { p1, p2 });
		}
	}
}
