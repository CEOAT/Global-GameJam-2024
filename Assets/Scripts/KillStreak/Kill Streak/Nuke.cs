using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Nuke : BaseKillStreak
{
    [SerializeField] private GameObject nukePrefab;
    private Transform nukeTransform;
    [SerializeField] private float nukeScaleDownSpeed = 1f;
    [SerializeField] private float nukeMinimumSize = 0.1f;

    [SerializeField] private GameObject nukeDamageTriggerPrefab;
    private GameObject nukeDamageTriggerObject;

    [SerializeField] private GameObject whiteScreenPrefab;
    private SpriteRenderer whiteScreenSprite;
    [SerializeField] private float fadeInSpeed = 1f;
    [SerializeField] private float fadeOutSpeed = 1f;
    [SerializeField] private float fadeInWaitTime = 1f;
    [SerializeField] private float fadeOutWaitTime = 1f;
    private bool isFadeOut;
    private float alphaValue;

    public override void OnFire(Vector2 mousePosition)
    {
        CreateNuke(mousePosition);
    }
    private void CreateNuke(Vector2 nukePosition)
    {
        nukeTransform = Instantiate(nukePrefab, nukePosition, nukePrefab.transform.rotation).transform;
    }

    [Button("Test Nuke")]
    private void TestNuke()
    {
        //CreateNuke(new Vector2(0,0));
    }

    private void FixedUpdate()
    {
        ScaleNukeDown();
        ChangeWhiteScreenAlpha();
        CheckDetonation();
    }
    private void ScaleNukeDown()
    {
        if(nukeTransform != null)
        {
            nukeTransform.localScale = Vector3.Lerp(nukeTransform.localScale, new Vector3(), nukeScaleDownSpeed);
        }
    }

    private void ChangeWhiteScreenAlpha()
    {
        if(whiteScreenSprite == null) { return; }
        
        if(!isFadeOut)
        {
            alphaValue += Time.deltaTime * fadeInSpeed;
            whiteScreenSprite.color = new Color(1,1,1,alphaValue);
        }
        else if(isFadeOut)
        {
            alphaValue -= Time.deltaTime * fadeInSpeed;
            whiteScreenSprite.color = new Color(1,1,1,alphaValue);
        }
    }
    private void CheckDetonation()
    {
        if(nukeTransform != null && nukeTransform.localScale.x < nukeMinimumSize)
        {
            Destroy(nukeTransform.gameObject);
            nukeTransform = null;
            StartCoroutine(DetonationSequence());
        }
    }
    private IEnumerator DetonationSequence()
    {
        whiteScreenSprite = Instantiate(whiteScreenPrefab).GetComponent<SpriteRenderer>();
        nukeDamageTriggerObject = Instantiate(nukeDamageTriggerPrefab);
        nukeDamageTriggerObject.GetComponent<DamageTrigger>().SetDamage(damage);
        whiteScreenSprite.color = new Color(1,1,1,0);
        alphaValue = 0;
        isFadeOut = false;
        yield return new WaitForSeconds(fadeInWaitTime);

        isFadeOut = true;
        Destroy(nukeDamageTriggerObject.gameObject);
        nukeDamageTriggerObject = null;
        yield return new WaitForSeconds(fadeOutWaitTime);

        Destroy(whiteScreenSprite.gameObject);
        whiteScreenSprite = null;
    }
}