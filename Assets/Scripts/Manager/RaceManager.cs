using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
	public bool isRacing;
	public bool isRaceDone;
	public float Timer => _timer;
	private float _timer;

	public int CurrentLap => _currentLap;
	[ReadOnly]
	[SerializeField]
	private int _currentLap;

	public int TotalLaps => _totalLaps;

	[SerializeField]
	private int _totalLaps;


	void Start()
	{
		if (GameManager.TrackManager.GetCurrentTrack() == null)
			return;
	}

	void Update()
	{
		if (isRacing && !isRaceDone)
		{
			_timer += Time.deltaTime;
		}
		if (_currentLap == _totalLaps)
		{
			isRacing = false;
			isRaceDone = true;
		}
	}

	public void ResetRace()
	{
		InitRace();
		_timer = 0;
		_currentLap = 0;
		GameManager.PlayerManager.ResetRace();
	}

	public void InitRace()
	{
		_totalLaps = GameManager.TrackManager.GetCurrentTrack().TotalLaps;
		isRacing = false;
		isRaceDone = false;
	}

	public void LapDone()
	{
		_currentLap++;
	}
}
