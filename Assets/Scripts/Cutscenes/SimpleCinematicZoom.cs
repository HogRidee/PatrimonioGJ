using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SimpleCinematicZoom : MonoBehaviour
{
    public RectTransform target;

    [Header("Zoom")]
    public Vector3 startScale = Vector3.one;
    public Vector3 endScale = Vector3.one * 1.05f;
    public float duration = 3f;

    public string nextSceneName = "NombreDeTuEscena";

    void Start()
    {
        if (target != null)
            target.localScale = startScale;
        StartCoroutine(ZoomAndLoad());
    }

    private IEnumerator ZoomAndLoad()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(t / duration);
            target.localScale = Vector3.Lerp(startScale, endScale, alpha);
            yield return null;
        }
        SceneManager.LoadScene(2);
    }
}
