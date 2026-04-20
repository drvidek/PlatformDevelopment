using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IReset, IStop
{
    private Rigidbody2D rb;
    private ScoreKeeper scoreKeeper;
    private UIManager uiManager;

    [SerializeField] private float speed = 3;

    private Transform follow;

    private bool caught = false;

    private bool catchable = false;

    private Vector3 homePosition;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        homePosition = transform.position;
        scoreKeeper = GetComponent<ScoreKeeper>();
        uiManager = FindAnyObjectByType<UIManager>();
        Reset();
    }

    public void Update()
    {
        if (caught)
        {
            transform.position = new Vector3(follow.position.x, transform.position.y, 0);

            if (!catchable)
            {
                caught = false;
                rb.linearVelocity = Vector2.down * speed;
            }

            return;
        }

        if (Mathf.Abs(rb.linearVelocity.y) < Mathf.Abs(rb.linearVelocity.x) * 0.15f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Sign(rb.linearVelocity.y) * Mathf.Abs(rb.linearVelocity.x) * 0.15f);

        if (rb.linearVelocity.magnitude != speed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent?.GetComponent<Paddle>())
        {
            if (catchable)
            {
                rb.linearVelocity = Vector2.zero;
                caught = true;
                follow = collision.transform;
            }
        }
        if (collision.gameObject.GetComponent<Brick>())
        {
            collision.gameObject.SetActive(false);
            scoreKeeper.IncreaseScore(1);
            uiManager.UpdateCurrentScore(scoreKeeper.Score);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Killzone")
        {
            uiManager.UpdateCurrentHighscore(scoreKeeper.TryToSaveHighScore("Breakout") ? scoreKeeper.Score : scoreKeeper.GetHighscore("Breakout"));

            RoundManager.Singleton.EndGame();
        }
    }

    public void SetCatchable(bool value)
    {
        catchable = value;
    }

    public void Stop()
    {
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;
    }

    public void Reset()
    {
        rb.simulated = true;

        transform.position = homePosition;
        rb.linearVelocity = Random.insideUnitCircle.normalized * speed;
    }
}
