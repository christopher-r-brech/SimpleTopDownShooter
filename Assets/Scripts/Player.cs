using UnityEngine;

public class Player : MonoBehaviour
{
    #region Serialized Private Fields
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject projectile;

    [SerializeField] private float speed;
    [SerializeField] private string fireButton;
    [SerializeField] private string horizontalAxis;
    [SerializeField] private string verticalAxis;
    #endregion

    #region Private Fields
    private Vector3 currentMovement = Vector3.zero;
    private bool isFiring = false;
    private bool releasedFire = true;
    private bool canFire = true;
    private float horizontal = 0f;
    private float vertical = 0f;
    #endregion

    #region MonoBehaviour Methods
    void Start()
    {
        if (character == null) character = gameObject;
        if (projectile == null) Debug.LogError("No Projectile is set! Can not fire.", this);
    }

    void Update()
    {
        horizontal = Input.GetAxis(horizontalAxis);
        vertical = Input.GetAxis(verticalAxis);
        isFiring = Input.GetButtonDown(fireButton) || Input.GetButton(fireButton);
        releasedFire = Input.GetButtonUp(fireButton) || !Input.GetButton(fireButton);

        currentMovement.x = 0f;
        currentMovement.y = vertical;
        currentMovement.z = horizontal;
        currentMovement *= speed * Time.deltaTime;
    }

    void FixedUpdate()
    {
        Move(ref currentMovement, character);
        if (projectile != null && CanFire(isFiring, releasedFire, ref canFire))
        {
            Fire(projectile, character.transform.position);
        }
        
    }
    #endregion

    #region Player Methods
    private bool CanFire(bool isFiring, bool releasedFire, ref bool canFire)
    {
        // Overly thurough method of making sure we don't miss inputs but also can't fire more than once per mouse click.
        if (isFiring && canFire)
        {
            canFire = false;
            return true;
        }
        else if (releasedFire && !canFire)
        {
            canFire = true;
        }
        return false;
    }

    private void Fire(GameObject projectile, Vector3 start)
    {
        GameObject shot = Instantiate(projectile);
        shot.transform.position = start;
    }

    private void Move(ref Vector3 movement, GameObject character)
    {
        character.transform.Translate(movement);
    }
    #endregion
}
