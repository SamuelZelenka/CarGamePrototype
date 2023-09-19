using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
	[SerializeField]
	private Path _path; 

	[SerializeField]
	private float _roadWidth = 2.0f; 

	[SerializeField]
	private int _subdivisions = 100; 

	[SerializeField]
	private Material _roadMaterial;

	[SerializeField]
	private Material _startLineMaterial;

	private MeshFilter _roadMeshFilter;
	private MeshRenderer _roadMeshRenderer;

	[SerializeField]
	private GameObject _roadObject;

	[SerializeField]
	private StartLine _startLine;

	private MeshFilter _startLineMeshFilter;
	private MeshRenderer _startLineMeshRenderer;

	private void Start()
	{
		GetMeshComponents(_roadObject, ref _roadMeshFilter, ref _roadMeshRenderer);
		GetMeshComponents(_startLine.gameObject, ref _startLineMeshFilter, ref _startLineMeshRenderer);
		GenerateStartLine();
		GenerateRoadMesh();
		_roadMeshRenderer.material = _roadMaterial;
	}

	private void GetMeshComponents(GameObject meshObject ,ref MeshFilter filter, ref MeshRenderer renderer)
	{
		filter = GetComponent<MeshFilter>();
		if (filter == null)
			filter = meshObject.AddComponent<MeshFilter>();

		renderer = GetComponent<MeshRenderer>();
		if (renderer == null)
			renderer = meshObject.AddComponent<MeshRenderer>();
	}

	private void GenerateStartLine()
	{
		Mesh startLineMesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector2> uv = new List<Vector2>();

		Vector3 splinePoint = _path.GetPos(0);
		Vector3 forward = _path.GetPos(0.01f) - splinePoint;
		Vector3 right = Quaternion.Euler(0, 90, 0) * forward.normalized;

		Vector3 vertex0 = splinePoint - right * (_roadWidth * 0.5f) + Vector3.up * 0.01f;
		Vector3 vertex1 = vertex0 + forward * (_roadWidth * 0.5f) + Vector3.up * 0.01f;
		Vector3 vertex2 = splinePoint + right * (_roadWidth * 0.5f) + Vector3.up * 0.01f;
		Vector3 vertex3 = vertex2 + forward * (_roadWidth * 0.5f) + Vector3.up * 0.01f;

		Vector2 uvTopLeft = new Vector2(0, 0);
		Vector2 uvTopRight = new Vector2(0.125f, 0);
		Vector2 uvBottomLeft = new Vector2(0, 1);
		Vector2 uvBottomRight = new Vector2(0.125f, 1);

		_startLine.SetStartLine(vertex0, vertex2);

		vertices.Add(vertex0);
		vertices.Add(vertex1);
		vertices.Add(vertex2);
		vertices.Add(vertex3);

		uv.Add(uvTopLeft);
		uv.Add(uvTopRight);
		uv.Add(uvBottomLeft);
		uv.Add(uvBottomRight);

		triangles.Add(0);
		triangles.Add(1);
		triangles.Add(2);

		triangles.Add(2);
		triangles.Add(1);
		triangles.Add(3);

		startLineMesh.vertices = vertices.ToArray();
		startLineMesh.triangles = triangles.ToArray();
		startLineMesh.uv = uv.ToArray();

		startLineMesh.RecalculateNormals();

		_startLineMeshRenderer.material = _startLineMaterial;

		_startLineMeshFilter.mesh = startLineMesh;
	}

	private void GenerateRoadMesh()
	{
		Mesh roadMesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector2> uv = new List<Vector2>(); 

		for (int i = 0; i <= _subdivisions; i++)
		{
			float t = ((float)i / _subdivisions) * _path.controlPoints.Length;

			Vector3 splinePoint = _path.GetPos(t);
			Vector3 forward = _path.GetPos(t + 0.01f) - splinePoint;
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

		_roadMeshFilter.mesh = roadMesh;
	}
}
