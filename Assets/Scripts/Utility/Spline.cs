using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spline
{
	public static Vector3 GetSplinePoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		Vector3 interpolatedPoint =
			0.5f * ((2 * p1) +
			(-p0 + p2) * t +
			(2 * p0 - 5 * p1 + 4 * p2 - p3) * t2 +
			(-p0 + 3 * p1 - 3 * p2 + p3) * t3);

		return interpolatedPoint;
	}
}
