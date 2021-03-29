using Pokemon;
using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    CharacterAnimator animator;
    public float moveSpeed;
    public bool IsMoving { get; private set; }

    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    public IEnumerator Move(Vector2 moveVec, Action OnMoveOver = null)
    {
        animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

        var targetPos = transform.position;
        targetPos.x += moveVec.x;
        targetPos.y += moveVec.y;

        if (!IsPathClear(targetPos))
        {
            var isPlayer = GetComponent<PlayerMovement>();
            if (isPlayer != null)
                GameController.soundFX = "Collision";

            yield break;
        }

        IsMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        IsMoving = false;

        OnMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.IsMoving = IsMoving;
    }

    private bool IsPathClear(Vector3 targetPosition)
    {
        var difference = targetPosition - transform.position;
        var direction = difference.normalized;

        if (Physics2D.BoxCast(transform.position + direction, new Vector2(0.2f, 0.2f), 0f, direction, difference.magnitude - 1, GameplayLayers.i.SolidLayer | GameplayLayers.i.InteractLayer | GameplayLayers.i.PlayerLayer) == true)
            return false;
        else
            return true;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.125f, GameplayLayers.i.SolidLayer | GameplayLayers.i.InteractLayer) != null)
        {
            return false;
        }
        return true;
    }

    public void LookTowards(Vector3 targetPosition)
    {
        var xDifference = Mathf.Floor(targetPosition.x) - Mathf.Floor(transform.position.x);
        var yDifference = Mathf.Floor(targetPosition.y) - Mathf.Floor(transform.position.y);

        if (xDifference == 0 || yDifference == 0)
        {
            animator.MoveX = Mathf.Clamp(xDifference, -1f, 1f);
            animator.MoveY = Mathf.Clamp(yDifference, -1f, 1f);
        }
    }

    public CharacterAnimator Animator
    {
        get => animator;
    }
}
