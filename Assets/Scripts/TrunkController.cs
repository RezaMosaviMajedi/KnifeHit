using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TrunkController : MonoBehaviour
{
    SpriteRenderer sprite;
    Animator animator;

    DataLevel currntLevel;
    float timer = 0;
    float maxDirection = 360;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void SetCurrntLevel(DataLevel dataLevel)
    {
        currntLevel = dataLevel;
        animator.Play("normal");
        transform.localScale = Vector3.one;
        if (currntLevel.apple_count > 0 || currntLevel.knive_count > 0)
        {
            List<int> points = new List<int>();
            for (int i = 0; i < 18; i++)
            {
                points.Add(i);
            }
            if (currntLevel.apple_count > 0)
            {
                for (int i = 0; i < currntLevel.apple_count; i++)
                {
                    int point = Random.Range(0, points.Count);

                    GameObject _apple = Instantiate(GameManager.Instance.applePrefab, transform);

                    _apple.transform.rotation = Quaternion.Euler(0, 0, points[point] * 20);
                    _apple.transform.position = transform.position + (_apple.transform.up * ((sprite.size.x / 2) + 0.25f));

                    points.Remove(point);
                }
            }

            if (currntLevel.knive_count > 0)
            {
                for (int i = 0; i < currntLevel.knive_count; i++)
                {
                    int point = Random.Range(0, points.Count);

                    GameObject knive = Instantiate(GameManager.Instance.knivePrefab, transform);
                    knive.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

                    knive.transform.rotation = Quaternion.Euler(0, 0, points[point] * 20);
                    knive.transform.position = transform.position + (knive.transform.up * ((sprite.size.x / 2) + 0.25f));
                    knive.transform.Rotate(0, 0, 180);

                    points.Remove(point);
                }
            }
        }

        StartCoroutine(RunTrunk());
    }

    IEnumerator RunTrunk()
    {
        timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / currntLevel.durationTime);

            float curveValue = currntLevel.rotationCurve.Evaluate(t);
            float angle = curveValue * maxDirection * currntLevel.maxRotation;

            transform.rotation = Quaternion.Euler(0, 0, angle);

            if (timer >= currntLevel.durationTime)
            {
                timer = 0f;
                if (currntLevel.pingpong)
                    maxDirection *= -1;
            }

            yield return new Update();

        }
    }

    public void DestroyTrunk()
    {
        StopAllCoroutines();

        int childCount = transform.childCount;
        int j = 0;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(j);
            if (child.gameObject.GetComponent<KniveController>())
            {
                child.gameObject.GetComponent<Collider2D>().isTrigger = true;
                child.gameObject.GetComponent<KniveController>().OutKnive();
                child.transform.parent = null;
            }
            else
            {
                j++;
                Destroy(child.gameObject);
            }
        }

        animator.SetTrigger("Destroy");
    }
}
