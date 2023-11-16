using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class RunTimePathEditor : MonoBehaviour
{
    private PathEditMode _currentEditMode;

    [SerializeField]
    private ControlPoint _controlPointPrefab;



    private List<ControlPoint> _controlPoints = new List<ControlPoint>();
    private List<Vector3> _previousPositions = new List<Vector3>();

    [SerializeField]
    private RoadGenerator _roadGenerator;

	[SerializeField]
	private EditorCameraController _cameraController;

	public PathEditMode GetEditMode()
	{
		return _currentEditMode;
	}
	public void SetEditMode(PathEditMode editMode)
    {
        _currentEditMode = editMode;
    }

    public void Init()
    {
		var controlPoints = GameManager.TrackManager.ControlPoints.ToArray();
		GameManager.TrackManager.ControlPoints.Clear();
		var count = controlPoints.Length;
		for (int i = 0; i < count; i++)
		{
			CreateControlPoint(controlPoints[i], i);
		}
	}

    public void Unload()
    {
        while (_controlPoints.Count > 0)
        {
            Destroy(_controlPoints[0].gameObject); // Consider object pooling
            _controlPoints.RemoveAt(0);
        }
    }


    private void Update()
    {
        OnTouch();

        if (PositionHasChanged())
        {
            for (int i = 0; i < _controlPoints.Count; i++)
            {
                GameManager.TrackManager.ControlPoints[i] = _controlPoints[i].transform.position;
                _roadGenerator.Generate(GameManager.TrackManager.GetCurrentTrackIndex());
            }

            GameManager.TrackManager.SaveTrack();

			_previousPositions.Clear();
            _previousPositions.AddRange(GameManager.TrackManager.ControlPoints);
        }
    }

	private void OnTouch()
    {
        bool isHoveringUI = EventSystem.current.IsPointerOverGameObject();
        if (Input.GetMouseButtonDown(0) && _currentEditMode == PathEditMode.Place && !isHoveringUI)
        {
            AddControlPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    private void AddControlPoint(Vector3 position)
    {
        int closestIndex = 0;
        for (int i = 1; i < _controlPoints.Count; i++)
        {
            var distToPoint = (_controlPoints[i].transform.position - position).sqrMagnitude;
            var distToClosestPoint = (_controlPoints[closestIndex].transform.position - position).sqrMagnitude;

            if (distToPoint < distToClosestPoint)
            {
                closestIndex = i;
            }
        }

        var clampedUpIndex = ClampIndex(closestIndex + 1, 0, _controlPoints.Count - 1);
        var clampedDownIndex = ClampIndex(closestIndex - 1, 0, _controlPoints.Count - 1);
        var distToNextPoint = (_controlPoints[clampedUpIndex].transform.position - position).sqrMagnitude;
        var distToPrevPoint = (_controlPoints[clampedDownIndex].transform.position - position).sqrMagnitude;
        var insertIndex = distToNextPoint < distToPrevPoint ? closestIndex + 1  : closestIndex;
        insertIndex = ClampIndex(insertIndex, 0, _controlPoints.Count - 1);

        CreateControlPoint(position, insertIndex);
		GameManager.TrackManager.SaveTrack();

	}

    public void RemoveControlPoint(Transform removeTransform)
    {
        for (int i = 0; i < _controlPoints.Count; i++)
        {
            if (_controlPoints[i] == removeTransform)
            {
                _previousPositions.RemoveAt(i);
                GameManager.TrackManager.ControlPoints.RemoveAt(i);
                _controlPoints.RemoveAt(i);
                _roadGenerator.Generate(GameManager.TrackManager.GetCurrentTrackIndex());
                GameManager.TrackManager.SaveTrack();
                return;
            }
        }
        UpdateControlPoints();
	}

    private int ClampIndex(int index, int min, int max)
    {
        return (index - min + max + 1) % (max - min + 1) + min;
    }

    private void CreateControlPoint(Vector3 position, int index)
    {
        position.y = 0;
        var newControlpoint = Instantiate(_controlPointPrefab, position, _controlPointPrefab.transform.rotation, transform);
        newControlpoint.name = "ControlPoint " + index;
        
        _controlPoints.Insert(index, newControlpoint);
        _previousPositions.Insert(index, position);
        GameManager.TrackManager.ControlPoints.Insert(index, position);
		newControlpoint.SetCameraController(_cameraController);
        newControlpoint.SetPathEditor(this);
        _roadGenerator.Generate(GameManager.TrackManager.GetCurrentTrackIndex());
        UpdateControlPoints();

	}

	public void FlipTrackDirection()
	{
		GameManager.TrackManager.ControlPoints.Reverse();
		GameManager.TrackManager.ControlPoints.OffsetListBackward(3);
        GameManager.TrackManager.SaveTrack();
        UpdateControlPoints();
    }

    private void UpdateControlPoints()
    {
        var count = _controlPoints.Count;
        for (int i = 0; i < count; i++)
        {
            _controlPoints[i].transform.position = GameManager.TrackManager.ControlPoints[i];
            var convertedIndex = ((i - 1) % count + count) % count; ;
			_controlPoints[i].SetIndex(convertedIndex);
		}
	}
	private bool PositionHasChanged()
	{
        if (_controlPoints.Count != _previousPositions.Count)
            return true;

        for (int i = 0; i < _controlPoints.Count; i++)
        {
            if (_previousPositions[i] != _controlPoints[i].transform.position)
            { 
            
                return true;
            }
        }

        return false;
    }
}

public enum PathEditMode 
{ 
    Edit, 
    Place, 
    Remove 
}