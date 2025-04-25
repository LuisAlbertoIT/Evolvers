using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string nombreEscena;
    [SerializeField] private float transitionTime = 1f;
    public int sceneIndex;

    public Vector2 playerDestination;
    private Animator transitionAnimator;

    private void Start()
    {
        transitionAnimator = GetComponentInChildren<Animator>();
    }
    public void CargarEscena1(string nombreEscena)
    {

        DataInstance.Instance.playerPosition = playerDestination;
        StartCoroutine(SceneLoad1(nombreEscena));


    }
    public void CargarEscena(string nombreEscena)
    {
       
        StartCoroutine(SceneLoad(nombreEscena));
      

    }

    public void EnviarCriaturasAEscena(string nombreEscena)
    {
      
        DontDestroyOnLoad(GameManager.instancia.gameObject);
        SceneManager.LoadScene(nombreEscena);
    }

    public IEnumerator SceneLoad (string nombreEscena)
    {
        transitionAnimator.SetTrigger("StartTranstion");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(nombreEscena);

    }
    public IEnumerator SceneLoad1(string nombreEscena)
    {
        transitionAnimator.SetTrigger("StartTranstion");
        yield return new WaitForSeconds(transitionTime);

        // Cargar la escena y esperar a que esté lista
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nombreEscena);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Esperamos un frame por seguridad para que todo esté inicializado
        yield return null;

        // Buscar y activar el Canvas
        GameObject canvas = GameObject.Find("GuiaEnemy");
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("No se encontró el CanvasEspecial en la nueva escena.");
        }
    }
}

