using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSupplyController : MonoBehaviour
{
    [SerializeField] private GameObject healthSupplyPrefab;
    [SerializeField] private float spawnTimeMin;
    [SerializeField] private float spawnTimeMax;
    [SerializeField]
    private float randomTime;

    [SerializeField] private Vector2 spawnPositionMin;
    [SerializeField] private Vector2 spawnPositionMax;
    private Vector2 spawnPosition;

    // [SerializeField] private player health script;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(spawnPositionMin, new Vector2(spawnPositionMin.x, spawnPositionMax.y));
        Gizmos.DrawLine(spawnPositionMin, new Vector2(spawnPositionMax.x, spawnPositionMin.y));
        Gizmos.DrawLine(spawnPositionMax, new Vector2(spawnPositionMin.x, spawnPositionMax.y));
        Gizmos.DrawLine(spawnPositionMax, new Vector2(spawnPositionMax.x, spawnPositionMin.y));
    }

    private void Start()
    {
        StartCreateHealthSupply();
    }
    private void StartCreateHealthSupply()
    {
        StartCoroutine(CreateSequence());
    }
    private IEnumerator CreateSequence()
    {
        RandomSpawnPoint();
        CreateHealthSupply();
        RandomTime();
        yield return new WaitForSeconds(randomTime);
        StartCreateHealthSupply();
    }
    private void RandomTime()
    {
        randomTime = Random.Range(spawnTimeMin, spawnTimeMax);
    }
    private void RandomSpawnPoint()
    {
        spawnPosition = new Vector2(Random.Range(spawnPositionMin.x, spawnPositionMax.x), 
                                    Random.Range(spawnPositionMin.y, spawnPositionMax.y));
    }
    private void CreateHealthSupply()
    {
        GameObject supplyTemp;
        supplyTemp = Instantiate(healthSupplyPrefab, spawnPosition, healthSupplyPrefab.transform.rotation);
        // supplyTemp get component health supply and assign player health script
    }
}