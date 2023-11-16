using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrackData
{
	[SerializeField]
	private string _trackName;
	[SerializeField]
	private int _totalLaps;

	[SerializeField]
	private List<Vector3> _controlPoints = new List<Vector3>
	{
		new Vector3 (50f, 0f, 50f),
		new Vector3 (50f, 0f, -50f),
		new Vector3 (-50f, 0f, -50f),
		new Vector3 (-50f, 0f, 50f),
	};

	public TrackData(string trackName)
	{
		_trackName = trackName;
		_totalLaps = 1;
	}

	public string TrackName => _trackName;
	public List<Vector3> ControlPoints => _controlPoints;
	public int TotalLaps => _totalLaps;

	public void FlipTrackDirection()
	{
		_controlPoints.Reverse();

		_controlPoints.OffsetListBackward(3);
		GameManager.TrackManager.SaveTrack();
	}
}
