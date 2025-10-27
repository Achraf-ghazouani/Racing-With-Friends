using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float hoverScale = 1.2f; // Scale when hovered
    [SerializeField] private float animationSpeed = 0.2f; // Speed of scaling effect

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale; // Save original scale
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer Entered: " + gameObject.name);
        StopAllCoroutines();
        StartCoroutine(ScaleButton(hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer Exited: " + gameObject.name);
        StopAllCoroutines();
        StartCoroutine(ScaleButton(originalScale.x));
    }

    private System.Collections.IEnumerator ScaleButton(float targetScale)
    {
        Vector3 target = new Vector3(targetScale, targetScale, targetScale);
        float time = 0;

        while (time < animationSpeed)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, target, time / animationSpeed);
            time += Time.unscaledDeltaTime; // Use unscaledDeltaTime to ignore timeScale
            yield return null;
        }

        transform.localScale = target; // Ensure the final scale is set
    }
}