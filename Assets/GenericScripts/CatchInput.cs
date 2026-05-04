using UnityEngine;

public class CatchInput : MonoBehaviour
{
    public int catchFingerCount = 2;

    private Ball ball;

    void Start()
    {
        ball = GetComponent<Ball>();
    }

    public void CheckFingers(int count)
    {
        ball.SetCatchable(count >= catchFingerCount);
    }
}
