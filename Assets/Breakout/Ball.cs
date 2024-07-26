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

    private bool moving = true;

    private bool input = false;

    private Vector3 homePosition;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        homePosition = transform.position;
        scoreKeeper = GetComponent<ScoreKeeper>();
        uiManager = FindObjectOfType<UIManager>();
        Reset();
    }

    public void Update()
    {
        input = CheckInput();
        if (!moving)
        {
            transform.position = new Vector3(follow.position.x, transform.position.y, 0);

            if (!input)
            {
                moving = true;
                rb.velocity = Vector2.down * speed;
            }

            return;
        }

        if (Mathf.Abs(rb.velocity.y) < Mathf.Abs(rb.velocity.x) * 0.15f)
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * Mathf.Abs(rb.velocity.x) * 0.15f);

        if (rb.velocity.magnitude != speed)
        {
            rb.velocity = rb.velocity.normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.parent?.GetComponent<Paddle>())
        {
            if (input)
            {
                rb.velocity = Vector2.zero;
                moving = false;
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

    private bool CheckInput()
    {
#if UNITY_EDITOR
        return Input.GetMouseButton(1);
#else
           return (Input.touchCount > 1);
#endif
    }

    public void Stop()
    {
        rb.velocity = Vector2.zero;
        rb.simulated = false;
    }

    public void Reset()
    {
        rb.simulated = true;

        transform.position = homePosition;
        rb.velocity = Random.insideUnitCircle.normalized * speed;
    }
}
