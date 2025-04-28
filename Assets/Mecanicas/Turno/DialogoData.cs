using System.Collections.Generic;
using UnityEngine;

public class DialogoData : MonoBehaviour
{
    public static DialogoData Instance { get; private set; }

    private HashSet<int> dialogosDestruidos = new HashSet<int>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MarcarDialogoDestruido(int indice)
    {
        if (!dialogosDestruidos.Contains(indice))
        {
            dialogosDestruidos.Add(indice);
        }
    }

    public bool EstaDialogoDestruido(int indice)
    {
        return dialogosDestruidos.Contains(indice);
    }
}
