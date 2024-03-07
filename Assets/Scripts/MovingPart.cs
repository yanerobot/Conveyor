using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovingPart : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;

    GameObject GFX;
    Rigidbody rb;
    int layer;
    bool canMove = true;
    bool changeGFX;
    GameObject newGFX;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GFX = GetComponentInChildren<Renderer>().gameObject;
        layer = (int)Mathf.Log(groundLayer.value, 2);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == layer)
        {
            LastBreath();
        }
    }

    void LastBreath()
    {
        rb.velocity = Vector3.zero;
        StopAllCoroutines();
        Destroy(gameObject, 0.5f);
    }

    public void ChangeGFX(GameObject newGFX)
    {
        changeGFX = true;
        this.newGFX = newGFX;
    }

    public IEnumerator MoveRb(Vector3 destination, Vector3 rotation)
    {
        yield return new WaitUntil(() => canMove);

        canMove = false;

        transform.localEulerAngles = rotation;
        destination.y = rb.position.y;
        var direction = (destination - transform.position).normalized;
        rb.velocity = direction;

        while (Vector3.Dot((destination - transform.position).normalized, direction) > 0.5f)
        {
            if (rb.velocity == Vector3.zero)
            {
                LastBreath();
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }

        if (changeGFX)
        {
            Destroy(GFX);
            GFX = Instantiate(newGFX, transform);
        }

        canMove = true;
    }
}