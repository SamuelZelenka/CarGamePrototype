using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackSelectionUI : MonoBehaviour
{
    [SerializeField]
    private float _tOffset;

    [SerializeField]
    private Transform _p0, _p1, _p2, _p3;

	[SerializeField]
	private TrackOptionUI _optionPrefab;
    [SerializeField]
	private List<TrackOptionUI> _options;
	[SerializeField]
	private float _currentT;

	private Vector2 touchStartPos;
	private float touchStartValue;

	public float scrollSpeed;
	private void OnEnable()
	{
		GameManager.GameEventManager.OnTrackCreated += CreateOption;
	}

	private void Start()
	{
		CreateOptions();
	}
	void Update()
    {
		CheckInput();
		UpdatePosition();
	}

	private void CreateOption(int index)
	{
		var trackOption = Instantiate(_optionPrefab, transform);
		trackOption.Init(index);
		_options.Add(trackOption);
	}
	private void CreateOptions()
	{
		var tracks = GameManager.TrackManager.GetTracks();
		for (int i = 0; i < tracks.Count; i++)
		{
			CreateOption(i);
		}
	}

    private void CheckInput()
    {
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);

			if (touch.phase == TouchPhase.Began)
			{
				touchStartPos = touch.position;
				touchStartValue = _currentT;
			}
			else if (touch.phase == TouchPhase.Moved)
			{
				Vector2 delta = touch.position - touchStartPos;
				float scrollDelta = delta.y * scrollSpeed * Time.deltaTime;

				_currentT = Mathf.Clamp( touchStartValue - scrollDelta, _options.Count * -0.1f, 1);
			}
		}
	}

	private void UpdatePosition()
	{
		for (int i = 0; i < _options.Count; i++)
		{
			var t = _currentT + _tOffset * i;
			var nextPos = Spline.GetSplinePoint(_p0.position, _p1.position, _p2.position, _p3.position, t);
			_options[i].SetAlpha(Mathf.Abs(t + 0.5f));
			_options[i].transform.position = nextPos;
		}
	}
}
