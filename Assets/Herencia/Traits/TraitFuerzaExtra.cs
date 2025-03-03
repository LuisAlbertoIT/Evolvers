using UnityEngine;

public class TraitFuerzaExtra : TraitBase
{
    public override void ApplyEffect()
    {
        Creature creature = GetComponent<Creature>();
        if (creature != null)
        {
            creature.Fuerza += 5;
            Debug.Log("Fuerza aumentada en +5");
        }
    }
}
