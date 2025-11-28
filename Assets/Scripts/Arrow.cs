using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 3f;
    
    [Header("Arrow Properties")]
    public KeyCode requiredKey;
    public string arrowDirection;
    
    [Header("Timing")]
    private bool isInHitZone = false;
    private bool hasBeenScored = false;
    
void Update()
{
    // Move arrow from left to right
    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    
    // Destroy if goes off-screen (right side)
    if (transform.position.x > 10f)
    {
        if (!hasBeenScored)
        {
            Debug.Log("Missed arrow: " + arrowDirection);
        }
        
        // NEW: Notify GameManager that an arrow finished
        GameManager.Instance.OnArrowFinished();
        
        Destroy(gameObject);
    }
}
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Arrow entered hit zone
        if (collision.CompareTag("HitZone"))
        {
            isInHitZone = true;
            Debug.Log(arrowDirection + " arrow entered hit zone! IsInHitZone = " + isInHitZone);
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Arrow left hit zone
        if (collision.CompareTag("HitZone"))
        {
            isInHitZone = false;
            
            if (!hasBeenScored)
            {
                Debug.Log("Exited without hit - " + arrowDirection);
            }
        }
    }
    
    // IMPORTANT: Make sure this method is PUBLIC!
    public bool IsInHitZone()
    {
        Debug.Log("IsInHitZone called for " + arrowDirection + " - returning: " + isInHitZone);
        return isInHitZone && !hasBeenScored;
    }
    
public bool TryHit(KeyCode pressedKey)
{
    Debug.Log("TryHit called for " + arrowDirection + " arrow. Required: " + requiredKey + ", Pressed: " + pressedKey);
    
    if (hasBeenScored)
    {
        Debug.Log("Already scored!");
        return false;
    }
    
    if (!isInHitZone)
    {
        Debug.Log("Not in hit zone!");
        return false;
    }
    
    if (pressedKey == requiredKey)
    {
        hasBeenScored = true;
        Debug.Log("CORRECT KEY! Destroying arrow.");
        
        // NEW: Notify GameManager that an arrow finished
        GameManager.Instance.OnArrowFinished();
        
        Destroy(gameObject);
        return true;
    }
    else
    {
        Debug.Log("WRONG KEY!");
        return false;
    }
}
}