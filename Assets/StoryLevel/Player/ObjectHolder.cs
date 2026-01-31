using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private Transform holdPoint;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private GrabbableObject currentObject = null;
    private Transform currentDropSlot = null;
    private GrabbableObject heldObject = null;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (heldObject == null)
                TryPickup();
            else
                DropObject();
        }
    }

    private void TryPickup()
    {
        if (currentObject == null) return;

        heldObject = currentObject;
        heldObject.OnPickup(holdPoint);
    }

    private void DropObject()
    {
        if (heldObject == null) return;

        bool matched = false;
        Transform matchedPos = null;

        if (currentDropSlot != null)
        {
            matched = MatchSlotManager.Instance.TryMatch(heldObject, currentDropSlot, out matchedPos);
        }

        heldObject.OnDrop(matched, matchedPos);
        heldObject = null;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Grabbable") && other.TryGetComponent(out GrabbableObject obj))
        {
            currentObject = obj;
        }

        if (other.CompareTag("DropSlot"))
        {
            currentDropSlot = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Grabbable") && currentObject != null && other.transform == currentObject.transform)
        {
            currentObject = null;
        }

        if (other.CompareTag("DropSlot") && other.transform == currentDropSlot)
        {
            currentDropSlot = null;
        }
    }
}
