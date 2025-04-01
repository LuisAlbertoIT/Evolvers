using UnityEngine;

using System.Collections.Generic;

public class Criatura : MonoBehaviour
{
    public string Nombre;
    public int Vida;
    public int VidaMax;

    public int Energia;
    public int EnergiaMax;

    public int Acciones;
    public int AccionesMax;

    public int Vitalidad;
    public int Vigor;
    public int Fuerza;
    public int resistencia;
    public int Adaptabilidad;
    public int Inteligencia;
    public int Velocidad;
    public int Metabolismo;

    public SpriteRenderer[] sprites; // Sprites de la criatura (Ojos, Cuerpo, etc.)
    public List<TraitBase> traits = new List<TraitBase>(); // Traits que afectan su comportamiento

    public Criatura(string nombre, int vida, int vidaMax, int energia, int energiaMax, int acciones, int accionesMax, int vitalidad, int vigor, int fuerza,  int adaptabilidad, int inteligencia, int velocidad, int metabolismo, SpriteRenderer[] sprites, TraitBase[] traits, int resistencia)
    {
        this.Nombre = nombre;
        this.Vida = vida;
        this.VidaMax = vidaMax;
        this.Energia = energia;
        this.EnergiaMax = energiaMax;
        this.Acciones = acciones;
        this.AccionesMax = accionesMax;
        this.Vitalidad = vitalidad;
        this.Vigor = vigor;
        this.Fuerza = fuerza;
        this.Adaptabilidad = adaptabilidad;
        this.Inteligencia = inteligencia;
        this.Velocidad = velocidad;
        this.Metabolismo = metabolismo;
        this.traits.AddRange(traits);
        this.sprites = sprites;
        this.resistencia = resistencia;
    }

    public void AddTrait(TraitBase trait)
    {
        traits.Add(trait);
        gameObject.AddComponent(trait.GetType()); // Agrega el script como componente
    }

    // Start is called before the first frame update
    void Start()
    {
        //añadir todos los traits a la criatura
        foreach (TraitBase trait in GetComponents<TraitBase>())
        {
            traits.Add(trait);
        }

      //foreach (TraitBase trait in traits)
       // {
       //     trait.ApplyEffect();
       // }

        // agregar todos los sprites al array de sprites
        sprites = GetComponentsInChildren<SpriteRenderer>();
    }
}

