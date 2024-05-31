using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletList : MonoBehaviour
{
    public static List<GameObject> bullets;

    void Start()
    {
        bullets = new List<GameObject>();
    }

    public static GameObject getBulletList()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].GetComponent<Bullet>().OnEnable();
                bullets[i].SetActive(true);
                return bullets[i];
            }
        }
        return null;
    }
}
