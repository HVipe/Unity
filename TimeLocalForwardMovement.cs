using System.Collections;
using UnityEngine;

public class TimedLocalForwardMovement : MonoBehaviour
{
    public Transform player;
    public float detectionRadius = 5f;
    public float maxEscapeSpeed = 5f;
    public float minEscapeSpeed = 2f;
    public float escapeAcceleration = 5f;
    public float escapeDistanceThreshold = 1f;

    public float minMoveDistance = 1f;
    public float maxMoveDistance = 3f;
    public float minMoveTime = 2f;
    public float maxMoveTime = 4f;
    public float moveSpeed = 2f;
    public float turnSpeed = 2f;
    public float turnInterval = 30f;
    public float rayDistance = 2f;
    public LayerMask obstacleLayer;

    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isEscaping = false;
    private float initialYPosition;
    private Animator animator;

    void Start()
    {
        targetPosition = transform.position;
        initialYPosition = transform.position.y;
        animator = GetComponent<Animator>();

        StartCoroutine(RandomMovement());
        StartCoroutine(TurnAfterDelay());
    }

    void Update()
    {
        DetectPlayer();

        if (isMoving || isEscaping)
        {
            if (isEscaping)
            {
                animator.speed = Mathf.Lerp(1, maxEscapeSpeed / moveSpeed, 0.5f);
            }
            else
            {
                animator.speed = 1;
            }
        }
        else
        {
            animator.speed = 0;
        }

        if (isEscaping)
        {
            EscapeFromPlayer();
        }
        else if (isMoving)
        {
            MoveTowardsTarget();
        }

        // Add obstacle detection
        Vector3 direction = (targetPosition - transform.position).normalized;
        Debug.DrawRay(transform.position + Vector3.up * 0.1f, direction * rayDistance, Color.red);  // Visualize Raycast
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, direction, rayDistance, obstacleLayer))
        {
            isMoving = false;  // Stop moving if an obstacle is detected
        }
    }

    IEnumerator RandomMovement()
    {
        while (true)
        {
            if (!isEscaping && !isMoving)
            {
                Vector3 randomDirection = GetValidRandomDirection();
                float randomMoveDistance = Random.Range(minMoveDistance, maxMoveDistance);
                targetPosition = transform.position + randomDirection * randomMoveDistance;
                targetPosition.y = initialYPosition;

                isMoving = true;
                SmoothRotate(randomDirection);

                yield return new WaitUntil(() => !isMoving);
                float waitTime = Random.Range(minMoveTime, maxMoveTime);
                yield return new WaitForSeconds(waitTime);
            }
            yield return null;
        }
    }

    void MoveTowardsTarget()
    {
        float distance = Vector3.Distance(transform.position, targetPosition);

        if (distance < 0.1f)
        {
            isMoving = false;
            transform.position = targetPosition;
            return;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;

        if (!Physics.Raycast(transform.position + Vector3.up * 0.1f, direction, rayDistance, obstacleLayer))
        {
            Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;
            newPosition.y = initialYPosition;
            transform.position = newPosition;
            SmoothRotate(direction);
        }
        else
        {
            isMoving = false;
        }
    }

    void SmoothRotate(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    IEnumerator TurnAfterDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(turnInterval);
            if (!isEscaping)
            {
                Vector3 turnDirection = -transform.forward;
                SmoothRotate(turnDirection);
            }
        }
    }

    void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRadius && !isEscaping)
        {
            isEscaping = true;
        }
    }

    void EscapeFromPlayer()
    {
        isMoving = true;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        Vector3 directionAwayFromPlayer = (transform.position - player.position).normalized;

        Debug.DrawRay(transform.position + Vector3.up * 0.1f, directionAwayFromPlayer * rayDistance, Color.red);  // Visualize Raycast
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, directionAwayFromPlayer, rayDistance, obstacleLayer))
        {
            if (!TryMoveInDirection(Quaternion.Euler(0, 45, 0) * directionAwayFromPlayer) &&
                !TryMoveInDirection(Quaternion.Euler(0, -45, 0) * directionAwayFromPlayer))
            {
                Vector3 bestEscapeDirection = GetBestEscapeDirection();
                SmoothRotate(bestEscapeDirection);
                Vector3 escapePosition = transform.position + bestEscapeDirection * maxEscapeSpeed * Time.deltaTime;
                escapePosition.y = initialYPosition;
                transform.position = escapePosition;
            }
        }
        else
        {
            Vector3 escapePosition = transform.position + directionAwayFromPlayer * maxEscapeSpeed * Time.deltaTime;
            escapePosition.y = initialYPosition;
            transform.position = escapePosition;
            SmoothRotate(directionAwayFromPlayer);
        }

        if (distanceToPlayer > detectionRadius)
        {
            isEscaping = false;
            isMoving = false;
        }
    }

    bool TryMoveInDirection(Vector3 direction)
    {
        if (!Physics.Raycast(transform.position + Vector3.up * 0.1f, direction, rayDistance, obstacleLayer))
        {
            Vector3 newPosition = transform.position + direction * maxEscapeSpeed * Time.deltaTime;
            newPosition.y = initialYPosition;
            transform.position = newPosition;
            SmoothRotate(direction);
            return true;
        }
        return false;
    }

    Vector3 GetBestEscapeDirection()
    {
        Vector3 bestDirection = transform.forward;
        float maxDistance = 0;

        for (int i = -90; i <= 90; i += 15)
        {
            Vector3 testDirection = Quaternion.Euler(0, i, 0) * transform.forward;
            float distanceToPlayer = Vector3.Distance(transform.position + testDirection, player.position);
            if (distanceToPlayer > maxDistance && !Physics.Raycast(transform.position + Vector3.up * 0.1f, testDirection, rayDistance, obstacleLayer))
            {
                maxDistance = distanceToPlayer;
                bestDirection = testDirection;
            }
        }

        return bestDirection;
    }

    Vector3 GetValidRandomDirection()
    {
        Vector3 randomDirection = transform.forward;

        for (int i = 0; i < 10; i++)
        {
            randomDirection = Random.insideUnitSphere.normalized;
            randomDirection.y = 0;

            if (!Physics.Raycast(transform.position + Vector3.up * 0.1f, randomDirection, rayDistance, obstacleLayer))
            {
                break;
            }
        }

        return randomDirection;
    }
}

