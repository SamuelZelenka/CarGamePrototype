public class TrackEnvironment : Environment
{
	public override void Load()
	{
		base.Load();
		GameManager.RaceManager.ResetRace();
	}

}