using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    void Update()
    {
		float totalTime = GameManager.RaceManager.Timer;
		int minutes = Mathf.FloorToInt(totalTime / 60);
		int seconds = Mathf.FloorToInt(totalTime % 60);
		int milliseconds = Mathf.FloorToInt((totalTime * 1000) % 1000); 

		string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D3}", minutes, seconds, milliseconds);

        _text.text = formattedTime;
    }
}
