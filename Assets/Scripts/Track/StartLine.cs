using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class StartLine : MonoBehaviour
{
	public void SetStartLine(Vector3 p0, Vector3 p1)
	{
		Vector3 centerPosition = (p0 + p1) / 2f;

		GameObject finishLineObject = new GameObject("FinishLine");
		finishLineObject.transform.position = new Vector3(transform.position.x, transform.position.y - 2.5f, transform.position.z);

		MeshCollider collider = GetComponent<MeshCollider>();
		collider.convex = true;
		collider.isTrigger = true;

		MeshFilter meshFilter = finishLineObject.AddComponent<MeshFilter>();

		Vector3[] vertices = new Vector3[4];
		vertices[0] = p0 * 1.01f; //Offset point slightly for QuickHull Algorithm in PhysX
		vertices[1] = p1; //Offset point slightly for QuickHull Algorithm in PhysX
		vertices[2] = p0 + Vector3.up * 5f; 
		vertices[3] = p1 + Vector3.up * 5f;

		int[] triangles = 
		{ 
			0, 1, 2,
			1, 3, 2 
		};

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		meshFilter.mesh = mesh;
		collider.sharedMesh = mesh;

		finishLineObject.transform.parent = transform;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!GameSession.Instance.isRacing)
		{
			return;
		}
		GameSession.Instance.LapDone();
	}
}
