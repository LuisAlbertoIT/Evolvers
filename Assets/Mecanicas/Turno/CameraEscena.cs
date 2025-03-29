using System.Collections;
using UnityEngine;

public class CameraEscena : MonoBehaviour
{
    Transform player;

    public float yDistance = 6f;
    public float yMovement = 12f;

    public float xDistance = 11f;
    public float xMovement = 22f;

    private Vector3 cameraDestination;
    public float movementTime = 0.5f;
    private bool isMoving = false;

    // Margen de seguridad para evitar movimiento constante en los límites
    private float movementThreshold = 1f;

    void Start()
    {
        player = FindAnyObjectByType<Movimiento>()?.transform;
        SetCameraFirstPosition();

        if (player == null)
        {
            Debug.LogError("No se encontró el objeto con el script Movimiento.");
            return;
        }

        cameraDestination = transform.position; // Inicializa la posición destino
    }

    void Update()
    {
        if (player == null || isMoving) return; // Evita errores si player es null

        float deltaX = player.position.x - transform.position.x;
        float deltaY = player.position.y - transform.position.y;

        if (deltaY >= yDistance + movementThreshold) // Se mueve solo si supera el margen
        {
            isMoving = true;
            cameraDestination = transform.position + new Vector3(0, yMovement, 0);
            StartCoroutine(MoveCamera());
        }
        else if (deltaY <= -yDistance - movementThreshold)
        {
            isMoving = true;
            cameraDestination = transform.position - new Vector3(0, yMovement, 0);
            StartCoroutine(MoveCamera());
        }
        else if (deltaX >= xDistance + movementThreshold)
        {
            isMoving = true;
            cameraDestination = transform.position + new Vector3(xMovement, 0, 0);
            StartCoroutine(MoveCamera());
        }
        else if (deltaX <= -xDistance - movementThreshold) // Se mueve solo si supera el margen
        {
            isMoving = true;
            cameraDestination = transform.position - new Vector3(xMovement, 0, 0);
            StartCoroutine(MoveCamera());
        }
    }

    private void SetCameraFirstPosition()
    {
        float x = Mathf.Round(player.position.x / xMovement) * xMovement;
        float y = Mathf.Round(player.position.y / yMovement) * yMovement;

        transform.position = new Vector3(x,y, transform.position.z);
        cameraDestination = transform.position;
    }

    IEnumerator MoveCamera()
    {
        Vector3 startPos = transform.position;
        float t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / movementTime;
            transform.position = Vector3.Lerp(startPos, cameraDestination, t);
            transform.position = new Vector3(transform.position.x, transform.position.y, startPos.z); // Mantiene el mismo Z
            yield return null;
        }

        isMoving = false;
    }

}
