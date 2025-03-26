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

        if (collision.tag == "weapon")
        {
            print("des");
            Destroy(gameObject);
            
        }
    }
}