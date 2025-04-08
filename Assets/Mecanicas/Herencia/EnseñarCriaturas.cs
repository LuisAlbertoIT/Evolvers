using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnseñarCriaturas : MonoBehaviour
{
    public GameObject criaturaPrefab; // Prefab que contiene los elementos de UI
    public Transform parentTransform; // Transform del contenedor donde se instanciarán los prefabs

    void Start()
    {
        MostrarCriaturas();
    }

    void MostrarCriaturas()
    {
        foreach (Criatura criatura in GameManager.instancia.listaCriaturas)
        {
            GameObject criaturaUI = Instantiate(criaturaPrefab, parentTransform);

            // Asignar nombre
            Text nombreText = criaturaUI.transform.Find("NombreText").GetComponent<Text>();
            nombreText.text = criatura.Nombre;

            // Asignar stats
            Text statsText = criaturaUI.transform.Find("StatsText").GetComponent<Text>();
            statsText.text = $"Vida: {criatura.Vida}/{criatura.VidaMax}\n" +
                             $"Energía: {criatura.Energia}/{criatura.EnergiaMax}\n" +
                             $"Acciones: {criatura.Acciones}/{criatura.AccionesMax}\n" +
                             $"Vitalidad: {criatura.Vitalidad}\n" +
                             $"Vigor: {criatura.Vigor}\n" +
                             $"Fuerza: {criatura.Fuerza}\n" +
                             $"Resistencia: {criatura.Resistencia}\n" +
                             $"Adaptabilidad: {criatura.Adaptabilidad}\n" +
                             $"Inteligencia: {criatura.Inteligencia}\n" +
                             $"Velocidad: {criatura.Velocidad}\n" +
                             $"Metabolismo: {criatura.Metabolismo}";

            // Asignar traits
            Text traitsText = criaturaUI.transform.Find("TraitsText").GetComponent<Text>();
            traitsText.text = string.Join(", ", criatura.traits.Select(trait => trait.GetType().Name));

            // Asignar sprites
            Transform spritesContainer = criaturaUI.transform.Find("SpritesContainer");
            foreach (SpriteRenderer spriteRenderer in criatura.sprites)
            {
                GameObject spriteGO = new GameObject(spriteRenderer.name);
                Image spriteImage = spriteGO.AddComponent<Image>();
                spriteImage.sprite = spriteRenderer.sprite;
                spriteGO.transform.SetParent(spritesContainer, false);
            }

            // Asignar imagen de fondo (opcional)
            Image backgroundImage = criaturaUI.GetComponent<Image>();
            // backgroundImage.sprite = ...; // Asignar la imagen de fondo si es necesario
        }
    }
}

