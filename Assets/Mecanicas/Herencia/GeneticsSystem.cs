using System.Collections.Generic;
using UnityEngine;

public static class GeneticsSystem
{
    public static Criatura BreedCreatures(Criatura parent1, Criatura parent2)
    {
        GameObject childObject = new GameObject(parent1.Nombre + "Junior");
        Criatura child = childObject.AddComponent<Criatura>();
        child.traits = new List<TraitBase>(); // Aseg�rate de inicializar la lista

        // A�adir prefab a la nueva criatura  
        /*GameObject prefab = Resources.Load<GameObject>("Sprites/Criaturas/Evolvers/Evolver");
        if (prefab != null)
        {
            GameObject.Instantiate(prefab, childObject.transform);
        }*/

        // Herencia de stats  
        child.Fuerza = Random.Range(parent1.Fuerza, parent2.Fuerza + 1);
        child.Vitalidad = Random.Range(parent1.Vitalidad, parent2.Vitalidad + 1);
        child.Vigor = Random.Range(parent1.Vigor, parent2.Vigor + 1);
        child.Adaptabilidad = Random.Range(parent1.Adaptabilidad, parent2.Adaptabilidad + 1);
        child.Inteligencia = Random.Range(parent1.Inteligencia, parent2.Inteligencia + 1);
        child.Velocidad = Random.Range(parent1.Velocidad, parent2.Velocidad + 1);
        child.Metabolismo = Random.Range(parent1.Metabolismo, parent2.Metabolismo + 1);

        child.VidaMax = child.Vitalidad * 10;
        child.Vida = child.VidaMax;
        child.EnergiaMax = child.Vigor * 10;
        child.Energia = child.EnergiaMax;

        child.AccionesMax = 3;
        child.Acciones = child.AccionesMax;

        // Herencia de sprites  
        child.sprites = new SpriteRenderer[parent1.sprites.Length];
        for (int i = 0; i < parent1.sprites.Length; i++)
        {
            if (parent1.sprites[i] != null)
            {
                // Crear un nuevo GameObject para el sprite
                GameObject spriteObject = new GameObject(parent1.sprites[i].name);
                spriteObject.transform.SetParent(childObject.transform);

                // A�adir un SpriteRenderer al nuevo GameObject
                SpriteRenderer spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = parent1.sprites[i].sprite;

                // Copiar propiedades del SpriteRenderer del padre
                spriteRenderer.sortingLayerID = parent1.sprites[i].sortingLayerID;
                spriteRenderer.sortingOrder = parent1.sprites[i].sortingOrder;
                spriteRenderer.color = parent1.sprites[i].color; // Opcional: heredar el color del sprite

                // Asignar el SpriteRenderer al array de sprites del hijo
                child.sprites[i] = spriteRenderer;
            }
        }


        // Herencia de traits (50% de probabilidad por cada trait)

        foreach (TraitBase trait in parent1.traits)
        {
            if (trait != null && Random.value < 0.5f)
            {
                TraitBase newTrait = childObject.AddComponent(trait.GetType()) as TraitBase;
                if (newTrait != null)
                {
                    child.AddTrait(newTrait);
                }
                else
                {
                    Debug.LogError($"No se pudo a�adir el trait del tipo {trait.GetType()} al hijo.");
                }
            }
        }

        foreach (TraitBase trait in parent2.traits)
        {
            if (trait != null && Random.value < 0.5f && !child.traits.Exists(t => t.GetType() == trait.GetType()))
            {
                TraitBase newTrait = childObject.AddComponent(trait.GetType()) as TraitBase;
                if (newTrait != null)
                {
                    child.AddTrait(newTrait);
                }
                else
                {
                    Debug.LogError($"No se pudo a�adir el trait del tipo {trait.GetType()} al hijo.");
                }
            }
        }

        // Mutaciones (10% de probabilidad)  
        List<System.Type> possibleMutations = new List<System.Type> { typeof(TraitColorChanger), typeof(TraitFuerzaExtra) };
        if (Random.value < 0.1f)
        {
            System.Type mutation = possibleMutations[Random.Range(0, possibleMutations.Count)];
            if (!child.traits.Exists(t => t.GetType() == mutation))
                child.AddTrait(childObject.AddComponent(mutation) as TraitBase);
        }

        return child;
    }
}

