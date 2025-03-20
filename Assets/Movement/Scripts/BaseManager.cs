using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseManager : MonoBehaviour
{
    private static BaseManager _instance;
    public static BaseManager Instance { get { return _instance; } }

    public List<CharacterInfo> allCreatures = new List<CharacterInfo>();
    public Transform creatureListContent;
    public GameObject creatureListItemPrefab;
    public CreatureEditor creatureEditor;

    public List<CharacterInfo> selectedCreaturesForWorld = new List<CharacterInfo>();

    public CharacterInfo baseCharacter;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadGameState();
        PopulateCreatureList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulateCreatureList()
    {
        foreach (Transform child in creatureListContent)
        {
            Destroy(child.gameObject);
        }

        foreach (CharacterInfo creature in allCreatures)
        {
            GameObject item = Instantiate(creatureListItemPrefab, creatureListContent);
            CreatureListItem listItem = item.GetComponent<CreatureListItem>();
            if (listItem != null)
            {
                listItem.Setup(creature, OnCreatureSelected);
            }
        }
    }

    void OnCreatureSelected(CharacterInfo creature)
    {
        if (creatureEditor != null)
        {
            creatureEditor.EditCreature(creature);
        }
    }

    public void ConfirmSelectionForWorld()
    {
        selectedCreaturesForWorld = new List<CharacterInfo>(allCreatures);
        SaveGameState();
    }

    public void SaveGameState()
    {
        SaveManager.SaveGame(allCreatures);
    }

    public void LoadGameState()
    {
        List<CharacterData> loadedData = SaveManager.LoadGame();
        if (loadedData == null)
        {
            CharacterInfo creature1 = baseCharacter;
            creature1.characterName = "Nameless";
            creature1.currentHP = creature1.maxHP;
            creature1.ID = Guid.NewGuid().ToString();
            creature1.attacks.Add(gameObject.AddComponent<Attacks>().StandardAttack());
            creature1.activeAtk = creature1.attacks[0].attackName;
            creature1.attacks.Add(gameObject.AddComponent<Attacks>().AcidSpit());
            CharacterInfo creature2 = baseCharacter;
            creature2.characterName = "Nameless";
            creature2.currentHP = creature2.maxHP;
            creature2.ID = Guid.NewGuid().ToString();
            creature2.attacks.Add(gameObject.AddComponent<Attacks>().StandardAttack());
            creature2.activeAtk = creature1.attacks[0].attackName;
            creature2.attacks.Add(gameObject.AddComponent<Attacks>().AcidSpit());
            allCreatures.Add(creature1);
            allCreatures.Add(creature2);
        }
        else
        {
            foreach (CharacterData data in loadedData)
            {
                CharacterInfo creature = allCreatures.Find(x => x.ID == data.ID);
                if (creature != null)
                {
                    creature.characterName = data.characterName;
                    creature.maxHP = data.maxHP;
                    creature.currentHP = data.currentHP;
                    creature.attack = data.attack;
                    creature.defense = data.defense;
                    creature.level = data.level;
                    creature.currentEXP = data.currentEXP;
                    creature.expToNextLevel = data.expToNextLevel;
                    // Attacks
                }
            }
        }
    }
}
