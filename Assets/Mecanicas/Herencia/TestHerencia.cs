using UnityEngine;

public class TestHerencia : MonoBehaviour
{
    public Criatura Evo1;
    public Criatura Evo2;

    public void Mezclar(Criatura a, Criatura b)
    {
        if (a == null || b == null)
        {
            Debug.LogError("Una de las criaturas proporcionadas es nula.");
            return;
        }

        Criatura obj1 = a.GetComponent<Criatura>();
        Criatura obj2 = b.GetComponent<Criatura>();

        if (obj1 == null || obj2 == null)
        {
            Debug.LogError("No se pudo obtener el componente Criatura de uno de los objetos.");
            return;
        }

        Criatura child = GeneticsSystem.BreedCreatures(obj1, obj2);

        if (child == null)
        {
            Debug.LogError("La criatura hija es nula.");
            return;
        }

        Debug.Log($"Criatura Hija: Fuerza {child.Fuerza}, Velocidad {child.Velocidad}, Inteligencia {child.Inteligencia}, Vitalidad {child.Vitalidad}, Vigor {child.Adaptabilidad}, Metabolismo {child.Metabolismo}");
        Debug.Log("Traits Heredados:");

        if (child.traits == null || child.traits.Count <= 0)
        {
            Debug.Log("No tiene traits");
            return;
        }
        else
        {
            foreach (var trait in child.traits)
            {
                if (trait != null)
                {
                    Debug.Log(trait.GetType().Name);
                }
                else
                {
                    Debug.LogError("Uno de los traits heredados es nulo.");
                }
            }
        }
    }
}


