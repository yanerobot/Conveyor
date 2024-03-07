using System.Collections;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    [SerializeField, Range(5, 15)] float cameraPositionThreshhold = 11f;


    InputMaster input;
    ConveyourBelt belt;
    ConveyourBelt checkBelt;
    Camera camMain;
    bool isDragging;
    bool isMovingCamera;
    float y;

    void Awake()
    {
        camMain = Camera.main;
        input = new InputMaster();
        input.Enable();

        input.Pointer.DragStartPos.performed += ctx => GetObjectUnderPointer(ctx.ReadValue<Vector2>());
        input.Pointer.EndDrag.performed += ctx => PutObject();
    }

    void CheckObjectUnderPointer(Vector2 screenPos)
    {
        var ray = camMain.ScreenPointToRay(screenPos);

        Physics.Raycast(ray, out var hit);

        if (hit.collider == null || !hit.collider.TryGetComponent(out belt))
        {
            isMovingCamera = true;
            StartCoroutine(MoveCamera());
            return;
        }
    }

    void GetObjectUnderPointer(Vector2 screenpos)
    {
        isMovingCamera = false; 
        var ray = camMain.ScreenPointToRay(screenpos);

        Physics.Raycast(ray, out var hit);

        if (hit.collider == null || !hit.collider.TryGetComponent(out checkBelt))
        {
            isMovingCamera = true;
            StartCoroutine(MoveCamera());
            return;
        }
        if (isDragging) return;

        isDragging = true;

        belt = checkBelt;

        GridObject.grid.SetValue(0, new Vector3(belt.transform.position.x, 0, belt.transform.position.z));

        belt.DragState();

        StartCoroutine(MoveBelt());
    }

    void PutObject()
    {
        if (isMovingCamera)
        {
            isMovingCamera = false;
            return;
        }
        y = 0.5f;
        if (belt == null) return;

        var pos = belt.transform.position.WhereY(0);

        if (GridObject.grid.GetValue(pos) != 0)
        {
            belt.WrongSlotState();
            return;
        }
        isDragging = false;
        y = 0f;

        GridObject.grid.SetValue(1, pos);

        belt.transform.position = pos;

        belt.StaticState();
    }

    IEnumerator MoveBelt()
    {
        if (belt == null) yield break;

        while (isDragging)
        {
            var ray = camMain.ScreenPointToRay(input.Pointer.DragPos.ReadValue<Vector2>());
            Physics.Raycast(ray, out var hit, 1000f, groundLayer);

            belt.transform.position = hit.point.WhereY(0.5f);
            yield return new WaitForFixedUpdate();
        }

        var pos = belt.transform.position;
        pos = pos.WhereX(Mathf.Round(pos.x)).WhereZ(Mathf.Round(pos.z)).WhereY(y);
        belt.transform.position = pos;
    }

    IEnumerator MoveCamera()
    {
        if (camMain == null) yield break;

        while (isMovingCamera)
        {
            var delta = input.Pointer.DragDelta.ReadValue<Vector2>().y;
            var worldDelta = -delta / 200;
            if (Mathf.Abs(camMain.transform.position.x + worldDelta) < cameraPositionThreshhold)
            {
                camMain.transform.position = camMain.transform.position.AddTo(x: worldDelta);
            }
            yield return null;
        }
    }

    public void SpeedUp()
    {
        Time.timeScale = 8;
    }
    public void OriginalSpeed()
    {
        Time.timeScale = 1;
    }
}
