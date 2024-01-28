using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class HealthSupplyController : MonoBehaviour
{
    [SerializeField] HealthSupply healthSupplyPrefab;
    [SerializeField] float spawnTimeMin;
    [SerializeField] float spawnTimeMax;
    float randomTime;

    [SerializeField] Vector2 spawnPositionMin;
    [SerializeField] Vector2 spawnPositionMax;

    [SerializeField] AntTarget antTarget;

    void Start()
    {
        StartCreateHealthSupply();
    }
    
    void StartCreateHealthSupply()
    {
        StartCoroutine(CreateSequence());
    }
    
    IEnumerator CreateSequence()
    {
        CreateHealthSupply();
        RandomTime();
        yield return new WaitForSeconds(randomTime);
        StartCreateHealthSupply();
    }

    void RandomTime()
    {
        randomTime = Random.Range(spawnTimeMin, spawnTimeMax);
    }

    Vector2 RandomSpawnPoint()
    {
        return new Vector2(Random.Range(spawnPositionMin.x, spawnPositionMax.x), 
                                    Random.Range(spawnPositionMin.y, spawnPositionMax.y));
    }

    [Button]
    void CreateHealthSupply()
    {
        HealthSupply supplyTemp = Instantiate(healthSupplyPrefab, RandomSpawnPoint(), healthSupplyPrefab.transform.rotation);
        supplyTemp.onHeal += antTarget.Heal;
        // supplyTemp get component health supply and assign player health script
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(spawnPositionMin, new Vector2(spawnPositionMin.x, spawnPositionMax.y));
        Gizmos.DrawLine(spawnPositionMin, new Vector2(spawnPositionMax.x, spawnPositionMin.y));
        Gizmos.DrawLine(spawnPositionMax, new Vector2(spawnPositionMin.x, spawnPositionMax.y));
        Gizmos.DrawLine(spawnPositionMax, new Vector2(spawnPositionMax.x, spawnPositionMin.y));
    }
}
