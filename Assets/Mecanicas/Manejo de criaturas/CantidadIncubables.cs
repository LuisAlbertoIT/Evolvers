using UnityEngine;

public class CantidadIncubables : MonoBehaviour
{
    // Este script toma el valor de la cantidad de criaturas incubables en GameManager
    // y lo asigna a un objeto de texto en la UI.
    public TMPro.TMP_Text cantidadIncubablesText; // Asigna el objeto de texto en la UI desde el inspector
    private GameManager gameManager;

    void Start()
    {
        // Encuentra el GameManager en la escena
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("No se encontró el GameManager en la escena.");
            return;
        }
        // Actualiza el texto al inicio
        UpdateCantidadIncubablesText();
    }

    void Update()
    {
        // Actualiza el texto cada frame
        UpdateCantidadIncubablesText();
    }

    private void UpdateCantidadIncubablesText()
    {
        // Verifica si el GameManager y la lista de criaturas son válidos
        if (gameManager != null && gameManager.listaSinIncubar != null)
        {
            int cantidadIncubables = gameManager.listaSinIncubar.Count;
            cantidadIncubablesText.text =  cantidadIncubables.ToString();
        }
        else
        {
            Debug.LogError("El GameManager o la lista de criaturas no están inicializados correctamente.");
        }
    }
}

