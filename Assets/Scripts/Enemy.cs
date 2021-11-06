using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Serialized Private Fields
    [SerializeField] private string targetTag = "Player";
    [SerializeField] private float downwardInclination = 1f;
    [SerializeField] private float speed = 1f;
    #endregion

    #region Private Fields
    private GameObject target;

    private Vector3 belowPosition; // Position below current position
    private float destroyAt = 0f; // Position to trigger self destruct

    // Set callbacks when instantiating this object
    private System.Action<bool> bottomCallback = (bool hitPlayer) => { };
    private System.Action deathCallback = () => { };
    #endregion

    #region MonoBehaviour Methods
    void Start()
    {
        target = GameObject.FindGameObjectWithTag(targetTag);
        belowPosition = transform.position;
    }

    void Update()
    {
        belowPosition.x = transform.position.x;
        belowPosition.y = transform.position.y - downwardInclination;
        belowPosition.z = transform.position.z;
        if (transform.position.y <= destroyAt)
        {
            bottomCallback(/*hitPlayer:*/ false);
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(belowPosition, target.transform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(targetTag))
        {
            bottomCallback(/*hitPlayer:*/ true);
            Destroy(this.gameObject);
        }
    }
    #endregion

    #region Enemy Methods
    public void SetDestroyAt(float destroyAt)
    {
        this.destroyAt = destroyAt;
    }

    public void SetBottomCallback(System.Action<bool> callback)
    {
        bottomCallback = callback;
    }

    public void SetDeathCallback(System.Action callback)
    {
        deathCallback = callback;
    }

    public void Die()
    {
        deathCallback();
        Destroy(gameObject);
    }
    #endregion
}
