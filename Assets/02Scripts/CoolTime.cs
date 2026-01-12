using UnityEngine;

public class CoolTime
{
    public float coolTime;
    float coolCnt = 0;

    void Start()
    {
        coolCnt = Time.time;
    }

    public float Timer(float time)
    {
        coolTime += Time.deltaTime;
        if(coolCnt + time <= Time.time)
        {
            coolCnt = Time.time;
            coolTime = 0f;
        }
        return coolTime * 0.1f;
    }
}
