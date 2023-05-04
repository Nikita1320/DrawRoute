using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Character : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private float timeMove;
    [SerializeField] private bool routeIsDrawed = false;
    [SerializeField] private LineDrawer lineDrawer;
    [SerializeField] private CharacterTarget[] possibleTargets;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LayerMask targetLayer;
    private bool isCollisionWithTarget = false;

    private Vector3 startPosition;
    private bool flipX;

    private Coroutine movingCororutine;
    public Action routeDrawed;
    public Action achieveGoal;
    public Action collideObstacle;
    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        flipX = spriteRenderer.flipX;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        foreach (var target in possibleTargets)
        {
            if (target.IsAccess == true)
            {
                target.droped += StopDrawing;
            }
        }

        if (routeIsDrawed == false)
        {
            lineDrawer.StartDrawing();
        }
    }
    public void StopDrawing()
    {
        routeIsDrawed = true;
        routeDrawed?.Invoke();
        lineDrawer.StopDrawing();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (routeIsDrawed == false)
        {
            lineDrawer.StopDrawing();
            lineDrawer.ClearLine();
        }

        foreach (var target in possibleTargets)
        {
            if (target.IsAccess == true)
            {
                target.droped -= StopDrawing;
            }
        }
    }
    public void Move()
    {
        if (lineDrawer.Points.Count > 0)
        {
            movingCororutine = StartCoroutine(MovingCororutine());
        }
    }
    private IEnumerator MovingCororutine()
    {
        animator.SetBool("IsMoving", true);
        int pointCounter = 0;
        float generalLength = lineDrawer.LengthLine + Vector2.Distance(transform.position, lineDrawer.Points[pointCounter]);
        float timeTransitionBetweenPoint = Vector2.Distance(transform.position, lineDrawer.Points[pointCounter]) / generalLength * timeMove;
        SetFlip(lineDrawer.Points[pointCounter]);

        float fraction = 0;
        float remainingTime = 0;
        var startPosition = transform.position;

        while (true)
        {
            yield return new WaitForFixedUpdate();

            remainingTime += Time.fixedDeltaTime;
            fraction = remainingTime / timeTransitionBetweenPoint;
            if (fraction > 1)
            {
                fraction = 1;
            }
            transform.position = Vector2.Lerp(startPosition, lineDrawer.Points[pointCounter], fraction);

            
            if ((Vector2)transform.position == lineDrawer.Points[pointCounter])
            {
                pointCounter++;

                if (pointCounter == lineDrawer.Points.Count - 1)
                {
                    EndMoving();
                    break;
                }
                startPosition = transform.position;
                remainingTime = 0;
                timeTransitionBetweenPoint = Vector2.Distance(transform.position, lineDrawer.Points[pointCounter]) / generalLength * timeMove;
                SetFlip(lineDrawer.Points[pointCounter]);
            }
        }
    }
    private void EndMoving()
    {
        if (isCollisionWithTarget)
        {
            achieveGoal?.Invoke();
        }
        StopMoving();
    }
    private void SetFlip(Vector2 direction)
    {
        if (transform.position.x < direction.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    public void StopMoving()
    {
        animator.SetBool("IsMoving", false);
        if (movingCororutine != null)
        {
            StopCoroutine(movingCororutine);
        }
    }
    private void Die()
    {
        animator.SetTrigger("Die");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (targetLayer == (targetLayer | (1 << collision.gameObject.layer)))
        {
            isCollisionWithTarget = true;
        }
        if (obstacleLayer == (obstacleLayer | (1 << collision.gameObject.layer)))
        {
            Die();
            collideObstacle?.Invoke();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (obstacleLayer == (obstacleLayer | (1 << collision.gameObject.layer)))
        {
            collideObstacle?.Invoke();
        }
    }
    public void Reset()
    {
        StopMoving();
        animator.SetTrigger("Reset");
        transform.position = startPosition;
        spriteRenderer.flipX = flipX;
        routeIsDrawed = false;
        isCollisionWithTarget = false;
        lineDrawer.Reset();
        foreach (var possibleTarget in possibleTargets)
        {
            possibleTarget.Reset();
        }
    }
}
