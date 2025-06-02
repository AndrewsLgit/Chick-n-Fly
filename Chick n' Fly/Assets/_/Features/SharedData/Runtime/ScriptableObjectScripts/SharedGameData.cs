using System;
using System.Collections.Generic;
using UnityEngine;

namespace _.Features.SharedData.Runtime
{
    [CreateAssetMenu(fileName = "New SharedGameData", menuName = "SharedData/ScriptableObjects/SharedGameData")]
    public abstract class SharedGameData : ScriptableObject
    {
        // #region Private Variables
        //
        // [SerializeField]
        // private float _vehicleSpeed = 10f;
        // [SerializeField]
        // private float _initialMaxGameTimer = 60f;
        //
        // private Queue<Vehicle> _pooledVehicles = new Queue<Vehicle>();
        // private Queue<Vehicle> _nextPooledVehicles = new Queue<Vehicle>();
        // // Dictionary<Vector3(x = startVehicleTimer, y = endVehicleTimer, z = gameTime), Vector2 = playerInput>
        //
        // #endregion
        //
        // #region Public Members
        //
        // public enum VEHICLE_TYPE
        // {
        //     PLAYER = 0,
        //     GHOST,
        //     NONE
        // }
        //
        // public enum GAME_STATE
        // {
        //     IDLE = 0,
        //     PLAYING,
        //     PAUSED
        // }
        // public float GameTimer { get; private set; } = 0f;
        //
        // public float VehicleSpeed => _vehicleSpeed;
        //
        // /// <summary>
        // /// Vector3: x = startVehicleTimer, y = endVehicleTimer, z = currentGameTime
        // /// Vector2: playerInput Vector2
        // /// </summary>
        // public Dictionary<Vector3, Vector2> PlayerTimerInputVectors { get; } = new Dictionary<Vector3, Vector2>();
        //
        // #endregion
        //
        // #region Unity API
        //
        // private void OnEnable()
        // {
        //     GameTimer = _initialMaxGameTimer;
        // }
        //
        // #endregion
        //
        // #region Main Methods
        //
        // // On player input "OnMove" -> register round time + vehicleTimer (InputValue<Vector2>)
        // public void AddPlayerInputToDict(Vector3 timers, Vector2 playerInput)
        // {
        //     PlayerTimerInputVectors.Add(timers, playerInput);
        // }
        //
        // public void AddVehicleToQueue(Vehicle vehicle)
        // {
        //     _pooledVehicles.Enqueue(vehicle);
        // }
        //
        // #endregion
    }
}