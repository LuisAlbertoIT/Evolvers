using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public List<Criatura> listaCriaturas = new List<Criatura>();  // Criaturas que posees
    public List<Criatura> listaExpedicion = new List<Criatura>(); // Criaturas en expedici�n
    public List<Criatura> listaSinIncubar = new List<Criatura>(); // Criaturas sin incubar

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // M�todo para agregar una criatura a la lista de criaturas pose�das
    public void AgregarCriatura(Criatura criatura)
    {
        listaCriaturas.Add(criatura);
        DontDestroyOnLoad(criatura.gameObject);
    }

    // M�todo para enviar una criatura a la expedici�n
    public void EnviarAExpedicion(Criatura criatura)
    {
        if (listaCriaturas.Contains(criatura))
        {
            listaCriaturas.Remove(criatura);
            listaExpedicion.Add(criatura);
            DontDestroyOnLoad(criatura.gameObject);
        }
    }

    // M�todo para regresar una criatura de la expedici�n
    public void RegresarDeExpedicion(Criatura criatura)
    {
        if (listaExpedicion.Contains(criatura))
        {
            listaExpedicion.Remove(criatura);
            listaCriaturas.Add(criatura);
            DontDestroyOnLoad(criatura.gameObject);
        }
    }

    // M�todo para desactivar todas las criaturas en la pr�xima escena
    public void DesactivarCriaturasEnProximaEscena()
    {
        foreach (var criatura in listaCriaturas)
        {
            criatura.gameObject.SetActive(false);
        }

        foreach (var criatura in listaExpedicion)
        {
            criatura.gameObject.SetActive(false);
        }
    }
}

