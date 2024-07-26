using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour, IStop, IReset
{
    [Tooltip("The pair of pipes to spawn")]
    [SerializeField] private GameObject pipePrefab;
    [Tooltip("How far up or down the pipes can spawn from centre")]
    [SerializeField] private float pipeSpawnRange;
    [Tooltip("How long to wait between pipes")]
    [SerializeField] private float pipeSpawnDelay;

    private float pipeLastSpawnTime;

    private PipePair[] pipesInScene = new PipePair[4];
    private int pipeIndex = 0;

    public void Reset()
    {
        pipeLastSpawnTime = Time.time;

        for (int i = 0; i < pipesInScene.Length; i++)
        {
            if (pipesInScene[i])
            Destroy(pipesInScene[i].gameObject);
            pipesInScene[i] = null;
        }
    }

    public void Stop()
    {
        foreach (var pipe in pipesInScene)
        {
            if (pipe == null)
            continue;

            pipe.Stop();
        }
    }

    private void Update()
    {
        if (!RoundManager.Singleton.RoundActive)
            return;

        if (Time.time > pipeLastSpawnTime + pipeSpawnDelay)
        {
            SpawnPipe();
        }
    }

    private void SpawnPipe()
    {
        pipeLastSpawnTime = Time.time;

        float yOffset = Random.Range(-pipeSpawnRange, pipeSpawnRange);

        if (pipesInScene[pipeIndex] == null)
            pipesInScene[pipeIndex] = Instantiate(pipePrefab, transform.position, Quaternion.identity).GetComponent<PipePair>();

        pipesInScene[pipeIndex].transform.position = transform.position + Vector3.up * yOffset;

        pipeIndex++;

        if (pipeIndex == pipesInScene.Length)
            pipeIndex = 0;
    }
}
