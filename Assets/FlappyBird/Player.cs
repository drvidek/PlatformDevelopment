using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class FlappyPlayer : MonoBehaviour, IReset
{
    [SerializeField] private float jumpPower;
    Rigidbody2D rb;

    private RoundManager roundManager;
    private Vector3 homePosition;

    private ScoreKeeper scoreKeeper;

    private Rigidbody2D currentPipe;
    private bool canScore;

    private UIManager uIManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreKeeper = GetComponent<ScoreKeeper>();
        uIManager = FindObjectOfType<UIManager>();
        homePosition = transform.position;
    }

    public void Update()
    {
        //Input.GetMouseButtonDown(0) will fire when the screen is touched!
        if (Input.GetMouseButtonDown(0) && RoundManager.Singleton.RoundActive)
        {
            Jump();
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, Vector2.up, 3f);

        if (hit)
        {
            if (!currentPipe)
            {
                canScore = true;
                currentPipe = hit.rigidbody;
            }
        }
        else
        {
            currentPipe = null;
        }

        if (currentPipe && canScore && transform.position.x > currentPipe.position.x)
        {
            canScore = false;
            scoreKeeper.IncreaseScore(1);
            uIManager.UpdateCurrentScore(scoreKeeper.Score);
        }

    }

    public void Jump()
    {
        rb.velocity = Vector2.up * jumpPower;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (roundManager == null)
        {
            roundManager = FindObjectOfType<RoundManager>();
        }
        rb.velocity = Vector2.zero;
        rb.simulated = false;
        uIManager.UpdateCurrentHighscore(scoreKeeper.TryToSaveHighScore("FlappyBird") ? scoreKeeper.Score : scoreKeeper.GetHighscore("FlappyBird"));
        roundManager.EndGame();
    }

    public void Reset()
    {
        //scoreKeeper.Reset();
        uIManager.UpdateCurrentScore(0);
        rb.simulated = true;
        transform.position = homePosition;
    }
}
