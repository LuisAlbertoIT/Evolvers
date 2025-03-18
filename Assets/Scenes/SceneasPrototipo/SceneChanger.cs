using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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

