using UnityEngine;

public class MatchSlotManager : MonoBehaviour
{
    public static MatchSlotManager Instance;

    [Header("Parent References (Auto-filled by name)")]
    private Transform Objects;     // Grabbable objelerin parent objesi
    private Transform Positions;   // Drop pozisyonlarının parent objesi

    public GrabbableObject[] grabbableObjects;
    public Transform[] targetPositions;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Otomatik olarak sahnede isimle objeleri bul
        Objects = GameObject.Find("Objects")?.transform;
        Positions = GameObject.Find("Positions")?.transform;

        if (Objects == null || Positions == null)
        {
            Debug.LogError("❌ 'Objects' veya 'Positions' adlı GameObject sahnede bulunamadı!");
            return;
        }

        FillArraysFromParents();
    }

    private void FillArraysFromParents()
    {
        // Objeleri doldur
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

        // Pozisyonları doldur
        int posCount = Positions.childCount;
        targetPositions = new Transform[posCount];

        for (int i = 0; i < posCount; i++)
        {
            targetPositions[i] = Positions.GetChild(i);
        }

        if (objCount != posCount)
        {
            Debug.LogWarning($"⚠️ Objeler ({objCount}) ve Pozisyonlar ({posCount}) sayısı eşleşmiyor!");
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
                return true;
            }
        }

        return false;
    }
}
