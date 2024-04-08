using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OwnerState
{
    Player,
    AI
}
public enum HorseState
{
    Idle,
    Running,
    Finished
}

public class RacingHorse : MonoBehaviour
{
    [SerializeField] private ParticleSystem _speedBoostEffect;
    [SerializeField] private GameObject _crown;
    public OwnerState ownerState;
    public HorseState horseState;
    public float Speed;
    public Animator Animator;
    private Collider _collider;
    private bool _isSpeedBoosted = false;
    private void Update()
    {
        if (horseState == HorseState.Running)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
            RaceController.Instance.HorseCoveredDistances[this] += Time.deltaTime * Speed;
        }
    }

    public void StartRunning()
    {
        horseState = HorseState.Running;
        Animator.SetBool("isRunning", true);
        Animator.SetFloat("AnimatorRunSpeed",Speed/10);
    }
    public void SetOwnerState(OwnerState ownerState)
    {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FinishLine")
        {
            horseState = HorseState.Finished;
            Animator.SetBool("isRunning", false);
            RaceController.Instance.SetHorseFinished(this);
            //_collider.enabled = false;
        }
    }

    public void ActivateSpeedBoost(float buffMultiplier, float buffDuration)
    {
        if (_isSpeedBoosted)
            return;
        _isSpeedBoosted = true;
        _speedBoostEffect.Play();
        Speed *= buffMultiplier;
        StartCoroutine(RevertSpeedBuff(buffMultiplier,buffDuration));
    }
    private IEnumerator RevertSpeedBuff(float buffMultiplier,float buffDuration)
    {
        yield return new WaitForSeconds(buffDuration);
        Speed /= buffMultiplier;
        _isSpeedBoosted = false;
    }

    public void SetCrownVisibility(bool value)
    {
        _crown.SetActive(value);
    }
}
