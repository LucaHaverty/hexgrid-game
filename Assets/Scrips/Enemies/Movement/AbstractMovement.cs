using System;
using UnityEngine;

public abstract class AbstractMovement : MonoBehaviour
{
    public float speed;
    private bool paused;
    protected Vector2 targetPos;

    [HideInInspector] public Vector2 velocity = Vector2.zero;
    private Vector2 prevPosition;

    protected virtual void Start()
    {
        prevPosition = transform.position;
    }

    protected virtual void FixedUpdate()
    {
        if (!paused)
            RunMovement();

        CalculateVelocity();
    }

    protected void Update() { }

    private void CalculateVelocity()
    {
        velocity = ((Vector2)transform.position - prevPosition).normalized * speed;
        prevPosition = transform.position;
    }

    protected abstract void RunMovement();

    public void SetTargetPos(Vector2 newTargetPos) => this.targetPos = newTargetPos;
    public void PauseMovement() => paused = true;
    public void ResumeMovement() => paused = false;

    public float DistanceFromTarget()
    {
        return Vector2.Distance(transform.position, targetPos);
    }

    protected virtual void UpdatePosition(Vector2 position) => transform.position = position;
}
