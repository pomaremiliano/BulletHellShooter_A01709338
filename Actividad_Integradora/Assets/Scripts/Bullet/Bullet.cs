using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 velocity;
    public float speed;
    public float rotation;

    public float lifeTime;

    float timer;


    void Start()
    {
        timer = lifeTime;
        transform.rotation = Quaternion.Euler(1, 1, rotation);        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity*speed*Time.deltaTime);
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnEnable()
    {
        timer = lifeTime;
    }
}
