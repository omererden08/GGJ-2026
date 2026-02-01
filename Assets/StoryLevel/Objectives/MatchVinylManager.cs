using UnityEngine;

public class MatchVinylManager : MonoBehaviour
{
	public static MatchVinylManager Instance;

	[Header("Vinyl Match")]
	[SerializeField] private GrabVinyl grabbableVinyl;
	[SerializeField] private Transform targetPosition;
	[SerializeField] private string sceneName;
	[SerializeField] GameObject gramafon1;
	[SerializeField] GameObject gramafon2;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	// Try to match a single vinyl to its target position (no arrays)
	public bool TryMatch(GrabVinyl obj, Transform candidatePos, out Transform correctPos)
	{
		correctPos = null;
		if (obj == grabbableVinyl && candidatePos == targetPosition)
		{
			correctPos = targetPosition;
			CheckForMatch();
			return true;
		}

		return false;
	}

	// If the single vinyl is placed at the target, advance the scene
	private void CheckForMatch()
	{
		if (grabbableVinyl == null || targetPosition == null) return;

		if (grabbableVinyl.transform.position != targetPosition.position) return;

		Debug.Log("✅ Vinyl placed correctly. Lets gooo");
		//game objective yok et
		gramafon1.SetActive(false);
		//game objective çağır
		gramafon2.SetActive(true);
		//müzik oynamaya başlat
		AudioManager.Instance.PlayMusic(2);

        GameManager.Instance.storyScore += 1;
	}


	// Called by the vinyl when it was placed correctly (after OnDrop)
	public void NotifyVinylPlaced()
	{
		CheckForMatch();
	}
	// Optional setter for runtime assignment
	public void SetTarget(GrabVinyl vinyl, Transform target)
	{
		grabbableVinyl = vinyl;
		targetPosition = target;
	}
}

