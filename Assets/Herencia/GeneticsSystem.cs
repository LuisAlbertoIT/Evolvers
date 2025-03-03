using System.Collections.Generic;
using UnityEngine;

public static class GeneticsSystem
{
    public static Creature BreedCreatures(Creature parent1, Creature parent2)
    {
        GameObject childObject = new GameObject("Nueva Criatura");
        Creature child = childObject.AddComponent<Creature>();

        // Herencia de stats
        child.Fuerza = Random.Range(parent1.Fuerza, parent2.Fuerza + 1);
        child.Vitalidad = Random.Range(parent1.Vitalidad, parent2.Vitalidad + 1);
        child.Vigor = Random.Range(parent1.Vigor, parent2.Vigor + 1);
        child.Adaptabilidad = Random.Range(parent1.Adaptabilidad, parent2.Adaptabilidad + 1);
        child.Inteligencia = Random.Range(parent1.Inteligencia, parent2.Inteligencia + 1);
        child.Velocidad = Random.Range(parent1.Velocidad, parent2.Velocidad + 1);
        child.Metabolismo = Random.Range(parent1.Metabolismo, parent2.Metabolismo + 1);

        // Herencia de traits (50% de probabilidad por cada trait)
        foreach (TraitBase trait in parent1.Traits)
        {
            if (Random.value < 0.5f)
                child.AddTrait(childObject.AddComponent(trait.GetType()) as TraitBase);
        }

        foreach (TraitBase trait in parent2.Traits)
        {
            if (Random.value < 0.5f && !child.Traits.Exists(t => t.GetType() == trait.GetType()))
                child.AddTrait(childObject.AddComponent(trait.GetType()) as TraitBase);
        }

        // Mutaciones (10% de probabilidad)
        List<System.Type> possibleMutations = new List<System.Type> { typeof(TraitColorChanger), typeof(TraitFuerzaExtra) };
        if (Random.value < 0.1f)
        {
            System.Type mutation = possibleMutations[Random.Range(0, possibleMutations.Count)];
            if (!child.Traits.Exists(t => t.GetType() == mutation))
                child.AddTrait(childObject.AddComponent(mutation) as TraitBase);
        }

        return child;
    }
}

