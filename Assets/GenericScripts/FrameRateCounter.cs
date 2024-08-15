using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameRateCounter : MonoBehaviour
{
    public Text text;

    private void Update()
    {
        text.text = Mathf.Round(1f / Time.unscaledDeltaTime).ToString();
    }
}