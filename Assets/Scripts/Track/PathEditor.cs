#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

public class PathEditor : Editor
{

	private void OnDisable()
	{
		Tools.hidden = false;
	}

	//private void OnSceneGUI()
	//{
	//	var path = (Path)target;
	//	Tools.hidden = true;
	//	if (GameSession.TrackData.ControlPoints != null)
	//	{
	//		Handles.color = Color.green;
	//		var controlPoints = GameSession.TrackData.ControlPoints;
	//		for (int i = 0; i < controlPoints.Count; i++)
	//		{
	//			controlPoints[i] = Handles.DoPositionHandle(controlPoints[i],Quaternion.identity);
	//			var labelStyle = new GUIStyle();
	//
	//			if (i == 1)
	//			{
	//				labelStyle.normal.textColor = Color.green;
	//				Handles.Label(controlPoints[i], "StartingLine", labelStyle);
	//			}
	//			else
	//			{
	//				labelStyle.normal.textColor = Color.black;
	//				Handles.Label(controlPoints[i], "Point " + i, labelStyle);
	//			}
	//		}
	//	}
	//	if (GUI.changed)
	//	{
	//		path.GetComponent<RoadGenerator>().Generate();
	//	}
	//}

	private void OnUpdateHandlePos()
	{

	}
}
#endif