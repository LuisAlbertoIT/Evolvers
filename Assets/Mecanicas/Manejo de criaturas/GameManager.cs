using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;


    public List<Criatura> listaCriaturas = new List<Criatura>();  // Criaturas que posees
    public List<Criatura> listaExpedicion = new List<Criatura>(); // Criaturas en expedición
    public List<Criatura> listaSinIncubar = new List<Criatura>(); // Método para agregar una criatura a la lista de criaturas sin incubar
   
    public void AgregarCriaturaSinIncubar(Criatura criatura)
    {
        listaSinIncubar.Add(criatura);
        DontDestroyOnLoad(criatura.gameObject);
    }

    // Método para incubar una criatura y moverla a la lista de criaturas poseídas
    public void IncubarCriatura(Criatura criatura)
    {
        if (listaSinIncubar.Contains(criatura))
        {
            listaSinIncubar.Remove(criatura);
            listaCriaturas.Add(criatura);
            DontDestroyOnLoad(criatura.gameObject);
        }
    }

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager inicializado correctamente.");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Método para agregar una criatura a la lista de criaturas poseídas
    public void AgregarCriatura(Criatura criatura)
    {
        listaCriaturas.Add(criatura);
        DontDestroyOnLoad(criatura.gameObject);
    }

    // Método para enviar una criatura a la expedición
    public void EnviarAExpedicion(Criatura criatura)
    {
        if (listaCriaturas.Contains(criatura))
        {
            listaCriaturas.Remove(criatura);
            listaExpedicion.Add(criatura);
            DontDestroyOnLoad(criatura.gameObject);
        }
    }

    // Método para regresar una criatura de la expedición
    public void RegresarDeExpedicion(Criatura criatura)
    {
        if (listaExpedicion.Contains(criatura))
        {
            listaExpedicion.Remove(criatura);
            listaCriaturas.Add(criatura);
            DontDestroyOnLoad(criatura.gameObject);
        }
    }

    // Método para desactivar todas las criaturas en la próxima escena
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

        foreach (var criatura in listaSinIncubar)
        {
            criatura.gameObject.SetActive(false);
        }
    }
}

