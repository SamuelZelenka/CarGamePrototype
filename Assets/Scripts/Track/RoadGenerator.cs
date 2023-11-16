
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
	[SerializeField]
	private float _roadWidth = 2.0f; 

	[SerializeField]
	private int _subdivisions = 100; 

	[SerializeField]
	private Material _roadMaterial;

	[SerializeField]
	private MeshFilter _roadMeshFilter;

	[SerializeField]
	private MeshRenderer _roadMeshRenderer;

	[SerializeField]
	private StartLineMeshGenerator _startLineGenerator;

	private void Awake()
	{
		GameManager.GameEventManager.OnCurrentTrackChanged += Generate;
	}

	public void Generate(int trackId)
	{
		var controlPoints = GameManager.TrackManager.ControlPoints;
		_startLineGenerator.GenerateStartLine(controlPoints, _roadWidth);
		_roadMeshFilter.mesh = GenerateRoadMesh();
		_roadMeshRenderer.material = _roadMaterial;
	}

	private Mesh GenerateRoadMesh()
	{
		Mesh roadMesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector2> uv = new List<Vector2>();

		var controlPoints = GameManager.TrackManager.ControlPoints;

		for (int i = 0; i <= _subdivisions; i++)
		{
			float t = ((float)i / _subdivisions) * controlPoints.Count;

			Vector3 splinePoint = Path.GetPos(t, controlPoints);
			Vector3 forward = Path.GetPos(t + 0.01f, controlPoints) - splinePoint;
			Vector3 right = Quaternion.Euler(0, 90, 0) * forward.normalized;

			Vector3 corner1 = splinePoint - right * _roadWidth * 0.5f;
			Vector3 corner2 = splinePoint + right * _roadWidth * 0.5f;

			vertices.Add(corner1);
			vertices.Add(corner2);

			uv.Add(new Vector2(0, 0));
			uv.Add(new Vector2(1, 1));

			if (i < _subdivisions)
			{
				int v0 = i * 2; 
				int v1 = i * 2 + 1; 
				int v2 = (i + 1) * 2; 
				int v3 = (i + 1) * 2 + 1;

				triangles.Add(v0);
				triangles.Add(v2);
				triangles.Add(v1);

				triangles.Add(v1);
				triangles.Add(v2);
				triangles.Add(v3);
			}
		}

		roadMesh.vertices = vertices.ToArray();
		roadMesh.triangles = triangles.ToArray();
		roadMesh.uv = uv.ToArray(); 

		roadMesh.RecalculateNormals();

		return roadMesh;
	}
}
