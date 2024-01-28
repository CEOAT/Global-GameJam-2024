using System.Collections;
using System.Collections.Generic;
using GGJ2024;
using Sirenix.Utilities;
using UnityEngine;

public class GiantMolotov : BaseKillStreak
{
    [SerializeField] float damageRange = 3f;
    [SerializeField] float damageTime = 5f;
    [SerializeField] GameObject fireArea;
    [SerializeField] float burnEverySec;
    float burnTime = 0;
    List<Vector2> damageArea = new List<Vector2>();

    AudioSource audioSource;
    
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public override void OnFire(Vector2 mousePosition)
    {
        damageArea.Add(mousePosition);
        GameObject _fireArea = Instantiate(fireArea,mousePosition,Quaternion.identity);
        _fireArea.transform.localScale = new Vector3(damageRange * 2,damageRange * 2,damageRange * 2);
        StartCoroutine(StopFire(_fireArea));
    }

    protected override void Update() 
    {   
        base.Update();

        PlayBurnSound();

        if(damageArea.IsNullOrEmpty())
            return;

        burnTime -= Time.deltaTime;
        BurnEntity();
    }

    void PlayBurnSound()
    {
        if(damageArea.IsNullOrEmpty())
        {
            audioSource.Stop();
            return;
        }
        else
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
                return;
            }
        }
    }

    void BurnEntity()
    {
        if(burnTime <= 0)
        {
            foreach(var n in damageArea)
            {
                Collider2D[] detectAnts = Physics2D.OverlapCircleAll(n, damageRange);
                foreach (Collider2D ant in detectAnts)
                {
                    if (ant.GetComponent<IEntity>() != null)
                    {
                        ant.transform.gameObject.GetComponent<IEntity>().TakeDamage(damage,false);
                    }
                }
            }

            burnTime = burnEverySec;
        }
    }

    IEnumerator StopFire(GameObject _fireArea)
    {
        yield return new WaitForSeconds(damageTime);
        damageArea.Remove(damageArea[0]);
        Destroy(_fireArea);
    }

    private void OnDrawGizmos() 
    {
        foreach(var n in damageArea)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(n,damageRange);
        }
    }
}
