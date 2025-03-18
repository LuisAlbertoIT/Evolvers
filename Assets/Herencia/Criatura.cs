using UnityEngine;

using System.Collections.Generic;

public class Criatura : MonoBehaviour
{
    string Nombre;
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

    public SpriteRenderer[] sprites; // Sprites de la criatura (Ojos, Cuerpo, etc.)
    public TraitBase[] traits; // Traits que afectan su comportamiento

    public Criatura(string nombre, int vida, int vidaMax, int energia, int energiaMax, int acciones, int accionesMax, int vitalidad, int vigor, int fuerza, int adaptabilidad, int inteligencia, int velocidad, int metabolismo)
    {
        Nombre = nombre;
        Vida = vida;
        VidaMax = vidaMax;
        Energia = energia;
        EnergiaMax = energiaMax;
        Acciones = acciones;
        AccionesMax = accionesMax;
        Vitalidad = vitalidad;
        Vigor = vigor;
        Fuerza = fuerza;
        Adaptabilidad = adaptabilidad;
        Inteligencia = inteligencia;
        Velocidad = velocidad;
        Metabolismo = metabolismo;
    }


    public List<TraitBase> Traits = new List<TraitBase>();

    public void AddTrait(TraitBase trait)
    {
        Traits.Add(trait);
        gameObject.AddComponent(trait.GetType()); // Agrega el script como componente
    }
}

