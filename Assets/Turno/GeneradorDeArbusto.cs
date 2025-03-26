using UnityEngine;

public class GeneradorDeArbusto : MonoBehaviour
{
    public GameObject arbusto;
    public GameObject[] Hongos;
    private GameObject arbustoActual;
    public float tiempoReaparicion = 2f; // Tiempo para que reaparezca el arbusto
    private bool esperandoReaparicion = false;
    private bool objetosGenerados = false; // Nueva bandera para evitar dobles llamados

    void Start()
    {
        GenerarArbusto();
    }

    void Update()
    {
        if (arbustoActual == null && !esperandoReaparicion)
        {
            esperandoReaparicion = true;
            if (!objetosGenerados)
            {
                GenerarObjetosAleatorios(); // Se asegura que se llame solo una vez
                objetosGenerados = true;
            }
            Invoke("GenerarArbusto", tiempoReaparicion);
        }
    }

    void GenerarArbusto()
    {
        if (arbustoActual == null)
        {
            arbustoActual = Instantiate(arbusto, transform.position, transform.rotation);
        }
        esperandoReaparicion = false;
        objetosGenerados = false; // Restablece la bandera para el próximo ciclo
    }

    void GenerarObjetosAleatorios()
    {
        int cantidad = Random.Range(0, 2);
        for (int i = 0; i < cantidad; i++)
        {
            GameObject objetoAleatorio = Hongos[Random.Range(0, Hongos.Length)];
            Vector3 posicionDetras = transform.position - transform.forward * 2f;
            Instantiate(objetoAleatorio, posicionDetras, transform.rotation);
        }
    }
}
 