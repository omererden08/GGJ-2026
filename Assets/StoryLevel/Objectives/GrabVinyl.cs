using UnityEngine;

public class GrabVinyl : MonoBehaviour
{
	private Vector3 originalPosition;
	private Rigidbody2D rb2d;
	private Collider2D col2d;

	private void Awake()
	{
		originalPosition = transform.position;
		rb2d = GetComponent<Rigidbody2D>();
		col2d = GetComponent<Collider2D>();
	}

	// Keep this parameterized overload for compatibility with older code
	public void OnPickup(Transform newParent)
	{
		transform.SetParent(newParent);
		transform.localPosition = Vector3.zero;
		OnPickup();
		Debug.Log($"✅ {name} picked up (parented).");
	}

	// No-arg pickup used by newer ObjectHolder (position-based)
	public void OnPickup()
	{
		if (rb2d != null) rb2d.simulated = false;
		if (col2d != null) col2d.enabled = false;
		Debug.Log($"✅ {name} picked up.");
	}

	public void OnDrop(bool matched, Transform matchedTarget = null)
	{
		transform.SetParent(null);
		if (rb2d != null) rb2d.simulated = true;
		if (col2d != null) col2d.enabled = true;

		if (matched && matchedTarget != null)
		{
			transform.position = matchedTarget.position;
			Debug.Log($"✅ {name} correctly matched.");
			// Notify manager and destroy the vinyl now that it's correctly placed
			if (MatchVinylManager.Instance != null)
				MatchVinylManager.Instance.NotifyVinylPlaced();
			Destroy(gameObject);
		}
		else
		{
			transform.position = originalPosition;
			Debug.Log($"🔁 {name} returned to original position.");
		}
	}
}

