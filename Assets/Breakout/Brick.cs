using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour, IReset
{
    public void Reset()
    {
        gameObject.SetActive(true);
    }
}
