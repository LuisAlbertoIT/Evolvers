using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InventarioGlobal", menuName = "Scriptable Objects/InventarioGlobal")]
public class InventarioGlobal : ScriptableObject
{
    public List<Sprite> itemIcons = new List<Sprite>();

    // Evento para actualizar las interfaces automáticamente
    public UnityEvent OnInventarioChanged;

    public void AgregarItem(Sprite icon)
    {
        itemIcons.Add(icon);
        OnInventarioChanged?.Invoke();
    }

    public void QuitarItem(Sprite icon)
    {
        itemIcons.Remove(icon);
        OnInventarioChanged?.Invoke();
    }

    public void LimpiarInventario()
    {
        itemIcons.Clear();
        OnInventarioChanged?.Invoke();
    }
}
