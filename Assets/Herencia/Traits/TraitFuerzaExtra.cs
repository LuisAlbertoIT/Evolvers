using UnityEngine;

public class TraitFuerzaExtra : TraitBase
{
    public override void ApplyEffect()
    {
        Criatura creature = GetComponent<Criatura>();
        if (creature != null)
        {
            creature.Fuerza += 5;
            Debug.Log("Fuerza aumentada en +5");
        }
    }

    void Start()
    {
        ApplyEffect();
    }

}
