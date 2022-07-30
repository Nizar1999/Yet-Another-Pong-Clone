using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] float speed = 10;
    private Rigidbody2D body;

    private void Awake()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    public void InitBall()
    {
        transform.position = Vector3.zero;
        body.velocity = Vector2.zero;
        body.velocity = Vector2.right * speed;
    }

    public void ChangeDirection(Vector2 newDir)
    {
        Vector2 dir = newDir.normalized;
        GetComponent<Rigidbody2D>().velocity = dir * speed;
    }
    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.Play)
        {
            gameObject.SetActive(true);
            InitBall();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
