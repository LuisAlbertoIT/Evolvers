using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public Sprite icon;
    public int quantity = 1;

    public void Use(CharacterInfo character)
    {
        // l�gica para usar el item
        Debug.Log($"{character.name} us� {itemName}");
    }
}