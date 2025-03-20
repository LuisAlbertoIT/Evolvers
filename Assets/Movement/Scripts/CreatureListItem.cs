using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureListItem : MonoBehaviour
{
    public TMP_Text creatureNameText;
    public Button selectButton;

    private CharacterInfo creature;
    private System.Action<CharacterInfo> onSelectCallback;

    public void Setup(CharacterInfo creature, System.Action<CharacterInfo> onSelect)
    {
        this.creature = creature;
        creatureNameText.text = creature.characterName;
        onSelectCallback = onSelect;
        selectButton.onClick.AddListener(() => { 
            GameObject.Find("CreatureList").SetActive(false);
            GameObject.Find("Back").SetActive(false);
            FindInActiveObjectByName("CreatureEditor").SetActive(true); 
        });
        selectButton.onClick.AddListener(() => { onSelectCallback?.Invoke(creature); });
    }

    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
