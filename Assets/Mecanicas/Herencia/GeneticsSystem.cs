using System.Collections.Generic;
using UnityEngine;

public static class GeneticsSystem
{
    public static Criatura BreedCreatures(Criatura parent1, Criatura parent2)
    {
        GameObject childObject = new GameObject("Nueva Criatura");
        Criatura child = childObject.AddComponent<Criatura>();

        // Añadir prefab a la nueva criatura
        GameObject prefab = Resources.Load<GameObject>("Sprites/Criaturas/Evolvers/Evolver");
        if (prefab != null)
        {
            GameObject.Instantiate(prefab, childObject.transform);
        }

        // Herencia de stats
        child.name = parent1.name + "Junior";
        child.Fuerza = Random.Range(parent1.Fuerza, parent2.Fuerza + 1);
        child.Vitalidad = Random.Range(parent1.Vitalidad, parent2.Vitalidad + 1);
        child.Vigor = Random.Range(parent1.Vigor, parent2.Vigor + 1);
        child.Adaptabilidad = Random.Range(parent1.Adaptabilidad, parent2.Adaptabilidad + 1);
        child.Inteligencia = Random.Range(parent1.Inteligencia, parent2.Inteligencia + 1);
        child.Velocidad = Random.Range(parent1.Velocidad, parent2.Velocidad + 1);
        child.Metabolismo = Random.Range(parent1.Metabolismo, parent2.Metabolismo + 1);
        child.Resistencia = Random.Range(parent1.Resistencia, parent2.Resistencia + 1);

        child.VidaMax = child.Vitalidad * 10;
        child.Vida = child.VidaMax;
        child.EnergiaMax = child.Vigor * 10;
        child.Energia = child.EnergiaMax;

        child.AccionesMax = 3;
        child.Acciones = child.AccionesMax;

        // Herencia de traits (50% de probabilidad por cada trait)
        foreach (TraitBase trait in parent1.traits)
        {
            if (Random.value < 0.5f)
                child.AddTrait(childObject.AddComponent(trait.GetType()) as TraitBase);
        }

        foreach (TraitBase trait in parent2.traits)
        {
            if (Random.value < 0.5f && !child.traits.Exists(t => t.GetType() == trait.GetType()))
                child.AddTrait(childObject.AddComponent(trait.GetType()) as TraitBase);
        }

        // Mutaciones (10% de probabilidad)
        List<System.Type> possibleMutations = new List<System.Type> { typeof(TraitColorChanger), typeof(TraitFuerzaExtra) };
        if (Random.value < 0.1f)
        {
            System.Type mutation = possibleMutations[Random.Range(0, possibleMutations.Count)];
            if (!child.traits.Exists(t => t.GetType() == mutation))
                child.AddTrait(childObject.AddComponent(mutation) as TraitBase);
        }

        // Herencia de sprites
        child.sprites = new SpriteRenderer[parent1.sprites.Length];
        for (int i = 0; i < parent1.sprites.Length; i++)
        {
            child.sprites[i] = childObject.AddComponent<SpriteRenderer>();
            child.sprites[i].sprite = parent1.sprites[i].sprite;
        }

        return child;
    }
}

