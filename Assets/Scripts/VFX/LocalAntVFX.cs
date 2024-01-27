using System;
using System.Collections;
using System.Collections.Generic;
using GGJ2024;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocalAntVFX : MonoBehaviour
{
  [SerializeField] private Animator _smoke;
  [SerializeField] private ParticleSystem Blood;
  [SerializeField] private SpriteRenderer _antVisual;
  private float TimeToClear;
 
  private void Start()
  {
    _smoke.enabled = false;
    _smoke.StopPlayback();
    _smoke.speed = 0;
  }

  private void GetTimeToclear()
  {
    TimeToClear = Blood.main.startLifetime.constantMax +(Mathf.Abs(Blood.main.startLifetime.constantMax- _smoke.GetCurrentAnimatorStateInfo(0).length));
  }

  [Button]
  public void ExplodeKill()
  {
    _antVisual.gameObject.SetActive(false);
    _smoke.enabled = true;
    _smoke.Play("Smoke",-1,0);
    GetTimeToclear();
    _smoke.speed = 1;
    Blood.Play();
  }

  public void ClearTemp()
  {
    _antVisual.gameObject.SetActive(true);
    Blood.Stop(true);
    Blood.Clear();
    _smoke.speed = 0;
  }
}