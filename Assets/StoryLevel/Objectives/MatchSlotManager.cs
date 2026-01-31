using UnityEngine;

public class MatchSlotManager : MonoBehaviour
{
    public static MatchSlotManager Instance;

    [Header("Parent References")]
    [SerializeField] private Transform Objects;
    [SerializeField] private Transform Positions;

    [HideInInspector] public GrabbableObject[] grabbableObjects;
    [HideInInspector] public Transform[] targetPositions;

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

    private void Start()
    {
        FillArraysFromParents();
    }

    private void FillArraysFromParents()
    {
        int objCount = Objects.childCount;
        grabbableObjects = new GrabbableObject[objCount];

        for (int i = 0; i < objCount; i++)
        {
            var child = Objects.GetChild(i);
            var grabbable = child.GetComponent<GrabbableObject>();

            if (grabbable == null)
                Debug.LogWarning($"❗ '{child.name}' üzerinde GrabbableObject bileşeni eksik!");

            grabbableObjects[i] = grabbable;
        }

        int posCount = Positions.childCount;
        targetPositions = new Transform[posCount];

        for (int i = 0; i < posCount; i++)
        {
            targetPositions[i] = Positions.GetChild(i);
        }

        if (objCount != posCount)
        {
            Debug.LogWarning($"⚠️ Objeler ({objCount}) ve Pozisyonlar ({posCount}) eşleşmiyor!");
        }
    }

    public bool TryMatch(GrabbableObject obj, Transform candidatePos, out Transform correctPos)
    {
        correctPos = null;

        for (int i = 0; i < grabbableObjects.Length; i++)
        {
            if (grabbableObjects[i] == obj && targetPositions[i] == candidatePos)
            {
                correctPos = targetPositions[i];

                CheckForAllMatches(); // ✅ Match başarılıysa tüm eşleşmeleri kontrol et
                return true;
            }
        }

        return false;
    }

    private void CheckForAllMatches()
    {
        for (int i = 0; i < grabbableObjects.Length; i++)
        {
            if (grabbableObjects[i].transform.position != targetPositions[i].position)
                return; // ❌ Hâlâ eşleşmemiş bir obje var
        }

        // ✅ Hepsi doğru konumda → Sahne geçişi yapılabilir
        Debug.Log("✅ Tüm objeler doğru yerleştirildi! Sahne geçiyor...");
        SceneLoader.Instance.LoadScene(sceneName, GameState.Playing); // ❗ SAHNE ADINI DEĞİŞTİR
    }
}
