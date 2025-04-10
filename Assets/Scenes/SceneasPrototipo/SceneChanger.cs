using System.Collections;
using System.Transactions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string nombreEscena;
    [SerializeField] private float transitionTime = 1f;

    private Animator transitionAnimator;

    private void Start()
    {
        transitionAnimator = GetComponentInChildren<Animator>();
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

