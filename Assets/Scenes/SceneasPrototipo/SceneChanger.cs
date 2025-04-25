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
    public void VolverAMapaMundi()
    {
        if (ActivarGuiaEnemy.instance != null)
        {
            ActivarGuiaEnemy.instance.regresarDesdeExploracion = true;
            SceneManager.LoadScene("Mapa mundi");
        }
        else
        {
            Debug.LogError("No existe el SceneController. Asegúrate de iniciar desde la escena que lo tiene.");
        }
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
 
}

