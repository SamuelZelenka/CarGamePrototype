using UnityEngine;

public class StartLine : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (!GameManager.RaceManager.isRacing)
		{
			return;
		}
		GameManager.RaceManager.LapDone();
	}
}
