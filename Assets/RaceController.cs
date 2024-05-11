using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RaceState
{
    InsertCoin,
    Started,
    Finished
}

public class RaceController : Singleton<RaceController>
{
    
    [SerializeField] RaceCamera _raceCamera;
    [SerializeField] List<RacingHorse> _horses = new List<RacingHorse>();
    List<RacingHorse> _finishedHorses = new List<RacingHorse>();
    public Dictionary<RacingHorse,float> HorseCoveredDistances;
    [SerializeField] private Transform _finishLine;
    private RaceState _raceState;
    protected override void Awake()
    {
        base.Awake();
        _raceState = RaceState.InsertCoin;
    }

    private void Update()
    {
        if (_raceState == RaceState.InsertCoin)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                StartRace();
            }
            else if (Input.GetKey(KeyCode.A))
            {
                _horses[0].SetOwnerState(OwnerState.Player,0);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _horses[1].SetOwnerState(OwnerState.Player,1);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                _horses[2].SetOwnerState(OwnerState.Player,2);
            }
            else if (Input.GetKey(KeyCode.F))
            {
                _horses[3].SetOwnerState(OwnerState.Player,3);
            }
        }
        else if (_raceState==RaceState.Started)
        {
            SetLeadingHorseCrown();
            SetHorseProgressions();
            if(Input.GetKey(KeyCode.A))
            {
                _horses[0].ActivateSpeedBoost(3,0.5f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _horses[1].ActivateSpeedBoost(3,0.5f);
            }
            
        }
        

    }

    public RacingHorse GetLeadingHorse()
    {
        return HorseCoveredDistances?.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
    }
    private void SetLeadingHorseCrown()
    {
        var horseWithMaxDistance = GetLeadingHorse();
        foreach (var horse in _horses)
        {
            if (horse != horseWithMaxDistance)
            {
                horse.SetCrownVisibility(false);
            }
            else
            {
                horse.SetCrownVisibility(true);
            }
        }
    }
    private void SetHorseProgressions()
    {
        var maxDistanceCanCovered = _finishLine.position.z;
        foreach ((RacingHorse horse,float distance) in HorseCoveredDistances)
        {
            UIController.Instance.SetHorseProgression(horse, distance / maxDistanceCanCovered);
        }

    }
    void ShowInsertCoinScreen()
    {
        if (Input.GetKey(KeyCode.A))
        {
            _horses[0].SetOwnerState(OwnerState.Player);
        }
        else if (Input.GetKey(KeyCode.B))
        {
            _horses[1].SetOwnerState(OwnerState.Player);
        }
    }

    void StartRace()
    {
        _raceCamera.Initialize();
        _raceState = RaceState.Started;
        HorseCoveredDistances = new Dictionary<RacingHorse, float>();
        foreach (var horse in _horses)
        {
            HorseCoveredDistances.Add(horse,0);
            horse.StartRunning();
        }
    }
    
    void HideInsertCoinScreen()
    {
        
    }

    public void SetHorseFinished(RacingHorse racingHorse)
    {
        _finishedHorses.Add(racingHorse);
        if (_finishedHorses.Count == _horses.Count)
        {
            _raceState = RaceState.Finished;
            PlayerPrefs.SetInt("First",_finishedHorses[0].HorseIndex);
            PlayerPrefs.SetInt("Second",_finishedHorses[1].HorseIndex);
            PlayerPrefs.SetInt("Third",_finishedHorses[2].HorseIndex);
            PlayerPrefs.SetInt("Fourth",_finishedHorses[3].HorseIndex);
            PlayerPrefs.Save();
            SceneManager.LoadScene("FinishScene");
            // todo Show the winner
        }
    }
}
