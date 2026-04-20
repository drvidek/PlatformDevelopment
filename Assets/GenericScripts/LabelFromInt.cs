using UnityEngine;
using UnityEngine.UI;

public class LabelFromInt : MonoBehaviour
{
    private Text label;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        label = GetComponent<Text>();
    }

    public void Set(int amount)
    {
        label.text = amount.ToString();
    }
}
