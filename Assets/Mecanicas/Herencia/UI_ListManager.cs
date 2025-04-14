using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UI_ListManager : MonoBehaviour
{
    public GameObject buttonPrefab; // Prefab del bot�n
    public Transform contentPanel; // Donde se generar�n los botones
    public Transform selectedPanel; // Donde se mostrar�n los seleccionados
    public Button sendButton; // Bot�n para enviar los seleccionados

    private GameManager gameManager;
    private List<Criatura> selectedCriaturas = new List<Criatura>();

    void Start()
    {
        TestHerencia testHerencia = FindFirstObjectByType<TestHerencia>();

        gameManager = GameManager.instancia;
        PopulateList();
        sendButton.onClick.AddListener(SendSelectedCriaturas);
    }

    void PopulateList()
    {
        if (gameManager.listaCriaturas == null || gameManager.listaCriaturas.Count == 0)
        {
            Debug.LogError("La lista de criaturas est� vac�a o no est� inicializada.");
            return;
        }

        foreach (var criatura in gameManager.listaCriaturas)
        {
            if (criatura == null)
            {
                Debug.LogError("Una de las criaturas en la lista es nula.");
                continue;
            }

            GameObject newButton = Instantiate(buttonPrefab, contentPanel);
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();

            if (buttonText == null)
            {
                Debug.LogError("El prefab del bot�n no tiene un componente TextMeshProUGUI.");
                continue;
            }

            if (string.IsNullOrEmpty(criatura.Nombre))
            {
                Debug.LogError("La criatura no tiene un nombre v�lido.");
                buttonText.text = "SinNombre";
            }
            else
            {
                buttonText.text = criatura.Nombre;
            }

            newButton.GetComponent<Button>().onClick.AddListener(() => OnCriaturaButtonClicked(criatura, newButton));
        }
    }

    void OnCriaturaButtonClicked(Criatura criatura, GameObject button)
    {
        if (selectedCriaturas.Contains(criatura))
        {
            selectedCriaturas.Remove(criatura);
            button.transform.SetParent(contentPanel, false);
        }
        else
        {
            selectedCriaturas.Add(criatura);
            button.transform.SetParent(selectedPanel, false);
        }
    }

    void SendSelectedCriaturas()
    {
        TestHerencia testHerencia = FindFirstObjectByType<TestHerencia>();
        if (testHerencia != null && selectedCriaturas.Count >= 2)
        {
            testHerencia.Mezclar(selectedCriaturas[0], selectedCriaturas[1]);
        }
    }
}



