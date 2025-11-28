using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 10f;
    
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private bool canScoreFromRope = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }
    
    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        canScoreFromRope = true;
    }
    public void TriggerJump()
    {
        if (isGrounded)
        {
            Jump();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            RotatingJumpRope rope = collision.GetComponent<RotatingJumpRope>();
            
            if (rope != null)
            {
                bool ropeIsAtBottom = rope.IsAtBottom();
                
                if (isGrounded && ropeIsAtBottom)
                {
                    // HIT BY ROPE!
                    GameManager.Instance.ResetScore();
                    
                    // NEW: Show message on screen!
                    MessageManager.Instance.ShowMessage("HIT BY ROPE!", Color.red);
                }
                else if (!isGrounded && ropeIsAtBottom && canScoreFromRope)
                {
                    // SUCCESSFUL JUMP!
                    GameManager.Instance.AddScore(1);
                    canScoreFromRope = false;
                    
                    // Optional: Show success message too!
                    // MessageManager.Instance.ShowMessage("PERFECT!", Color.green);
                }
                else if (!isGrounded && !ropeIsAtBottom)
                {
                    Debug.Log("Rope touched player but not at scoring position");
                }
            }
        }
    }
}