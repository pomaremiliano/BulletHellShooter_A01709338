using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int startHp;
    int hp;
    public float bulletCoolDown;
    float bulletTimer;
    void Start()
    {
        hp = startHp;
    }

    // Update is called once per frame
    void Update()
    {
        bulletTimer -= Time.deltaTime;
    }

    private void onTriggerEnter(Collider collision)
    {
        if(collision.tag == "Bullet" && bulletTimer <= 0)
        {
            hp--;
            if(hp <= 0)
            {
                Destroy(gameObject);
            }
            print("HP: " + hp);
            bulletTimer = bulletCoolDown;
        }
    }
}
