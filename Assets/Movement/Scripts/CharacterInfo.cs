using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;

    [Header("Basic Stats")]
    public string characterName;
    public int maxHP = 100;
    public int currentHP;
    public int attack = 10;
    public int defense = 5;
    public int range = 3;

    [Header("Experience and Leveling")]
    public int level = 1;
    public int currentEXP = 0;
    public int expToNextLevel = 100;

    [Header("Attacks and Traits")]
    public List<Attacks> attacks = new List<Attacks>();
    public string activeAtk;
    //public List<Traits> traits = new List<Traits>();*/

    [Header("Turn Management")]
    public bool canMove = true;
    public bool canAct = true;



    void Start()
    {
        currentHP = maxHP;
        attacks.Add(gameObject.AddComponent<Attacks>().StandardAttack());
        activeAtk = attacks[0].attackName;
        attacks.Add(gameObject.AddComponent<Attacks>().AcidSpit());
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log($"{characterName} takes {damage} damage! HP: {currentHP}/{maxHP}");
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        Debug.Log($"{characterName} heals for {amount}! HP: {currentHP}/{maxHP}");
    }

    private void Die()
    {
        Debug.Log($"{characterName} has been defeated!");
        gameObject.SetActive(false);
        Destroy(gameObject);
        if (gameObject.CompareTag("Enemy"))
            TurnManager.Instance.enemies.Remove(this);
        else if (gameObject.CompareTag("Player"))
            TurnManager.Instance.characters.Remove(this);
    }

    public void GainEXP(int exp)
    {
        currentEXP += exp;
        Debug.Log($"{characterName} gains {exp} EXP!");
        while (currentEXP >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        currentEXP -= expToNextLevel;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.5f);

        maxHP += 10;
        currentHP = maxHP;
        attack += 2;
        defense += 1;

        Debug.Log($"{characterName} leveled up to Level {level}! HP: {maxHP}, Attack: {attack}, Defense: {defense}");
    }

    public void PerformAttack(int attackIndex, CharacterInfo target)
    {
        if (attackIndex >= 0 && attackIndex < attacks.Count && canAct)
        {
            int dammage = 0;
            Attacks attack = attacks[attackIndex];
            foreach(Attacks atk in attacks)
            {
                if(atk.attackName == activeAtk)
                    dammage = atk.damage;
            }
            attack.Execute(this, target, dammage);
            canAct = false;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        activeTile = collision.GetComponent<OverlayTile>();
        activeTile.isBlocked = true;
        //collision.transform.position = transform.position;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        activeTile = collision.GetComponent<OverlayTile>();
        activeTile.isBlocked = false;
    }
}
