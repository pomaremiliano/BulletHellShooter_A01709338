using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed;
    public float bulletRotation;

    public Vector3 bulletVelocity;

    public float minRotation;
    public float maxRotation;

    public float bulletCoolDown;

    public int numBullets;
    float bulletTimer;
    float[] rotations;

    public float spiralSpeed;

    float currentSpiral;

    float currentPattern;

    public TextMeshProUGUI bulletCount;
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI patternText;

    void timerUpdate(float time)
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + time;
        }
    }

    void PatternUpdate(int pattern)
    {
        if (patternText != null)
        {
            patternText.text = "Pattern: " + pattern;
        }
    }


    void Start()
    {
        bulletTimer = bulletCoolDown;
        rotations = new float[numBullets];
        currentSpiral = minRotation;
        currentPattern = 0;
        timerUpdate(0);

        StartCoroutine(ChangePattern());
        StartCoroutine(CountBullets());
    }

    void Update()
    {
        if (bulletTimer <= 0)
        {
            SpawnBullets();
            bulletTimer = bulletCoolDown;
            bulletTimer += Time.deltaTime;
            timerUpdate(bulletTimer);
        }
        else
        {
            bulletTimer -= Time.deltaTime;
            timerUpdate(bulletTimer);
        }

    }

    IEnumerator ChangePattern()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            currentPattern++;
            PatternUpdate((int)currentPattern);
            if (currentPattern > 3)
            {
                currentPattern = 0;
            }
        }
    }

    public float[] RandomPattern()
    {
        float[] randomRotations = new float[numBullets];
        for (int i = 0; i < numBullets; i++)
        {
            randomRotations[i] = Random.Range(minRotation, maxRotation);
        }
        return randomRotations;
    }

    public float[] SpiralPattern()
    {
        for (int i = 0; i < numBullets; i++)
        {
            rotations[i] = currentSpiral + (i * spiralSpeed);
            rotations[i] = Mathf.Repeat(rotations[i], 360.0f);
        }
        currentSpiral += spiralSpeed;
        return rotations;
    }

    public float[] ConvergePattern()
    {
        float angle = 360.0f;
        for (int i = 0; i < numBullets; i++)
        {
            rotations[i] = minRotation + (angle * i);
        }
        return rotations;
    }

    public float[] CirclePattern()
    {
        float angle = 360.0f / numBullets;
        for (int i = 0; i < numBullets; i++)
        {
            rotations[i] = angle * i;
        }
        return rotations;
    }

    public float[] Patterns()
    {
        switch (currentPattern)
        {
            case 0:
                return CirclePattern();
            case 1:
                return SpiralPattern();
            case 2:
                return ConvergePattern();
            case 3:
                return RandomPattern();
            default:
                return CirclePattern();
        }
    }

    void bulletCountUpdate(int countBullets)
    {
        if (bulletCount != null)
        {
            bulletCount.text = "Bullets: " + countBullets;
        }
    }

    IEnumerator CountBullets()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            bulletCountUpdate(BulletList.bullets.Count);
        }
    }


    public GameObject[] SpawnBullets()
    {
        rotations = Patterns();

        GameObject[] bullets = new GameObject[numBullets];
        //GameObject[] BulletList = new GameObject[numBullets];
        for (int i = 0; i < numBullets; i++)
        {
            bullets[i] = BulletList.getBulletList();
            if (bullets[i] == null)
            {
                bullets[i] = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullets[i].transform.localPosition = Vector3.zero;
            }

            var b = bullets[i].GetComponent<Bullet>();
            b.rotation = rotations[i];
            b.speed = bulletSpeed;
            b.velocity = bulletVelocity;
        }
        bulletCountUpdate(numBullets);
        return bullets;


    }

}
