using UnityEngine;

public class Effect : MonoBehaviour
{
    public float life = 0.3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, life);
    }

   
}
