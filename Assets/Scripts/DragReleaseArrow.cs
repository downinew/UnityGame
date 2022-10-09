using UnityEngine;
using UnityEngine.UIElements;

public class DragReleaseArrow : MonoBehaviour
{
  private Camera mainCamera;
  private LineRenderer guideLineRenderer;
  private Vector2 startPosition;
  private Vector2 endPosition;
  private GameObject guideLine;

  private int selectedProjectileIndex;
  private bool hasStartPosition;
  private bool hasEndPosition;
  
  [SerializeField] [Tooltip("The maximum force that can be applied to a projectile by dragging with the mouse.")]
  private float maxPullForce;
  [SerializeField] [Tooltip("The force multiplier which gives projectiles more zaz.")]
  private float forceMultiplier;
  [SerializeField] [Tooltip("The projectiles that can be fired.")]
  private GameObject[] projectiles;

  private void Start()
  {
    mainCamera = Camera.main;
    guideLineRenderer = GetComponent<LineRenderer>();
    startPosition = Vector2.negativeInfinity;
    endPosition = Vector2.negativeInfinity;
  }

  private void Update()
  {
    Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
    bool fire1Down = Input.GetButtonDown("Fire1");
    bool fire1Up = Input.GetButtonUp("Fire1");

    if (!hasStartPosition
        && fire1Down)
    {
      // new start position
      startPosition = mousePosition;
      hasStartPosition = true;
    }
    else if (!hasEndPosition
             && fire1Up)
    {
      // new end position
      endPosition = mousePosition;
      hasEndPosition = true;
    }

    if (Input.GetButtonUp("Fire2"))
    {
      // change projectile
      cycleSelectedProjectile();
    }

    if (hasStartPosition)
    {
      Vector2 projectileVector = startPosition - endPosition;
      Vector2 guideLineEnd = Vector2.MoveTowards(startPosition, mousePosition, maxPullForce);
      // draw guide line
      guideLineRenderer.SetPosition(0, startPosition);
      guideLineRenderer.SetPosition(1, guideLineEnd);
        
      if (fire1Up
          && hasEndPosition)
      {
        // calculate projectile values
        float sign = startPosition.y < endPosition.y ? -1.0f : 1.0f;
        float fireAngle = Vector2.Angle(projectileVector, Vector2.right) * sign;
        Vector2 pullVector = Vector2.ClampMagnitude(projectileVector, maxPullForce);
        Quaternion projectileRotation = Quaternion.AngleAxis(fireAngle, Vector3.forward);
        // override pull force when over maximum
        Debug.Log($"Start {startPosition} End {endPosition}" +
                  $" Direction {pullVector} Fire Angle {fireAngle}");
        if (projectileVector.magnitude > 0)
        {
          // create new projectile
          GameObject newProjectile = Instantiate(projectiles[selectedProjectileIndex],
                                                 startPosition,
                                                 projectileRotation);
          var newProjectileRigidbody = newProjectile.GetComponent<Projectile>();
          if (newProjectileRigidbody != null)
          {
            // fire projectile
            newProjectileRigidbody.Shoot(pullVector * forceMultiplier);
          }
        }
        // reset for new input
        hasStartPosition = hasEndPosition = false;
        guideLineRenderer.SetPositions(new []{Vector3.zero, Vector3.zero});
      }
    }
  }

  private void cycleSelectedProjectile()
  {
    if (selectedProjectileIndex < projectiles.Length - 1)
    {
      selectedProjectileIndex++;
    }
    else
    {
      selectedProjectileIndex = 0;
    }
  }
}