using System.Collections;
using UnityEngine;

public class SimpleMovetment : MonoBehaviour
{
    //Creamos una variable para la velocidad
    public float velocidad = 5f;
    //Creamos una variable para el tamaño de la casilla
    public float tamañoCasilla = 1f;

    // Variable para controlar si el objeto está en movimiento
    private bool enMovimiento = false;

    // Update is called once per frame
    void Update()
    {
        // Si el objeto no está en movimiento, aceptamos input
        if (!enMovimiento)
        {
            //Creamos una variable para guardar el movimiento
            Vector3 movimiento = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

            //Si hay movimiento, movemos el objeto a la siguiente casilla
            if (movimiento != Vector3.zero)
            {
                StartCoroutine(MoverObjeto(movimiento));
            }
        }
    }

    // Corrutina para mover el objeto
    private IEnumerator MoverObjeto(Vector3 movimiento)
    {
        enMovimiento = true;
        Vector3 nuevaPosicion = transform.position + movimiento.normalized * tamañoCasilla;
        while (transform.position != nuevaPosicion)
        {
            transform.position = Vector3.MoveTowards(transform.position, nuevaPosicion, velocidad * Time.deltaTime);
            yield return null;
        }
        enMovimiento = false;
    }
}
