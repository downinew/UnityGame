using UnityEngine;

public class SplittingProjectile : Projectile
{
  [Tooltip("Which projectile should be spawned by the split.")]
  public GameObject spawnProjectile;

  public override void SpecialAbility()
  {
    SpawnSplit(Vector3.down * 5, Quaternion.AngleAxis(-30f, transform.position));
    SpawnSplit(Vector3.up * 5, Quaternion.AngleAxis(30f, transform.position));
  }

  private void SpawnSplit(Vector3 position, Quaternion rotation)
  {
    GameObject newProjectile = Instantiate(spawnProjectile,
                                    transform.position + position,
                                    transform.rotation * rotation);

    var physicsBody = newProjectile.GetComponent<Rigidbody2D>();
    physicsBody.velocity = projectileRigidBody.velocity;
  }
}
