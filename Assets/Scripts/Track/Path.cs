using UnityEngine;

public class Path : MonoBehaviour
{
	[SerializeField]
	public Transform[] controlPoints;
	public Vector3 GetPos(float t)
	{
		int section = (int)t;

		var p0 = controlPoints[GetClampedPointIndex(section)].position;
		var p1 = controlPoints[GetClampedPointIndex(section + 1)].position;
		var p2 = controlPoints[GetClampedPointIndex(section + 2)].position;
		var p3 = controlPoints[GetClampedPointIndex(section + 3)].position;

		return GetSplinePoint(p0, p1, p2, p3, t - section);
	}

	private int GetClampedPointIndex(int point)
	{
		return point % controlPoints.Length;
	}

	Vector3 GetSplinePoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
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

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		var prevPos = GetPos(0);
		for (float i = 0.01f; i < controlPoints.Length; i += 0.1f)
		{
			var nextPos = GetPos(i);
			Gizmos.DrawLine(prevPos, nextPos);
			prevPos = nextPos;
		}
	}
#endif
}
