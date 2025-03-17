using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    //Funcion que carga la escena por nombre
    public void ChangeScene(string Base)
    {
        //Carga la escena con el nombre que se le pase
        UnityEngine.SceneManagement.SceneManager.LoadScene(Base);
    }

}
