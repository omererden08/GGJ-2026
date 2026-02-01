using UnityEngine;
using System.Collections;
public class Dolap : MonoBehaviour
{
    [SerializeField] private GameObject acikDolap;
    [SerializeField] private GameObject maskeAnim;
    [SerializeField] private GameObject maske;
    [SerializeField] private float AnimTime = 0.8f;
    
    private bool playerInRange = false;
    private bool isOpen = false;
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            OpenDolap();
            isOpen = true;
        }
    }
    
    void OpenDolap()
    {
        acikDolap.SetActive(true);
        StartCoroutine(MaskAnim());

    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Holder"))
        {
            playerInRange = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Holder"))
        {
            playerInRange = false;
        }
    }
    private IEnumerator MaskAnim()
    {
        maskeAnim.SetActive(true);
        yield return new WaitForSecondsRealtime(AnimTime);
        maske.SetActive(true);
        maskeAnim.SetActive(false);
    }
}