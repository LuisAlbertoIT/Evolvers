using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogoManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialogo
    {
        public GameObject panelDialogo;
        public TextMeshProUGUI textoDialogo;
        public string[] lineasDialogo;
        public GameObject[] imagenesDialogo;
    }

    private bool hayDialogoActivo = false;

    public Dialogo[] dialogos;
    public float velocidadTexto = 0.05f;
    public GameObject imagenFinal;

    private int dialogoActivo = -1;
    private int indiceLinea = 0;
    private bool escribiendo = false;

    [Header("Botones a controlar")]
    public Button[] botonesAControlar; // <<<<< NUEVO: lista de botones que quieres desactivar/activar

    void Start()
    {
        foreach (var d in dialogos)
        {
            d.panelDialogo.SetActive(false);
            foreach (var imagen in d.imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(false);
            }
        }

        if (imagenFinal != null)
            imagenFinal.SetActive(false);

        ActivarBotones(true); // Asegura que los botones estén activos al principio
    }

    void Update()
    {
        if (hayDialogoActivo && dialogoActivo != -1 && Input.GetKeyDown(KeyCode.Space))
        {
            if (escribiendo)
            {
                StopAllCoroutines();
                dialogos[dialogoActivo].textoDialogo.text = dialogos[dialogoActivo].lineasDialogo[indiceLinea];
                escribiendo = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void ActivarDialogo(int indice)
    {
        if (indice < 0 || indice >= dialogos.Length)
        {
            Debug.LogWarning("Índice de diálogo inválido en ActivarDialogo: " + indice);
            return;
        }

        if (dialogoActivo != -1)
        {
            if (dialogos[dialogoActivo].panelDialogo != null)
                dialogos[dialogoActivo].panelDialogo.SetActive(false);

            foreach (var imagen in dialogos[dialogoActivo].imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(false);
            }
        }

        dialogoActivo = indice;
        indiceLinea = 0;

        // ⚡️ SOLO si existe un panel real, activamos diálogo
        if (dialogos[dialogoActivo].panelDialogo != null)
        {
            hayDialogoActivo = true;
            dialogos[dialogoActivo].panelDialogo.SetActive(true);

            foreach (var imagen in dialogos[dialogoActivo].imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(true);
            }

            ActivarBotones(false); // <<<<<< desactivamos botones SOLO si hay diálogo real
            StartCoroutine(EscribirLinea());
        }
        else
        {
            // Si el panel fue destruido, aseguramos que botones estén activos
            hayDialogoActivo = false;
            ActivarBotones(true); // <<<<<< aseguramos botones activos si no hay panel
            Debug.LogWarning("No se puede activar diálogo: Panel destruido.");
        }
    }

    IEnumerator EscribirLinea()
    {
        escribiendo = true;
        dialogos[dialogoActivo].textoDialogo.text = "";

        foreach (char letra in dialogos[dialogoActivo].lineasDialogo[indiceLinea].ToCharArray())
        {
            dialogos[dialogoActivo].textoDialogo.text += letra;
            yield return new WaitForSeconds(velocidadTexto);
        }

        escribiendo = false;
    }

    public void NextLine()
    {
        indiceLinea++;

        if (indiceLinea < dialogos[dialogoActivo].lineasDialogo.Length)
        {
            StartCoroutine(EscribirLinea());
        }
        else
        {
            if (imagenFinal != null)
                imagenFinal.SetActive(true);

            foreach (var imagen in dialogos[dialogoActivo].imagenesDialogo)
            {
                if (imagen != null)
                    imagen.SetActive(false);
            }

            Destroy(dialogos[dialogoActivo].panelDialogo);
            dialogoActivo = -1;
            hayDialogoActivo = false;

            ActivarBotones(true); // <<<<< ACTIVAR BOTONES cuando termina el diálogo normalmente
        }
    }

    public void DestruirDialogo(int indice)
    {
        if (indice < 0 || indice >= dialogos.Length)
        {
            Debug.LogWarning("Índice de diálogo fuera de rango al intentar destruir: " + indice);
            return;
        }

        if (dialogoActivo == indice)
        {
            StopAllCoroutines();
            escribiendo = false;
            dialogoActivo = -1;
            hayDialogoActivo = false;

            if (dialogos[indice].textoDialogo != null)
            {
                dialogos[indice].textoDialogo.text = string.Empty;
            }
        }

        if (dialogos[indice].panelDialogo != null)
        {
            Destroy(dialogos[indice].panelDialogo);
            dialogos[indice].panelDialogo = null;
        }

        ActivarBotones(true); // <<<<< ACTIVAR BOTONES si se destruye manualmente
    }

    private void ActivarBotones(bool estado)
    {
        if (botonesAControlar != null)
        {
            foreach (var boton in botonesAControlar)
            {
                if (boton != null)
                    boton.interactable = estado;
            }
        }
    }
}
