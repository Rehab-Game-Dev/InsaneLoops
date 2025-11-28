using UnityEngine;

public class ArrowInputManager : MonoBehaviour
{
    [Header("Player Reference")]
    public PlayerController player; // Link to player
    
    void Start()
    {
        // Find player if not assigned
        if (player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        
        if (player == null)
        {
            Debug.LogError("Player not found! Arrow jumps won't work!");
        }
    }
    
    void Update()
    {
        // Check for arrow key presses
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("=== UP ARROW PRESSED ===");
            TriggerJump(); // Make player jump!
            CheckArrowHit(KeyCode.UpArrow);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("=== DOWN ARROW PRESSED ===");
            TriggerJump(); // Make player jump!
            CheckArrowHit(KeyCode.DownArrow);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("=== LEFT ARROW PRESSED ===");
            TriggerJump(); // Make player jump!
            CheckArrowHit(KeyCode.LeftArrow);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("=== RIGHT ARROW PRESSED ===");
            TriggerJump(); // Make player jump!
            CheckArrowHit(KeyCode.RightArrow);
        }
    }
    
    // NEW METHOD: Trigger player jump
    void TriggerJump()
    {
        if (player != null)
        {
            player.TriggerJump();
        }
    }
    
    void CheckArrowHit(KeyCode pressedKey)
    {
        Debug.Log("Checking for arrows...");
        
        Arrow[] arrows = FindObjectsOfType<Arrow>();
        
        Debug.Log("Found " + arrows.Length + " arrows");
        
        if (arrows.Length == 0)
        {
            Debug.Log("No arrows in scene");
            return;
        }
        
        Arrow targetArrow = null;
        float closestDistance = float.MaxValue;
        
        foreach (Arrow arrow in arrows)
        {
            if (arrow.IsInHitZone())
            {
                float distance = Mathf.Abs(arrow.transform.position.x - 7f);
                
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetArrow = arrow;
                }
            }
        }
        
        if (targetArrow != null)
        {
            bool success = targetArrow.TryHit(pressedKey);
            
            if (success)
            {
                Debug.Log("SUCCESS!");
                GameManager.Instance.AddScore(10);
                MessageManager.Instance.ShowMessage("PERFECT!", Color.green);
            }
            else
            {
                Debug.Log("WRONG KEY!");
                GameManager.Instance.ResetScore();
                MessageManager.Instance.ShowMessage("WRONG ARROW!", Color.red);
            }
        }
        else
        {
            Debug.Log("NO ARROW IN HIT ZONE!");
            GameManager.Instance.ResetScore();
            MessageManager.Instance.ShowMessage("MISSED!", Color.red);
        }
    }
}