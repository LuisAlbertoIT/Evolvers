using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string nombreEscena;

    public void CargarEscena(string nombreEscena)
    {
        SceneManager.LoadScene(nombreEscena);
    }

    public void EnviarCriaturasAEscena(string nombreEscena)
    {
        DontDestroyOnLoad(GameManager.instancia.gameObject);
        SceneManager.LoadScene(nombreEscena);
    }
}

