using UnityEngine;

public class EditorEnvironment : Environment
{
	[SerializeField]
	private RunTimePathEditor _runTimePathEditor;

	public override void Load()
	{
		base.Load();
		_runTimePathEditor.SetEditMode(0);
		_runTimePathEditor.Init();
	}

	public override void Unload()
	{
		base.Unload();
		_runTimePathEditor.Unload();
	}
}
