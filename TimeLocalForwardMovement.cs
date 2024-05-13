using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLocalForwardMovement : MonoBehaviour
{
    public float minMoveInterval = 2f;  // ��С�ƶ����
    public float maxMoveInterval = 4f;  // ����ƶ����
    public float turnInterval = 30f;  // ת����
    public float moveDistance = 1f;  // ÿ���ƶ��ľ���
    public float lerpTime = 0.5f;    // ��ֵ�����ʱ��
    private Vector3 targetPosition;  // Ŀ��λ��
    private float timeSinceMoved;    // ���ϴ��ƶ��󾭹���ʱ��

    void Start()
    {
        targetPosition = transform.position;
        float initialDelay = Random.Range(minMoveInterval, maxMoveInterval);
        Invoke("SetTargetPosition", initialDelay);  // ʹ������ӳٿ�ʼ��һ���ƶ�
        StartCoroutine(TurnAfterDelay());
    }

    void Update()
    {
        // ��ֵ�ƶ���Ŀ��λ��
        if (timeSinceMoved < lerpTime)
        {
            timeSinceMoved += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPosition, timeSinceMoved / lerpTime);
        }
    }

    void SetTargetPosition()
    {
        targetPosition = transform.position + transform.forward * moveDistance;
        timeSinceMoved = 0;  // ���ò�ֵ��ʱ��
        float nextMoveDelay = Random.Range(minMoveInterval, maxMoveInterval);
        Invoke("SetTargetPosition", nextMoveDelay);  // ������һ���ƶ�������ӳ�
    }

    IEnumerator TurnAfterDelay()
    {
        yield return new WaitForSeconds(turnInterval);
        transform.Rotate(0f, 180f, 0f);
        StartCoroutine(TurnAfterDelay());
    }
}

