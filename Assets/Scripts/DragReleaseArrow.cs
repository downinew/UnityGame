using UnityEngine;

public class DragReleaseArrow : MonoBehaviour
{
  private Camera mainCamera;
  private Vector2 startPosition;
  private Vector2 endPosition;

  private bool hasStartPosition;
  private bool hasEndPosition;
  
  [SerializeField] private float maxPullForce;
  [SerializeField] private float forceMultiplier;
  [SerializeField] private GameObject projectile;

  private void Start()
  {
    mainCamera = Camera.main;
    startPosition = Vector2.negativeInfinity;
    endPosition = Vector2.negativeInfinity;
  }

  private void Update()
  {
    bool fire1Down = Input.GetButtonDown("Fire1");
    bool fire1Up = Input.GetButtonUp("Fire1");

    if (!hasStartPosition
        && fire1Down)
    {
      // new start position
      startPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      hasStartPosition = true;
    }
    else if (!hasEndPosition
             && fire1Up)
    {
      // new end position
      endPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      hasEndPosition = true;
    }

    if (fire1Up
        && hasStartPosition
        && hasEndPosition)
    {
      // calculate projectile values
      Vector2 projectileVector = startPosition - endPosition;
      float sign = startPosition.y < endPosition.y ? -1.0f : 1.0f;
      float pullForce = Vector2.Distance(startPosition, endPosition);
      float fireAngle = Vector2.Angle(projectileVector, Vector2.right) * sign;
      Quaternion projectileRotation = Quaternion.AngleAxis(fireAngle, Vector3.forward);
      // override pull force when over maximum
      pullForce = pullForce > maxPullForce ? maxPullForce : pullForce;
      Debug.Log($"Start {startPosition} End {endPosition}" +
                $" Direction {projectileVector} PullForce {pullForce} Fire Angle {fireAngle}");
      if (pullForce != 0)
      {
        // create new projectile
        GameObject newProjectile = Instantiate(projectile, startPosition,
                                               projectileRotation);
        var newProjectileRigidbody = newProjectile.GetComponent<Rigidbody2D>();
        if (newProjectileRigidbody != null)
        {
          // newProjectileRigidbody.centerOfMass = new Vector2(0.4f, 0);
          Debug.Log($"COM {newProjectileRigidbody.centerOfMass}");
          // fire projectile
          newProjectileRigidbody.AddForce(projectileVector * (pullForce * forceMultiplier), ForceMode2D.Impulse);
        }
      }
      // reset for new input
      hasStartPosition = hasEndPosition = false;
    }
  }
}