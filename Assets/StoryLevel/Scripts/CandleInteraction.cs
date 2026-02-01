using UnityEngine;

public class CandleInteraction : MonoBehaviour
{
    [SerializeField] private GameObject unlitCandle;
    [SerializeField] private GameObject litCandle;
    
    private bool playerInRange = false;
    private bool isLit = false;
    
    void Start()
    {
        litCandle.SetActive(false);
    }
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && GameManager.Instance.HasMatch && !isLit)
        {
            LightCandle();
            isLit = true;
        }
    }
    
    void LightCandle()
    {
        unlitCandle.SetActive(false);
        litCandle.SetActive(true);
        GameManager.Instance.storyScore += 1;
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
}