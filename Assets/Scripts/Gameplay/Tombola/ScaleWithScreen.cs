using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ScaleWithScreen : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer.sprite == null)
        {
            Debug.LogError("El objeto no tiene un sprite asignado.");
            return;
        }

        // Obtener el tamaño de la pantalla en unidades del mundo
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // Obtener el tamaño del sprite
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Ajustar la escala del objeto
        transform.localScale = new Vector3(
            worldScreenWidth / spriteSize.x,
            worldScreenHeight / spriteSize.y,
            1f
        );
    }
}
