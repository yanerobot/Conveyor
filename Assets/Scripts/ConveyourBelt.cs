using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ConveyourBelt : MonoBehaviour
{
    InputMaster input;
    Transform arrow;
    Renderer[] renderers;
    Outline outline;
    BoxCollider coll;


    void Start()
    {
        input = new InputMaster();
        input.Enable();

        arrow = transform.Find("Arrow");
        renderers = GetComponentsInChildren<MeshRenderer>();
        coll = GetComponent<BoxCollider>();

        GridObject.grid.SetValue(1, new Vector3(transform.position.x, 0, transform.position.z));
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out MovingPart movingPart))
        {
            movingPart.StartCoroutine(movingPart.MoveRb(transform.position + transform.right, transform.localEulerAngles));
        }
    }


    public void DragState()
    {
        coll.enabled = false;
        arrow.gameObject.SetActive(true);
        if (outline == null) outline = gameObject.AddComponent<Outline>();
        outline.OutlineWidth = 5f;
        outline.OutlineColor = Color.yellow;
        SetAlpha(.6f);
    }

    public void StaticState()
    {
        coll.enabled = true;
        arrow.gameObject.SetActive(false);
        Destroy(outline);
        SetAlpha(1);
    }

    void SetAlpha(float alpha)
    {
        foreach (var rend in renderers)
        {
            Color color = rend.material.color;
            color.a = alpha;
            rend.material.color = color;
        }
    }

    public void WrongSlotState()
    {
        outline.OutlineColor = Color.red;
    }
}
