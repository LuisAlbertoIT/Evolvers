using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    public List<GameObject> personajes; // Lista de personajes para spawnear
    public Camera mainCamera; // Cámara principal
    private List<Collider2D> zonasOcupadas = new List<Collider2D>(); // Lista de zonas ocupadas
    public GameObject ActivadorTurnManager;
    public GameObject creaturePrefab;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Obtener la cámara principal si no se asigna
        }

        List<CharacterData> loadedData = SaveManager.LoadGame();
        if (loadedData != null && loadedData.Count > 0)
        {
            foreach (CharacterData data in loadedData)
            {
                GameObject newCreature = Instantiate(creaturePrefab);
                CharacterInfo characterInfo = newCreature.GetComponent<CharacterInfo>();

                if (characterInfo != null)
                {
                    characterInfo.ID = data.ID;
                    characterInfo.characterName = data.characterName;
                    characterInfo.maxHP = data.maxHP;
                    characterInfo.currentHP = data.currentHP;
                    characterInfo.attack = data.attack;
                    characterInfo.defense = data.defense;
                    characterInfo.level = data.level;

                    personajes.Add(newCreature);
                    newCreature.SetActive(false);
                }
            }
        }
        else
        {
            Debug.LogWarning("No hay personajes guardados, asegúrate de que se haya guardado al menos uno.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detectar clic izquierdo del mouse
        {
            SpawnPersonaje();
        }
    }

    void SpawnPersonaje()
    {
        if (personajes.Count == 0) // Verificar si todavía hay personajes disponibles
        {
            Debug.Log("Todos los personajes ya han sido spawneados.");

            if (ActivadorTurnManager != null)
            {
                ActivadorTurnManager.SetActive(true); // Activar el GameObject al terminar
            } 
            return;

        }

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z); // Profundidad consistente con la cámara
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Convertir a 2D para la detección con Physics2D
        Vector2 worldPosition2D = new Vector2(worldPosition.x, worldPosition.y);

        // Detectar todos los objetos debajo del punto del mouse
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(worldPosition2D);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("TileArea")) // Verificar el tag "TileArea"
            {
                if (zonasOcupadas.Contains(hitCollider)) // Verificar si la zona ya fue ocupada
                {
                    Debug.Log("Esta zona ya tiene un personaje spawneado.");
                    return;
                }

                // Obtener el centro del Collider2D
                Vector3 centroZona = hitCollider.bounds.center;

                // Spawnear personaje en el centro de la zona
                int randomIndex = Random.Range(0, personajes.Count); // Elegir un personaje aleatorio
                GameObject personajeSeleccionado = personajes[randomIndex];

                Instantiate(personajeSeleccionado, centroZona, Quaternion.identity); // Spawnear personaje
                personajes.RemoveAt(randomIndex); // Eliminar personaje de la lista

                zonasOcupadas.Add(hitCollider); // Marcar la zona como ocupada
                Debug.Log($"Personaje spawneado en la zona: {hitCollider.name} en posición {centroZona}");
                return; // Salir después de spawnear un personaje
            }
        }

        Debug.Log("El clic no se realizó en una TileArea válida.");
    }
}
