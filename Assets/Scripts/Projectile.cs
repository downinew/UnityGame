using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
  protected Rigidbody2D projectileRigidBody;

  [SerializeField] [Tooltip("Defines how long after shooting that the special is activatable.")]
  private float specialActiveTime = 10f;

  [SerializeField] [Tooltip("The duration in seconds a projectile is active.")]
  private float lifetime = 30;

  private float timeLeft;
  private bool specialUsed = false;

  private void Awake()
  {
    projectileRigidBody = GetComponent<Rigidbody2D>();
    timeLeft = specialActiveTime;
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

    // make sure we can't wrap around to positive number again
    if (timeLeft >= 0) 
    {
      timeLeft -= Time.deltaTime;
    }
    
    // only allow special for a certain time after spawned projectile
    if (timeLeft > 0 && !specialUsed && Input.GetButtonUp("Jump"))
    {
      specialUsed = true;
      SpecialAbility();
    }
  }

  private IEnumerator Deactivate()
  {
    yield return new WaitForSeconds(lifetime);
    Destroy(gameObject);
  }

  public virtual void SpecialAbility()
  {
    // do nothing
  }
}