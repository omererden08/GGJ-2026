using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public void OnPickup(Transform newParent)
    {
        transform.SetParent(newParent);
        transform.localPosition = Vector3.zero;

        Debug.Log($"✅ {name} picked up.");
    }

    public void OnDrop(bool matched, Transform matchedTarget = null)
    {
        transform.SetParent(null);

        if (matched && matchedTarget != null)
        {
            transform.position = matchedTarget.position;
            Debug.Log($"✅ {name} correctly matched.");
        }
        else
        {
            transform.position = originalPosition;
            Debug.Log($"🔁 {name} returned to original position.");
        }
    }
}
