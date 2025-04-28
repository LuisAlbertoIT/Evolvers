using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.ObjectModel;

public class Dialogo : MonoBehaviour
{

    public TextMeshProUGUI dialogoIncubar;
    public string[] lines;
    public float textSpeed = 0.1f;
    public GameObject imagenFinal;

    [Header("Botones a controlar")]
    public Button[] botonesAControlar;
    public bool desactivarBotonesCompletamente = false; 
    int index;
    bool isDestroyed = false;
    private Movimiento movimiento;
    private string saveKey;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        saveKey = "Dialogo_" + gameObject.name;
    }

    [System.Obsolete]
    void Start()
    {
        movimiento = FindObjectOfType<Movimiento>();

        if (DialogoData.Instance != null && DialogoData.Instance.EstaDialogoDestruido(saveKey.GetHashCode()))
        {
            isDestroyed = true;
            gameObject.SetActive(false);
            return;
        }

        if (imagenFinal != null)
            imagenFinal.SetActive(true);

        dialogoIncubar.text = string.Empty;
        StarDialogue();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isDestroyed) return;

            if (dialogoIncubar.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogoIncubar.text = lines[index];
            }
        }
    }

    public void StarDialogue()
    {
        index = 0;
        StartCoroutine(WriteLine());

        if (movimiento != null)
            movimiento.enabled = false;

        ActivarBotones(false); 
    }

    IEnumerator WriteLine()
    {
        dialogoIncubar.text = string.Empty;
        foreach (char letter in lines[index].ToCharArray())
        {
            dialogoIncubar.text += letter;
            yield return new WaitForSecondsRealtime(textSpeed);
        }

        if (imagenFinal != null)
            imagenFinal.SetActive(true);
    }

    public void NextLine()
    {
        if (isDestroyed) return;

        if (index < lines.Length - 1)
        {
            index++;
            dialogoIncubar.text = string.Empty;
            StartCoroutine(WriteLine());
        }
        else
        {
            DestroyDialogue();
        }
    }

    public void DestroyDialogue()
    {
        if (isDestroyed) return;

        isDestroyed = true;

        if (imagenFinal != null)
            imagenFinal.SetActive(false);

        StopAllCoroutines();

        if (DialogoData.Instance != null)
        {
            DialogoData.Instance.MarcarDialogoDestruido(saveKey.GetHashCode());
        }

        gameObject.SetActive(false);

        if (movimiento != null)
            movimiento.enabled = true;

        ActivarBotones(true); 
    }

    public void OnButtonDestroyPressed()
    {
        DestroyDialogue();
    }

    private void ActivarBotones(bool estado)
    {
        if (botonesAControlar != null)
        {
            foreach (var boton in botonesAControlar)
            {
                if (boton != null)
                {
                    if (desactivarBotonesCompletamente)
                    {
                      
                        boton.gameObject.SetActive(estado);
                    }
                    else
                    {
                        
                        boton.interactable = estado;
                    }
                }
            }
        }
    }
}
