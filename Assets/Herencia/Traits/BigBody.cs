using UnityEngine;

public class BigBody : TraitBase
{
    public override void ApplyEffect()
    {
        Creature creature = GetComponent<Creature>();
        if (creature != null)
        {
            creature.Fuerza += 5;
            creature.Vitalidad += 5;
            creature.Velocidad -= 5;
            Debug.Log("Fuerza y vitalidad aumentada en +5");
            Debug.Log("Velocidad disminuida en -5");
        }
    }

    void Start()
    {

        // Carga el sprite desde la carpeta Resources
        targetSpriteName = "Cuerpo";
        newSprite = Resources.Load<Sprite>("Sprites/Criaturas/Evolvers/Cuerpos/EvolverCuerpoGrande");

        // Aplica el cambio de sprite
        Appel();
        ApplyEffect();
    }

}
