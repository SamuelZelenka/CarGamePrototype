using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapsUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _lapsText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var current = GameSession.Instance.CurrentLap;

		var total = GameSession.Instance.TotalLaps;
        _lapsText.text = $"{current}/{total}";

	}
}
