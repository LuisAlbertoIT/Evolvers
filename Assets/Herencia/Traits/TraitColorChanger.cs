using UnityEngine;

public class TraitColorChanger : TraitBase
{
    public Color newColor = Color.red; // Nuevo color para la criatura

    public override void ApplyEffect()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = newColor;
        }
    }
}

