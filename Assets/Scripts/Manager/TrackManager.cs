using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
	private List<TrackData> _trackList;
	private int _currentTrackIndex;

	private void Awake()
	{
		_trackList = SaveSystem.LoadFilesInDirectory<TrackData>(SaveSystem.TRACK_DATA_DIR);
	}

	public List<Vector3> ControlPoints => _trackList[_currentTrackIndex].ControlPoints;
	public int GetCurrentTrackIndex() => GetTrackIndex(GetCurrentTrack());
	public TrackData GetCurrentTrack()
	{
		if (_trackList?.Count == 0)
		{
			return null;
		}
		return _trackList[_currentTrackIndex];
	}
	public void SetCurrentTrack(int index) => _currentTrackIndex = index;
	public TrackData GetTrack(int index) => _trackList[index];
	public List<TrackData> GetTracks() => _trackList;

	public void SaveTrack()
	{
		var track = GetCurrentTrack();
		SaveSystem.SaveData(SaveSystem.TRACK_DATA_DIR, track.TrackName, track);
	}

	public int GetTrackIndex(TrackData trackData)
	{
		for (int i = 0; i < _trackList.Count; i++)
		{
			if (_trackList[i] == trackData)
			{
				return i;
			}
		}
		return -1;
	}

	public int CreateNewTrack(string trackName)
	{
		var data = new TrackData(trackName);
		_trackList.Add(data);
		var trackIndex = _trackList.Count - 1;
		SaveSystem.SaveData(SaveSystem.TRACK_DATA_DIR, trackName, data);
		GameManager.GameEventManager.OnTrackCreated?.Invoke(trackIndex);
		return trackIndex;
	}
}
