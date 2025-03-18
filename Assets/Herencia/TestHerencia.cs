using UnityEngine;

public class TestHerencia : MonoBehaviour
{
    [SerializeField] GameObject Evo1;
    [SerializeField] GameObject Evo2;

    void Start()
    {
        Criatura obj1 = Evo1.GetComponent<Criatura>();
        Criatura obj2 = Evo2.GetComponent<Criatura>();

        Criatura child = GeneticsSystem.BreedCreatures(obj1, obj2);

        Debug.Log($"Criatura Hija: Fuerza {child.Fuerza}, Velocidad {child.Velocidad}, Inteligencia {child.Inteligencia}, Vitalidad {child.Vitalidad}, Vigor {child.Adaptabilidad}, Metabolismo {child.Metabolismo}");
        Debug.Log("Traits Heredados:");
        foreach (var trait in child.Traits)
        {
            Debug.Log(trait.GetType().Name);
        }
    }
}

