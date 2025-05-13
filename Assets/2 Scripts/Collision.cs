using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public GameObject shockAnimationPrefab; // Assign your DeathAnimationPrefab in the Inspector

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) // Adjust the tag as needed
        {
            // Instantiate the death animation prefab at the collision point
            Instantiate(shockAnimationPrefab, collision.contacts[0].point, Quaternion.identity);

            // Destroy the colliding enemy (or any other logic you need)
            Destroy(collision.gameObject);
        }
    }
}