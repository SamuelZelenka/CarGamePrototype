using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	public CarController player;

	public void ResetRace()
	{
		player.ResetCar();
	}
}
