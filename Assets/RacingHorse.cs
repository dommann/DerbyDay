using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    [SerializeField] private Animator _jockeyAnimator;
    public OwnerState OwnerState;
    public HorseState HorseState;
    public int HorseIndex;
    public float Speed;
    public Animator Animator;
    private Collider _collider;
    private bool _isSpeedBoosted = false;

    private void Awake()
    {
        OwnerState = OwnerState.AI;
    }

    private void Update()
    {
        if (HorseState == HorseState.Running)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * Speed);
            RaceController.Instance.HorseCoveredDistances[this] += Time.deltaTime * Speed;
        }
    }

    public void StartRunning()
    {
        HorseState = HorseState.Running;
        Animator.SetBool("isRunning", true);
        Animator.SetFloat("AnimatorRunSpeed",Speed/10);
        _jockeyAnimator.SetBool("isRunning",true);
    }
    public void SetOwnerState(OwnerState ownerState,int horseIndex = -1)
    {
        OwnerState = ownerState;
        if (horseIndex != -1)
        {
            UIController.Instance.ActivateInputUI(horseIndex);
        }
            
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FinishLine")
        {
            HorseState = HorseState.Finished;
            Animator.SetBool("isRunning", false);
            _jockeyAnimator.SetBool("isRunning",false);
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
