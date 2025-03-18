using UnityEngine;

public class Attacks : MonoBehaviour
{
    public string attackName;
    public int damage;
    public int range;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Attacks StandardAttack()
    {
        attackName = "Bite";
        damage = 5;
        range = 1;

        return this;
    }

    public Attacks AcidSpit()
    {
        attackName = "Acid" + "\n" + "Spit";
        damage = 3;
        range = 2;

        return this;
    }

    public void Execute(CharacterInfo attacker, CharacterInfo target, int dammage)
    {
        int finalDamage = Mathf.Max(0, dammage * attacker.attack - (target.defense + target.defense*2));
        Debug.Log($"{attacker.characterName} uses {attacker.activeAtk} on {target.characterName} for {finalDamage} damage!");
        target.TakeDamage(finalDamage, attacker);
    }
}
