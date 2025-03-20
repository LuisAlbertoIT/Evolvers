using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class CharacterData
{
    public string ID;
    public string characterName;
    public int maxHP;
    public int currentHP;
    public int attack;
    public int defense;
    public int level;
    public int currentEXP;
    public int expToNextLevel;
    // Attacks
}

[System.Serializable]
public class GameData
{
    public List<CharacterData> characters;
}

public static class SaveManager
{
    private static string filePath = Path.Combine(Application.persistentDataPath, "savegame.json");

    public static void SaveGame(List<CharacterInfo> creatures)
    {
        GameData data = new GameData();
        data.characters = new List<CharacterData>();

        foreach (CharacterInfo creature in creatures)
        {
            CharacterData cdata = new CharacterData();
            cdata.ID = creature.ID;
            cdata.characterName = creature.characterName;
            cdata.maxHP = creature.maxHP;
            cdata.currentHP = creature.currentHP;
            cdata.attack = creature.attack;
            cdata.defense = creature.defense;
            cdata.level = creature.level;
            cdata.currentEXP = creature.currentEXP;
            cdata.expToNextLevel = creature.expToNextLevel;
            // Attacks
            data.characters.Add(cdata);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Game Saved to: " + filePath);
    }

    public static List<CharacterData> LoadGame()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            return data.characters;
        }
        else
        {
            Debug.Log("No save file found.");
            return null;
        }
    }
}