using TMPro;
using UnityEngine;

public class LapsUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _lapsText;

    void Update()
    {
        var current = GameSession.Instance.CurrentLap;

		var total = GameSession.Instance.TotalLaps;
        _lapsText.text = $"{current}/{total}";
	}
}
