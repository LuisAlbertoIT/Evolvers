using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatureEditor : MonoBehaviour
{
    public TMP_InputField nameInput;
    public TMP_Text statsText;
    public Button saveButton;

    private CharacterInfo currentCreature;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (nameInput != null)
            nameInput.onValueChanged.AddListener(delegate { OnNameChanged(); });
        if (saveButton != null)
            saveButton.onClick.AddListener(OnSaveCreature);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EditCreature(CharacterInfo creature)
    {
        currentCreature = creature;
        nameInput.text = creature.characterName;
        UpdateStatsText();
    }

    void OnNameChanged()
    {
        if (currentCreature != null)
        {
            currentCreature.characterName = nameInput.text;
            UpdateStatsText();
        }
    }

    void UpdateStatsText()
    {
        if (currentCreature != null)
        {
            statsText.text = $"HP: {currentCreature.currentHP}/{currentCreature.maxHP}\n" +
                             $"Attack: {currentCreature.attack}\n" +
                             $"Defense: {currentCreature.defense}\n" +
                             $"Level: {currentCreature.level}";
            //Attacks
        }
    }

    public void OnSaveCreature()
    {
        Debug.Log("Creature updated: " + currentCreature.characterName);
        BaseManager.Instance.SaveGameState();
    }
}
