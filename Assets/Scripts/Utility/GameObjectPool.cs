using UnityEngine;

public class GameObjectPool : ObjectPool<GameObject>
{
	private GameObject prefab;
	private Transform parent;

	public override GameObject Acquire()
	{
		GameObject acquired = base.Acquire();
		acquired.SetActive(true);
		return acquired;
	}

	public override void Release(GameObject releaseObject)
	{
		if (GetPoolSize() > capacity)
		{
			GameObject.Destroy(releaseObject);
			return;
		}
		pool.Enqueue(releaseObject);
		releaseObject.SetActive(false);
	}
	public GameObjectPool(GameObject prefab, Transform parent)
	{
		this.parent = parent;
		this.prefab = prefab;
		OnCreate = () => GameObject.Instantiate(prefab.gameObject, parent);
	}
}