using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectedCreature : MonoBehaviour
{
    private GameManager gameManager;
    private HorizontalLayoutGroup hlg;
    public Button button;
    public GameObject creaturePanel;
    public GameObject infoPanel;
    public GameObject selectedCreature;

    void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        hlg = GetComponentInChildren<HorizontalLayoutGroup>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnEnable()
    {
        foreach (Criatura creature in gameManager.listaCriaturas)
        {
            Button btn = Instantiate(button, hlg.transform, false);
            btn.GetComponentInChildren<TMP_Text>().text = creature.Nombre;
            btn.onClick.AddListener(ShowInfo);
        }
    }

    private void OnDisable()
    {
        for (int i = hlg.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(hlg.transform.GetChild(i).gameObject);
        }
    }

    private void ShowInfo()
    {
        creaturePanel.SetActive(false);
        infoPanel.SetActive(true);
        foreach (Criatura creature in gameManager.listaCriaturas)
        {
            if (creature.Nombre == EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMP_Text>().text)
            {
                GameObject.Find("Nombre").GetComponent<TMP_Text>().text = $"Nombre: {creature.Nombre}";
                GameObject.Find("Vida").GetComponent<TMP_Text>().text = $"Vida: {creature.VidaMax}";
                GameObject.Find("Energia").GetComponent<TMP_Text>().text = $"Energia: {creature.EnergiaMax}";
                GameObject.Find("Acciones").GetComponent<TMP_Text>().text = $"Acciones: {creature.AccionesMax}";
                GameObject.Find("Vitalidad").GetComponent<TMP_Text>().text = $"Vitalidad: {creature.Vitalidad}";
                GameObject.Find("Vigor").GetComponent<TMP_Text>().text = $"Vigor: {creature.Vigor}";
                GameObject.Find("Fuerza").GetComponent<TMP_Text>().text = $"Fuerza: {creature.Fuerza}";
                GameObject.Find("Adaptabilidad").GetComponent<TMP_Text>().text = $"Adaptabilidad: {creature.Adaptabilidad}";
                GameObject.Find("Inteligencia").GetComponent<TMP_Text>().text = $"Inteligencia: {creature.Inteligencia}";
                GameObject.Find("Velocidad").GetComponent<TMP_Text>().text = $"Velocidad: {creature.Velocidad}";
                GameObject.Find("Metabolismo").GetComponent<TMP_Text>().text = $"Metabolismo: {creature.Metabolismo}";


                break;
            }
        }
    }
}