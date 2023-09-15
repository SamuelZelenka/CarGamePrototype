using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TotalForceDebug : LineDebugToggle
{
	public override void UpdateDebug(CarController chaser)
	{
		base.UpdateDebug(chaser);
		if (toggle.isOn)
		{
			var p1 = chaser.transform.position;
			var p2 = chaser.GetForce();
			_text.text = "Force: " + Vector3.Magnitude(p1 - p2).ToString() + "\n" + (p2 - chaser.transform.position);
			_lineRenderer.SetPositions(new Vector3[] { p1, p2 });
		}
	}
}
