using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DartsElement : MonoBehaviour
{
    [SerializeField] Rigidbody body;
    [SerializeField] CapsuleCollider dartCollider;
    [SerializeField] Transform dartsTip;
    private Vector3 lauchDirection;
    [SerializeField] float lauchPower;

    public UnityEvent<Vector3> OnHit;

    private void OnTriggerEnter(Collider other)
    {
        body.constraints = RigidbodyConstraints.FreezeAll;
        body.velocity = Vector3.zero;
        OnHit.Invoke(dartsTip.position);

    }
    public void PlaceDarts(Vector3 position)
    {
        transform.position = position;
    }
    public void UpdateOrientation(Vector3 direction)
    {
        transform.rotation = Quaternion.LookRotation(direction);
        lauchDirection = direction.normalized;
    }
    public void OnLauch(float power)
    {
        body.constraints = RigidbodyConstraints.FreezeRotation;
        power *= lauchPower;
        body.AddForce(lauchDirection * power, ForceMode.Impulse);

    }
    void Start()
    {
        body.constraints = RigidbodyConstraints.FreezeAll;
        body.velocity = Vector3.zero;
        dartCollider.isTrigger = true;
    }

    void Update()
    {
        if (body.velocity.magnitude >= 0.05f)
        {
            transform.rotation = Quaternion.LookRotation(body.velocity.normalized);
        }
    }
}
