using UnityEngine;

public class RotatingJumpRope : MonoBehaviour
{
    [Header("Rope Properties")]
    public float ropeLength = 4f;
    public float ropeHeight = 4f;
    public int segments = 30;
    
    [Header("Rotation")]
    public float rotationSpeed = 180f;
    
    [Header("3D Perspective Effect")]
    public float perspectiveScale = 0.6f;
    public float ropeThickness = 0.15f;
    
    [Header("Curve Settings")]
    public float sagAmount = 0.3f;
    
    [Header("Position Adjustment")]
    public float verticalOffset = 0f;
    
    [Header("Scoring Zone")]
    public float scoringAngleRange = 50f;
    
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private float currentAngle = 0f;
    
    // NEW: Control when rope starts
    private bool isRotating = false; // Start frozen!
    private float targetRotationSpeed;
    
void Start()
{
    lineRenderer = gameObject.AddComponent<LineRenderer>();
    lineRenderer.positionCount = segments;
    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
    lineRenderer.startColor = Color.red;
    lineRenderer.endColor = Color.red;
    
    edgeCollider = gameObject.AddComponent<EdgeCollider2D>();
    edgeCollider.isTrigger = true;
    
    gameObject.tag = "Rope";
    
    // Store the target speed, but start at 0
    targetRotationSpeed = rotationSpeed;
    rotationSpeed = 0; // Frozen at start!
    
    // IMPORTANT: Start at angle 0 (bottom position, green)
    currentAngle = 0f;
    
    // Apply to PARENT (RopeSystem or whatever holds the rope)
    if (transform.parent != null)
    {
        transform.parent.rotation = Quaternion.Euler(0, 0, 0);
    }
    else
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    
    Debug.Log("Rope initialized at angle 0 (bottom, green)");
    
    DrawRopeWithPerspective();
}
    
void Update()
{
    if (isRotating)
    {
        currentAngle += rotationSpeed * Time.deltaTime;
        if (currentAngle >= 360f)
            currentAngle -= 360f;
        
        // Apply to parent if exists
        if (transform.parent != null)
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
    }
    
    DrawRopeWithPerspective();
}
    
    // NEW METHOD: Start the rope spinning
    public void StartRotating()
    {
        isRotating = true;
        rotationSpeed = targetRotationSpeed;
        Debug.Log("Rope started spinning at speed: " + rotationSpeed);
    }
    
    // NEW METHOD: Stop the rope
    public void StopRotating()
    {
        isRotating = false;
        rotationSpeed = 0;
        Debug.Log("Rope stopped");
    }
    
    // NEW METHOD: Set the rope to a specific angle
public void SetAngle(float angle)
{
    currentAngle = angle;
    
    // Apply to parent if exists
    if (transform.parent != null)
    {
        transform.parent.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
    else
    {
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
    
    Debug.Log("Rope angle set to: " + angle);
}
    
    void DrawRopeWithPerspective()
    {
        Vector2[] colliderPoints = new Vector2[segments];
        
        float angleRad = currentAngle * Mathf.Deg2Rad;
        float depth = Mathf.Cos(angleRad);
        float verticalPosition = Mathf.Sin(angleRad);
        
        float depthScale = Mathf.Lerp(perspectiveScale, 1f, (depth + 1f) / 2f);
        float currentLength = ropeLength * depthScale;
        float yOffset = verticalPosition * (ropeHeight / 2f) + verticalOffset;
        
        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            float x = (t - 0.5f) * currentLength;
            
            float normalizedX = (t - 0.5f) * 2f;
            float sag = -sagAmount * depthScale * (1f - normalizedX * normalizedX);
            float y = yOffset + sag;
            
            Vector3 point = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, point);
            colliderPoints[i] = new Vector2(x, y);
        }
        
        edgeCollider.points = colliderPoints;
        
        float width = ropeThickness * depthScale;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        
        // Rope is always red (no color change)
        Color ropeColor = Color.red;
        ropeColor.a = Mathf.Lerp(0.4f, 1f, depthScale);
        lineRenderer.startColor = ropeColor;
        lineRenderer.endColor = ropeColor;
    }
    
    public bool IsAtBottom()
    {
        float normalizedAngle = currentAngle % 360f;
        
        if (normalizedAngle <= scoringAngleRange || normalizedAngle >= (360f - scoringAngleRange))
        {
            return true;
        }
        
        return false;
    }
    
    public void IncreaseSpeed(float amount)
    {
        targetRotationSpeed += amount;
        if (isRotating)
        {
            rotationSpeed = targetRotationSpeed;
        }
    }
}