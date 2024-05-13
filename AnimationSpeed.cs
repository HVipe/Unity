using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSpeedController : MonoBehaviour
{
    public Animator animator; // 动画组件
    private float lastKeyPressTime; // 上一次按键时间
    private float keyPressInterval = 1.0f; // 两次按键间隔，默认为1秒

    void Update()
    {
        // 检测特定按键（例如：R键）
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 计算两次按键的时间间隔
            float currentTime = Time.time;
            if (lastKeyPressTime > 0)
            {
                keyPressInterval = currentTime - lastKeyPressTime;
            }
            lastKeyPressTime = currentTime;

            // 根据按键速度调整动画速度
            AdjustAnimationSpeed();
        }
    }

    void AdjustAnimationSpeed()
    {
        // 计算动画速度（你可以根据需要调整这里的计算方法）
        // 这里的例子是使得按键间隔越短，动画播放越快
        float speed = 1.0f / keyPressInterval;

        // 限制动画速度的范围，防止过快或过慢
        speed = Mathf.Clamp(speed, 0.5f, 3.0f);

        // 设置动画播放速度
        animator.speed = speed;
    }
}
