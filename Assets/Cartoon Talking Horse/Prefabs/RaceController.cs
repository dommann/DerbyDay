using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum RaceState
{
    InsertCoin,
    Started,
    Finished
}

public class RaceController : MonoBehaviour
{
    [SerializeField] RaceCamera _raceCamera;
    [SerializeField] List<RacingHorse> _horses = new List<RacingHorse>();
    Queue<RacingHorse> _finishedHorses = new Queue<RacingHorse>();
    public Dictionary<RacingHorse,float> HorseCoveredDistances;
    private RaceState _raceState;
    #region Singleton
    
    private static RaceController _instance;
    public static RaceController Instance
    {
        get
        {
            // Check if the instance is null
            if (_instance == null)
            {
                // Find the instance in the scene
                _instance = FindObjectOfType<RaceController>();

                // If no instance is found, log an error
                if (_instance == null)
                {
                    Debug.LogError("SingletonExample instance not found in the scene.");
                }
            }

            // Return the instance
            return _instance;
        }
    }

    #endregion
    

    private void Awake()
    {
        // Ensure there's only one instance
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Another instance of SingletonExample already exists. Destroying this instance.");
            Destroy(gameObject);
        }
        else
        {
            // Set the instance if it doesn't exist
            _instance = this;
            // Ensure the instance persists across scene loads
            DontDestroyOnLoad(gameObject);
        }
        _raceState = RaceState.InsertCoin;
    }

    private void Update()
    {
        if (_raceState==RaceState.Started)
        {
            SetLeadingHorseCrown();
            
            if(Input.GetKey(KeyCode.A))
            {
                _horses[0].ActivateSpeedBoost(3,0.5f);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _horses[1].ActivateSpeedBoost(3,0.5f);
            }
            
        }
        
        if (Input.GetKey(KeyCode.Space))
        {
            StartRace();
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
        _finishedHorses.Enqueue(racingHorse);
        if (_finishedHorses.Count == _horses.Count)
        {
            _raceState = RaceState.Finished;
            // todo Show the winner
        }
    }
}
