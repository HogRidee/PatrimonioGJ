using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpBar : MonoBehaviour
{
    [SerializeField] private Image _barImage;
    private Coroutine _barCoroutine;

    private void Awake()
    {
        gameObject.SetActive(false);
        _barImage.fillAmount = 0f;
    }

    public void StartBar(float duration)
    {
        if (_barCoroutine != null)
            StopCoroutine(_barCoroutine);

        gameObject.SetActive(true);
        _barCoroutine = StartCoroutine(RunBar(duration));
    }

    private IEnumerator RunBar(float duration)
    {
        //Debug.Log("Inicio RunBar");
        float elapsed = 0f;
        _barImage.fillAmount = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            _barImage.fillAmount = 1f - (elapsed / duration);
            yield return null;
        }

        _barImage.fillAmount = 0f;
        gameObject.SetActive(false);
        _barCoroutine = null;
    }
}
