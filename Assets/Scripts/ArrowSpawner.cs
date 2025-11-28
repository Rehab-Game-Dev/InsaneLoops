using UnityEngine;
using System.Collections;

public class ArrowSpawner : MonoBehaviour
{
    [Header("Arrow Prefabs")]
    public GameObject arrowUpPrefab;
    public GameObject arrowRightPrefab;
    public GameObject arrowDownPrefab;
    public GameObject arrowLeftPrefab;
    
    [Header("Spawn Settings")]
    public float spawnHeight = 0f;
    
    [Header("Timing Sync")]
    public RotatingJumpRope rope;
    
    [Header("Fine Tuning")]
    public float ropeStartOffset = 0f; // Adjust this to sync rope perfectly!
    // Positive = rope starts earlier
    // Negative = rope starts later
    
    [Header("Arrow Sequence")]
    public int[] arrowSequence = { 0, 1, 2, 3, 0 };
    
    void Start()
    {
        if (rope == null)
        {
            Debug.LogError("Rope not assigned!");
            return;
        }
        
        StartCoroutine(SpawnSyncedArrows());
    }
    
    IEnumerator SpawnSyncedArrows()
    {
        float ropeBeatInterval = 1f; // Rope beats every 1 second
        float arrowTravelTime = 5f; // Arrows take 5 seconds to reach hit zone
        
        Debug.Log("=== STARTING ARROW SEQUENCE ===");
        Debug.Log("Rope beats every: " + ropeBeatInterval + " seconds");
        Debug.Log("Arrow travel time: " + arrowTravelTime + " seconds");
        Debug.Log("Rope start offset: " + ropeStartOffset + " seconds");
        
        // Start rope at bottom (green) but frozen
        rope.SetAngle(0f);
        
        // Spawn all arrows, spaced 1 second apart
        for (int i = 0; i < arrowSequence.Length; i++)
        {
            SpawnArrow(arrowSequence[i], i);
            Debug.Log("Spawned arrow " + (i + 1) + " at time: " + Time.time);
            
            if (i < arrowSequence.Length - 1)
            {
                yield return new WaitForSeconds(ropeBeatInterval);
            }
        }
        
        Debug.Log("All arrows spawned.");
        
        // Calculate when to start rope
        float timeAlreadyWaited = (arrowSequence.Length - 1) * ropeBeatInterval; // 4 seconds
        float timeUntilFirstArrowArrives = arrowTravelTime - timeAlreadyWaited; // 5 - 4 = 1 second
        
        // Apply fine-tuning offset
        float ropeStartDelay = timeUntilFirstArrowArrives + ropeStartOffset;
        
        Debug.Log("Waiting " + ropeStartDelay + " seconds before starting rope...");
        yield return new WaitForSeconds(ropeStartDelay);
        
        Debug.Log("STARTING ROPE NOW at time: " + Time.time);
        rope.StartRotating();
    }
    
    void SpawnArrow(int arrowType, int index)
    {
        GameObject prefabToSpawn = null;
        string arrowName = "";
        
        switch (arrowType)
        {
            case 0:
                prefabToSpawn = arrowUpPrefab;
                arrowName = "Up";
                break;
            case 1:
                prefabToSpawn = arrowRightPrefab;
                arrowName = "Right";
                break;
            case 2:
                prefabToSpawn = arrowDownPrefab;
                arrowName = "Down";
                break;
            case 3:
                prefabToSpawn = arrowLeftPrefab;
                arrowName = "Left";
                break;
        }
        
        if (prefabToSpawn == null)
        {
            Debug.LogError("Arrow prefab not assigned!");
            return;
        }
        
        Vector3 spawnPosition = new Vector3(transform.position.x, spawnHeight, 0);
        GameObject newArrow = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
        
        Debug.Log("→ " + arrowName + " arrow spawned");
    }
}