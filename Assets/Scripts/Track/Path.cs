using UnityEngine;
using System.Collections.Generic;



public static class Path
{
	public static Vector3 GetPos(float t, List<Vector3> controlPoints)
	{
		int section = (int)t;

		var p0 = controlPoints[GetClampedPointIndex(section)];
		var p1 = controlPoints[GetClampedPointIndex(section + 1)];
		var p2 = controlPoints[GetClampedPointIndex(section + 2)];
		var p3 = controlPoints[GetClampedPointIndex(section + 3)];

		return Spline.GetSplinePoint(p0, p1, p2, p3, t - section);
	}

	private static int GetClampedPointIndex(int point)
	{
		return point % GameManager.TrackManager.ControlPoints.Count;
	}
}
