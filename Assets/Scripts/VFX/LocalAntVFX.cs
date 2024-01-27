using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LocalAntVFX : MonoBehaviour
{
  [SerializeField] private Animator _smoke;
  [SerializeField] private ParticleSystem Blood;
  private float TimeToClear;
 
  private void Start()
  {
    _smoke.enabled = false;
    _smoke.StopPlayback();
    _smoke.speed = 0;
  }

  private void GetTimeToclear()
  {
    Debug.Log(_smoke.GetCurrentAnimatorStateInfo(0).length);
    TimeToClear = Blood.main.startLifetime.constantMax +(Mathf.Abs(Blood.main.startLifetime.constantMax- _smoke.GetCurrentAnimatorStateInfo(0).length));
    Debug.Log(TimeToClear);
  }

  [Button]
  public void ExplodeKill()
  {
    _smoke.enabled = true;
    _smoke.Play("Smoke",-1,0);
    GetTimeToclear();
    _smoke.speed = 1;
    Blood.Play();
  }

  public void ClearTemp()
  {
    Blood.Stop(true);
    Blood.Clear();
    _smoke.speed = 0;
  }
}
