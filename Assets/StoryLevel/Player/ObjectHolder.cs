using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    
    private Player player;
    private GrabVinyl currentVinyl = null;
    private Transform currentVinylSlot = null;
    private GrabVinyl heldVinyl = null;
    private GrabVinyl currentMask = null;
    private Transform currentMaskSlot = null;
    private GrabVinyl heldMask = null;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (heldVinyl == null)
                TryPickupVinyl();
            else
                DropVinyl();
            if (heldMask == null)
                TryPickupMask();
            else
                DropMask();
        }
    }

    private void TryPickupVinyl()
    {
        if (currentVinyl == null) return;

        heldVinyl = currentVinyl;
        // Parent the vinyl to the hold point so it stays attached while carrying
        heldVinyl.OnPickup(holdPoint);
        player.StartCarrying();
    }
    private void TryPickupMask()
    {
        if (currentMask == null) return;

        heldMask = currentMask;
        // Parent the mask to the hold point so it stays attached while carrying
        heldMask.OnPickup(holdPoint);
        player.StartCarrying();
    }

    private void DropVinyl()
    {
        if (heldVinyl == null) return;

        bool matched = false;
        Transform matchedPos = null;

        if (currentVinylSlot != null)
        {
            matched = MatchVinylManager.Instance.TryMatch(heldVinyl, currentVinylSlot, out matchedPos);
        }

        heldVinyl.OnDrop(matched, matchedPos);
        heldVinyl = null;
        player.StopCarrying();
    }
        private void DropMask()
    {
        if (heldMask == null) return;

        bool matched = false;
        Transform matchedPos = null;

        if (currentMaskSlot != null)
        {
            matched = MatchVinylManager.Instance.TryMatch(heldMask, currentMaskSlot, out matchedPos);
        }

        heldMask.OnDrop(matched, matchedPos);
        heldMask = null;
        player.StopCarrying();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Vinyl") && other.TryGetComponent(out GrabVinyl vinyl))
        {
            currentVinyl = vinyl;
        }
        if (other.CompareTag("Mask") && other.TryGetComponent(out GrabVinyl mask))
        {
            currentMask = mask;
        }

        if (other.CompareTag("VinylSlot"))
        {
            currentVinylSlot = other.transform;
        }
        if (other.CompareTag("MaskSlot"))
        {
            currentMaskSlot = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Vinyl") && currentVinyl != null && other.transform == currentVinyl.transform)
        {
            currentVinyl = null;
        }

        if (other.CompareTag("Mask") && currentMask != null && other.transform == currentMask.transform)
        {
            currentMask = null;
        }

        if (other.CompareTag("VinylSlot") && other.transform == currentVinylSlot)
        {
            currentVinylSlot = null;
        }
        if (other.CompareTag("MaskSlot") && other.transform == currentMaskSlot)
        {
            currentMaskSlot = null;
        }
    }
}