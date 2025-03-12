using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicial : MonoBehaviour
{
    public void jugar()
    {
        SceneManager.LoadScene("Movement");
    }
    public void salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }

    public void Pausar()
    {
        Time.timeScale = 0f;
    }
    public void Reanudar()
    {
        Time.timeScale = 1f;
    }
}