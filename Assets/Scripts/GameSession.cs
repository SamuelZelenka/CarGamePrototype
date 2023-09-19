using System.ComponentModel;
using UnityEngine;

public class GameSession : MonoBehaviour
{
	public bool isRacing;
	public bool isRaceDone;
	public float Timer => _timer;
	private float _timer;

	public static CarController Player => _instance._player;
	[SerializeField]
	private CarController _player;

	public static Path Path => _instance._path;
	[SerializeField]
	private Path _path;

	public int CurrentLap => _currentLap;
	[ReadOnly(true)]
	[SerializeField]
	private int _currentLap;

	public int TotalLaps => _totalLaps;

	[SerializeField]
	private int _totalLaps;
	public static GameSession Instance => _instance;
	private static GameSession _instance;

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		_instance = this;

		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		isRacing = false;
		isRaceDone = false;
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

	public void LapDone()
	{
		_currentLap++;
	}
}
