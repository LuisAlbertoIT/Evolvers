using UnityEngine;


public abstract class TraitBase : MonoBehaviour
{
    protected Sprite newSprite; // Nuevo sprite a asignar
    protected string targetSpriteName = "Ojos"; // Nombre del objeto que queremos cambiar

    public void Appel()
    {
        // Busca todos los SpriteRenderer en los hijos
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            // Si el nombre del objeto coincide con el que queremos cambiar, le asignamos el nuevo sprite
            if (sr.gameObject.name == targetSpriteName)
            {
                sr.sprite = newSprite;
                Debug.Log("Sprite cambiado en: " + targetSpriteName);
                break; // Salimos del bucle porque ya encontramos el sprite
            }
        }
    }
    public abstract void ApplyEffect(); // Cada trait tendrá su propio efecto
}

