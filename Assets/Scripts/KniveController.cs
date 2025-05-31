using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KniveController : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_rb.bodyType == RigidbodyType2D.Static)
            return;

        if (collision.collider.CompareTag("Trunk"))
        {
            transform.parent = collision.transform;
            _rb.bodyType = RigidbodyType2D.Static;


            GameManager.Instance.AddScore();
        }
        else if (collision.collider.CompareTag("Knive"))
        {
            OutKnive();

            GameManager.Instance.Invoke("RestartGame", 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("apple"))
        {
            GameManager.Instance.AddAppel();
            Destroy(collision.gameObject);
        }
    }

    public void OutKnive()
    {
        _rb.bodyType = RigidbodyType2D.Dynamic;
        if (transform.parent)
        {
            _rb.angularVelocity = Random.Range(-3, 3) * 50;
            _rb.velocity = ((Vector2)transform.up + new Vector2(Random.Range(-2.5f, 2.5f), -1)) * 25;
        }
        else
        {
            _rb.angularVelocity = Random.Range(-3, 3) * 50;
            _rb.velocity = new Vector2(Random.Range(-2.5f, 2.5f), -1) * 25;
        }
    }
}
