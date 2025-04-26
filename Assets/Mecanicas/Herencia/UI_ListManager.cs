using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UI_ListManager : MonoBehaviour
{
    public GameObject buttonPrefab; // Prefab del botón  
    public Transform contentPanel; // Donde se generarán los botones  
    public Transform selectedPanel; // Donde se mostrarán los seleccionados  
    public Button sendButton; // Botón para enviar los seleccionados  

    private GameManager gameManager;
    private List<Criatura> selectedCriaturas = new List<Criatura>();
    private HashSet<Criatura> displayedCriaturas = new HashSet<Criatura>(); // Para rastrear las criaturas ya mostradas  

    void Start()
    {
        gameManager = GameManager.instancia;
        PopulateList();
        sendButton.onClick.AddListener(SendSelectedCriaturas);
    }

    void Update()
    {
        if (selectedCriaturas.Count >= 2)
        {
            sendButton.interactable = true;
        }
        else
        {
            sendButton.interactable = false;
        }

        // Llamar a PopulateList en cada frame para agregar nuevas criaturas  
        PopulateList();
    }

    void PopulateList()
    {
        if (gameManager.listaCriaturas == null || gameManager.listaCriaturas.Count == 0)
        {
            Debug.LogError("La lista de criaturas está vacía o no está inicializada.");
            return;
        }

        foreach (var criatura in gameManager.listaCriaturas)
        {
            if (criatura == null)
            {
                Debug.LogError("Una de las criaturas en la lista es nula.");
                continue;
            }

            // Verificar si la criatura ya fue mostrada  
            if (displayedCriaturas.Contains(criatura))
            {
                continue;
            }

            GameObject newButton = Instantiate(buttonPrefab, contentPanel);
            TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();

            if (buttonText == null)
            {
                Debug.LogError("El prefab del botón no tiene un componente TextMeshProUGUI.");
                continue;
            }

            if (string.IsNullOrEmpty(criatura.Nombre))
            {
                Debug.LogError("La criatura no tiene un nombre válido.");
                buttonText.text = "SinNombre";
            }
            else
            {
                buttonText.text = criatura.Nombre;
            }

            newButton.GetComponent<Button>().onClick.AddListener(() => OnCriaturaButtonClicked(criatura, newButton));

            // Agregar la criatura a la lista de mostradas  
            displayedCriaturas.Add(criatura);
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



