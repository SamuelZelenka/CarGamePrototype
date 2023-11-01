using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	private AudioManager _audioManager;
	[SerializeField]
	private CameraManager _cameraManager;
	[SerializeField]
	private PlayerManager _playerManager;
	[SerializeField]
	private TrackManager _trackManager;
	[SerializeField]
	private RaceManager _raceManager;
	[SerializeField]
	private EnvironmentManager _environmentManager;
	[SerializeField]
	private GameEventManager _gameEventManager;

	private static GameManager _instance;
	public static GameManager Instance
	{
		get 
		{
			if (_instance != null)
			{
				return _instance;
			}
			_instance = GameObject.FindObjectOfType<GameManager>();
			return _instance; 
		}
	}

	public static AudioManager AudioManager => Instance._audioManager;
	public static CameraManager CameraManager => Instance._cameraManager;
	public static PlayerManager PlayerManager => Instance._playerManager;
	public static RaceManager RaceManager => Instance._raceManager;
	public static TrackManager TrackManager => Instance._trackManager;
	public static EnvironmentManager EnvironmentManager => Instance._environmentManager;
	public static GameEventManager GameEventManager => Instance._gameEventManager;

	public void Awake()
	{
		if (Instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
}