using UnityEngine;

using System.Collections.Generic;

public class Creature : MonoBehaviour
{
    public int Vida;
    public int VidaMax;

    public int Energia;
    public int EnergiaMax;

    public int Acciones;
    public int AccionesMax;

    public int Vitalidad;
    public int Vigor;
    public int Fuerza;
    public int Adaptabilidad;
    public int Inteligencia;
    public int Velocidad;
    public int Metabolismo;



    public List<TraitBase> Traits = new List<TraitBase>();

    public void AddTrait(TraitBase trait)
    {
        Traits.Add(trait);
        gameObject.AddComponent(trait.GetType()); // Agrega el script como componente
    }
}

