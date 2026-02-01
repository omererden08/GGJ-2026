using UnityEngine;

public class MatchMaskManager : MonoBehaviour
{
	public static MatchMaskManager Instance;

	[Header("Mask Match")]
	[SerializeField] private GrabMask grabbableMask;
	[SerializeField] private Transform targetPosition;
	[SerializeField] private GameObject mask2;
	[SerializeField] private string sceneName;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	// Try to match a single mask to its target position (no arrays)
	public bool TryMatch(GrabMask obj, Transform candidatePos, out Transform correctPos)
	{
		correctPos = null;
		if (obj == grabbableMask && candidatePos == targetPosition)
		{
			correctPos = targetPosition;
			CheckForMatch();
			return true;
		}

		return false;
	}

	// If the single mask is placed at the target, advance the scene
	private void CheckForMatch()
	{
		if (grabbableMask == null || targetPosition == null) return;

		if (grabbableMask.transform.position != targetPosition.position) return;

		Debug.Log("✅ Mask placed correctly. Lets gooo");
        GameManager.Instance.storyScore += 1;
		mask2.SetActive(true);
	}


	// Called by the mask when it was placed correctly (after OnDrop)
	public void NotifyMaskPlaced()
	{
		CheckForMatch();
	}
	// Optional setter for runtime assignment
	public void SetTarget(GrabMask mask, Transform target)
	{
		grabbableMask = mask;
		targetPosition = target;
	}
}

