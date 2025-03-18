using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    public List<Criatura> listaCriaturas = new List<Criatura>();  // Criaturas que posees
    public List<Criatura> listaExpedicion = new List<Criatura>(); // Criaturas en expedición

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

    // Método para agregar una criatura a la lista de criaturas poseídas
    public void AgregarCriatura(Criatura criatura)
    {
        listaCriaturas.Add(criatura);
    }

    // Método para enviar una criatura a la expedición
    public void EnviarAExpedicion(Criatura criatura)
    {
        if (listaCriaturas.Contains(criatura))
        {
            listaCriaturas.Remove(criatura);
            listaExpedicion.Add(criatura);
        }
    }

    // Método para regresar una criatura de la expedición
    public void RegresarDeExpedicion(Criatura criatura)
    {
        if (listaExpedicion.Contains(criatura))
        {
            listaExpedicion.Remove(criatura);
            listaCriaturas.Add(criatura);
        }
    }
}

