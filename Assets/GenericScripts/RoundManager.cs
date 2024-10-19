using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IReset
{
    public void Reset();
}

public interface IStop
{
    public void Stop();
}

public class RoundManager : MonoBehaviour
{
    private static RoundManager _singleton;

    public static RoundManager Singleton
    {
        get => _singleton;
        set
        {
            if (_singleton != null)
            {
                Debug.LogWarning($"RoundManager already has a singleton! Make sure there is only one RoundManager in your scene.");
                Destroy(value);
                return;
            }
            _singleton = value;
        }
    }

    public void Awake()
    {
        Singleton = this;
    }

    private void OnDestroy()
    {
        if (Singleton == this)
            _singleton = null;
    }

    private bool roundActive = true;
    public bool RoundActive => roundActive;

    public void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void NewGame()
    {
        roundActive = true;

        foreach (IReset item in FindObjectsOfType<MonoBehaviour>(true).OfType<IReset>())
        {
            item.Reset();
        }
    }

    public void EndGame()
    {
        roundActive = false;
        foreach (IStop item in FindObjectsOfType<MonoBehaviour>(true).OfType<IStop>())
        {
            item.Stop();
        }
    }
}
