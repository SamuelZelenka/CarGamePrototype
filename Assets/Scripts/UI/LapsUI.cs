using TMPro;
using UnityEngine;

public class LapsUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _lapsText;

    void Update()
    {
        var current = GameManager.RaceManager.CurrentLap;

		var total = GameManager.RaceManager.TotalLaps;
        _lapsText.text = $"{current} / {total}";
	}
}
