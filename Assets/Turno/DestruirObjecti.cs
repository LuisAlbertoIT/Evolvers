using UnityEngine;

public class DestruirObjecti : MonoBehaviour
{
    CapsuleCollider2D capsuleCollider;
    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

<<<<<<< Updated upstream
        if (collision.tag == "weapon")
=======
        if (collision.tag == "Player")
>>>>>>> Stashed changes
        {
            print("des");
            Destroy(gameObject);
            
        }
    }
}