using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatRumble : BaseKillStreak
{
    [SerializeField] private GameObject catPrefab;
    private Transform catRumbleTransform;

    [SerializeField] private float CatRumbleSpeed = 500f;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Vector2 endPosition;

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition, endPosition);
    }

    public override void OnFire(Vector2 mousePosition)
    {
        CreateCatTransform();
    }
    private void CreateCatTransform()
    {
        catRumbleTransform = Instantiate(catPrefab, startPosition, transform.rotation).transform;
        catRumbleTransform.GetComponent<DamageTrigger>().SetDamage(damage);
    }

    private void FixedUpdate() 
    {
        CheckIfReachEndPosition();
        MoveCate();
    }
    private void CheckIfReachEndPosition()
    {
        if(catRumbleTransform == null) { return; }
        if(Vector2.Distance(catRumbleTransform.position, endPosition) < 0.1f)
        {
            Destroy(catRumbleTransform.gameObject);
            catRumbleTransform = null;
        }
    }
    private void MoveCate()
    {
        if(catRumbleTransform != null)
        {
            catRumbleTransform.position = Vector2.MoveTowards(catRumbleTransform.position, endPosition, CatRumbleSpeed * Time.deltaTime);
        }
    }
}
