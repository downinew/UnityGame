using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  private Rigidbody2D projectileRigidBody;

  [SerializeField] [Tooltip("The duration in seconds a projectile is active.")]
  private float lifetime = 30;

  private void Awake()
  {
    projectileRigidBody = GetComponent<Rigidbody2D>();
  }

  public void Shoot(Vector2 force)
  {
    projectileRigidBody.AddForce(force, ForceMode2D.Impulse);
    StartCoroutine(Deactivate());
  }

  private void Update()
  {
    transform.up = Vector2.Lerp(transform.up,
                                projectileRigidBody.velocity.normalized,
                                Time.deltaTime);
  }

  private IEnumerator Deactivate()
  {
    yield return new WaitForSeconds(lifetime);
    Destroy(gameObject);
  }
}