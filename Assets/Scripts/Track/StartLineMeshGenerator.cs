using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLineMeshGenerator : MonoBehaviour
{
	[SerializeField]
	private Material _material;

	[SerializeField]
	private MeshFilter _startLineMeshFilter;

	[SerializeField]
	private MeshRenderer _startLineMeshRenderer;

	[SerializeField]
	private MeshFilter _triggerMeshFilter;

	[SerializeField]
	private MeshCollider _triggerCollider;

	public void GenerateStartLine(List<Vector3> controlPoints, float width)
	{
		Mesh startLineMesh = new Mesh();

		List<Vector3> vertices = new List<Vector3>();
		List<int> triangles = new List<int>();
		List<Vector2> uv = new List<Vector2>();

		Vector3 splinePoint = Path.GetPos(0, controlPoints);
		Vector3 forward = Path.GetPos(0.01f, controlPoints) - splinePoint;
		Vector3 right = Quaternion.Euler(0, 90, 0) * forward.normalized;

		Vector3 vertex0 = splinePoint - right * (width * 0.5f) + Vector3.up * 0.01f;
		Vector3 vertex1 = vertex0 + forward * (width * 0.5f) + Vector3.up * 0.01f;
		Vector3 vertex2 = splinePoint + right * (width * 0.5f) + Vector3.up * 0.01f;
		Vector3 vertex3 = vertex2 + forward * (width * 0.5f) + Vector3.up * 0.01f;

		Vector2 uvTopLeft = new Vector2(0, 0);
		Vector2 uvTopRight = new Vector2(0.125f, 0);
		Vector2 uvBottomLeft = new Vector2(0, 1);
		Vector2 uvBottomRight = new Vector2(0.125f, 1);

		GenerateStartTrigger(vertex0, vertex1, vertex2, vertex3);

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

		_startLineMeshRenderer.material = _material;
		_startLineMeshFilter.mesh = startLineMesh;
	}

	private void GenerateStartTrigger(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
	{
		Vector3 centerPosition = (p0 + p1) / 2f;

		_triggerCollider.convex = true;
		_triggerCollider.isTrigger = true;

		Vector3[] vertices = new Vector3[8];
		vertices[0] = p0;
		vertices[1] = p1;
		vertices[2] = p2;
		vertices[3] = p3;
		vertices[4] = p0 + Vector3.up;
		vertices[5] = p1 + Vector3.up;
		vertices[6] = p2 + Vector3.up;
		vertices[7] = p3 + Vector3.up;

		int[] triangles =
		{
			0, 2, 1, // Bottom face
			2, 3, 1,
			4, 5, 6, // Top face
			5, 7, 6,
			0, 4, 2, // Side faces
			4, 6, 2,
			1, 3, 5,
			3, 7, 5
		};

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		_triggerMeshFilter.mesh = mesh;
		_triggerCollider.sharedMesh = mesh;
	}
}
