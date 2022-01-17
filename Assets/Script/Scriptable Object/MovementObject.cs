using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementObject : MonoBehaviour
{
    private Vector3 PosA;
    private Vector3 PosB;
    private Vector3 nextPos;

    [SerializeField] private float movementSpeed;

    [SerializeField] private Transform childTransform;

    [SerializeField] private Transform transformB;

    // Start is called before the first frame update
    void Start()
    {
        PosA = childTransform.localPosition;
        PosB = transformB.localPosition;
        nextPos = PosB;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        childTransform.localPosition = Vector3.MoveTowards(childTransform.localPosition, nextPos, movementSpeed * Time.deltaTime);
        if (Vector3.Distance(childTransform.localPosition, nextPos) <= 0.1)
        {
            AnotherMove();
        }
    }

    private void AnotherMove()
    {
        nextPos = nextPos != PosA ? PosA : PosB;
    }
}
