using System.Collections;
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

    void TimerUpdate(float time)
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

    IEnumerator StartTimer()
    {
        float time = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            time++;
            TimerUpdate(time);
        }
    }

    void Start()
    {
        bulletTimer = bulletCoolDown;
        rotations = new float[numBullets];
        currentSpiral = minRotation;

        StartCoroutine(ChangePattern());
        StartCoroutine(CountBullets());
        StartCoroutine(StartTimer());
    }
    void Update()
    {
        if (bulletTimer <= 0)
        {
            SpawnBullets();
            bulletTimer = bulletCoolDown;
            bulletTimer += Time.deltaTime;
        }
        else
        {
            bulletTimer -= Time.deltaTime;

        }

    }

    IEnumerator ChangePattern()
    {
        currentPattern = 1;
        while (true)
        {
            PatternUpdate((int)currentPattern);
            yield return new WaitForSeconds(10f);
            currentPattern++;
            PatternUpdate((int)currentPattern);
            if (currentPattern >= 5)
            {
                currentPattern = 1;
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
            case 1:
                return CirclePattern();
            case 2:
                return SpiralPattern();
            case 3:
                return ConvergePattern();
            case 4:
                return RandomPattern();
            default:
                return CirclePattern();
        }
    }

    void BulletCountUpdate(int countBullets)
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
            BulletCountUpdate(BulletList.bullets.Count);
        }
    }


    public GameObject[] SpawnBullets()
    {
        rotations = Patterns();

        GameObject[] bullets = new GameObject[numBullets];
        for (int i = 0; i < numBullets; i++)
        {
            bullets[i] = BulletList.GetBulletList();
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
        BulletCountUpdate(numBullets);
        return bullets;


    }

}
