using UnityEngine;
using TMPro;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;
    
    [Header("UI Reference")]
    public TextMeshProUGUI messageText;
    
    [Header("Settings")]
    public float displayDuration = 0.5f;
    public float fadeSpeed = 2f;
    
    private Coroutine currentMessageCoroutine;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (messageText != null)
        {
            Color color = messageText.color;
            color.a = 0;
            messageText.color = color;
        }
    }
    
    // Show temporary message (fades out)
    public void ShowMessage(string text, Color color)
    {
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }
        
        currentMessageCoroutine = StartCoroutine(DisplayMessage(text, color));
    }
    
    // NEW: Show permanent message (doesn't fade)
    public void ShowPermanentMessage(string text, Color color)
    {
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }
        
        currentMessageCoroutine = StartCoroutine(DisplayPermanentMessage(text, color));
    }
    
    IEnumerator DisplayMessage(string text, Color color)
    {
        if (messageText == null) yield break;
        
        messageText.text = text;
        messageText.color = color;
        
        // Fade IN
        float timer = 0;
        while (timer < 1f / fadeSpeed)
        {
            timer += Time.unscaledDeltaTime; // Use unscaledDeltaTime so it works when game is paused!
            Color newColor = messageText.color;
            newColor.a = Mathf.Lerp(0, 1, timer * fadeSpeed);
            messageText.color = newColor;
            yield return null;
        }
        
        Color fullColor = messageText.color;
        fullColor.a = 1;
        messageText.color = fullColor;
        
        yield return new WaitForSecondsRealtime(displayDuration); // Use Realtime!
        
        // Fade OUT
        timer = 0;
        while (timer < 1f / fadeSpeed)
        {
            timer += Time.unscaledDeltaTime;
            Color newColor = messageText.color;
            newColor.a = Mathf.Lerp(1, 0, timer * fadeSpeed);
            messageText.color = newColor;
            yield return null;
        }
        
        Color invisibleColor = messageText.color;
        invisibleColor.a = 0;
        messageText.color = invisibleColor;
    }
    
    // NEW COROUTINE: Permanent message
    IEnumerator DisplayPermanentMessage(string text, Color color)
    {
        if (messageText == null) yield break;
        
        messageText.text = text;
        messageText.color = color;
        
        // Fade IN
        float timer = 0;
        while (timer < 1f / fadeSpeed)
        {
            timer += Time.unscaledDeltaTime;
            Color newColor = messageText.color;
            newColor.a = Mathf.Lerp(0, 1, timer * fadeSpeed);
            messageText.color = newColor;
            yield return null;
        }
        
        // Stay visible permanently
        Color fullColor = messageText.color;
        fullColor.a = 1;
        messageText.color = fullColor;
        
        // Don't fade out!
    }
}