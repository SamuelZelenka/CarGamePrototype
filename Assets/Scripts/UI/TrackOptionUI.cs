using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class TrackOptionUI : MonoBehaviour
{
	[SerializeField]
	protected Image image;

	protected float alpha;

	public abstract void Init(int index);
	public virtual void SetAlpha(float alpha)
	{
		var bannerColor = image.color;
		image.color = new Color(bannerColor.r, bannerColor.g, bannerColor.b, alpha);
	}
}
