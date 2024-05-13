using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLocalForwardMovement : MonoBehaviour
{
    public float minMoveInterval = 2f;  // 最小移动间隔
    public float maxMoveInterval = 4f;  // 最大移动间隔
    public float turnInterval = 30f;  // 转身间隔
    public float moveDistance = 1f;  // 每次移动的距离
    public float lerpTime = 0.5f;    // 插值所需的时间
    private Vector3 targetPosition;  // 目标位置
    private float timeSinceMoved;    // 从上次移动后经过的时间

    void Start()
    {
        targetPosition = transform.position;
        float initialDelay = Random.Range(minMoveInterval, maxMoveInterval);
        Invoke("SetTargetPosition", initialDelay);  // 使用随机延迟开始第一次移动
        StartCoroutine(TurnAfterDelay());
    }

    void Update()
    {
        // 插值移动到目标位置
        if (timeSinceMoved < lerpTime)
        {
            timeSinceMoved += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, timeSinceMoved / lerpTime);
        }
    }

    void SetTargetPosition()
    {
        targetPosition = transform.position + transform.forward * moveDistance;
        timeSinceMoved = 0;  // 重置插值计时器
        float nextMoveDelay = Random.Range(minMoveInterval, maxMoveInterval);
        Invoke("SetTargetPosition", nextMoveDelay);  // 设置下一次移动的随机延迟
    }

    IEnumerator TurnAfterDelay()
    {
        yield return new WaitForSeconds(turnInterval);
        transform.Rotate(0f, 180f, 0f);
        StartCoroutine(TurnAfterDelay());
    }
}

