using UnityEngine;
using UnityEngine.UI;

public class UIHoverAnimation : MonoBehaviour
{
    public float scaleAmount = 1.1f;  // Max scale
    public float animationSpeed = 1f; // Speed of animation

    private Vector3 originalScale;
    private bool scalingUp = true;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        float scaleFactor = (scalingUp ? 1 : -1) * animationSpeed * Time.unscaledDeltaTime;
        transform.localScale += new Vector3(scaleFactor, scaleFactor, scaleFactor);

        if (transform.localScale.x >= originalScale.x * scaleAmount)
        {
            transform.localScale = originalScale * scaleAmount;
            scalingUp = false;
        }
        else if (transform.localScale.x <= originalScale.x)
        {
            transform.localScale = originalScale;
            scalingUp = true;
        }
    }
}
