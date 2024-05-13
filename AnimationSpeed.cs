using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    public Animator animator; // �������
    private float lastKeyPressTime; // ��һ�ΰ���ʱ��
    private float keyPressInterval = 1.0f; // ���ΰ��������Ĭ��Ϊ1��

    void Update()
    {
        // ����ض����������磺R����
        if (Input.GetKeyDown(KeyCode.R))
        {
            // �������ΰ�����ʱ����
            float currentTime = Time.time;
            if (lastKeyPressTime > 0)
            {
                keyPressInterval = currentTime - lastKeyPressTime;
            }
            lastKeyPressTime = currentTime;

            // ���ݰ����ٶȵ��������ٶ�
            AdjustAnimationSpeed();
        }
    }

    void AdjustAnimationSpeed()
    {
        // ���㶯���ٶȣ�����Ը�����Ҫ��������ļ��㷽����
        // �����������ʹ�ð������Խ�̣���������Խ��
        float speed = 1.0f / keyPressInterval;

        // ���ƶ����ٶȵķ�Χ����ֹ��������
        speed = Mathf.Clamp(speed, 0.5f, 3.0f);

        // ���ö��������ٶ�
        animator.speed = speed;
    }
}
