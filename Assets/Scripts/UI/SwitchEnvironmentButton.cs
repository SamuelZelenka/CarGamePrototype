
using UnityEngine;

public class SwitchEnvironmentButton : MonoBehaviour
{
	public Environment environment;
	public void ChangeScene()
	{
		GameManager.EnvironmentManager.LoadEnvironment(environment.EnvironmentName);
	}
}
