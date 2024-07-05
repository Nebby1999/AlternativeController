using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualController : MonoBehaviour
{
    [SerializeField] private float shrinkScale = 0.97f;
    [SerializeField, Range(0.125f, 0.25f)] private float duration;
    private Transform _transform;
    private Vector3 originalScale;
    private void Awake()
    {
        _transform = transform;
        originalScale = _transform.localScale;
    }
    public void VisualFeedback(bool value)
    {
        if (value) StartCoroutine(Shrink());
    }
    private IEnumerator Shrink()
    {
        float elapsedTime = 0f;

        // Shrink
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, originalScale * shrinkScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale * shrinkScale; // Asegura que la escala final sea exactamente la deseada.

        // Expand (volver a tamaño original)
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale * shrinkScale, originalScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale; // Asegura que la escala final sea exactamente la deseada.
    }
}
