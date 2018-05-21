using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Falcon
{
    public class CharacterMovementController : MonoBehaviourSingleton<CharacterMovementController>
    {
        public event Action OnFuelEndedListeners;

        private ConstantForce2D throttle;
        private Transform transform;
        private Rigidbody2D rigidbody2D;

        private float momentum;
        private float thrust;

        private float currentFuel;
        private float maxFuel;

        public override void OnAwake()
        {
            base.OnAwake();

            EnsureComponents();

            GameManager.Instance.OnGameDataLoadedListeners += OnSettingsLoaded;
            GameManager.Instance.OnGameStartedListeners += OnGameStarted;
            GameManager.Instance.OnGameEndedListeners += OnGameEnded;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnGameDataLoadedListeners -= OnSettingsLoaded;
            GameManager.Instance.OnGameStartedListeners -= OnGameStarted;
            GameManager.Instance.OnGameEndedListeners -= OnGameEnded;
        }

        private void EnsureComponents()
        {
            throttle = GetComponent<ConstantForce2D>();
            transform = GetComponent<Transform>();
            rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void BindButtons()
        {
            ControllerUI.Instance.TurnLeftDownListeners += OnLeftButtonPressed;
            ControllerUI.Instance.TurnRightDownListeners += OnRightButtonPressed;
            ControllerUI.Instance.ThrottleDownListeners += OnThrottleButtonPressed;

            ControllerUI.Instance.TurnLeftUpListeners += OnLeftButtonReleased;
            ControllerUI.Instance.TurnRightUpListeners += OnRightButtonReleased;
            ControllerUI.Instance.ThrottleUpListeners += OnThrottleButtonReleased;
        }

        private void UnbindButtons()
        {
            ControllerUI.Instance.TurnLeftDownListeners -= OnLeftButtonPressed;
            ControllerUI.Instance.TurnRightDownListeners -= OnRightButtonPressed;
            ControllerUI.Instance.ThrottleDownListeners -= OnThrottleButtonPressed;

            ControllerUI.Instance.TurnLeftUpListeners -= OnLeftButtonReleased;
            ControllerUI.Instance.TurnRightUpListeners -= OnRightButtonReleased;
            ControllerUI.Instance.ThrottleUpListeners -= OnThrottleButtonReleased;
        }

        private void OnLeftButtonPressed()
        {
            SetTorque(true);
        }

        private void OnRightButtonPressed()
        {
            SetTorque(false);
        }

        private void OnThrottleButtonPressed()
        {
            OpenThrottle();
        }

        private void OnThrottleButtonReleased()
        {
            CloseThrottle();
        }

        private void OnRightButtonReleased()
        {
            StopTorque();
        }

        private void OnLeftButtonReleased()
        {
            StopTorque();
        }

        private void SetTorque(bool turnLeft)
        {
            if(throttle)
            {
                var turnMomentum = momentum;

                if (!turnLeft)
                {
                    turnMomentum = -turnMomentum;
                }

                throttle.torque = turnMomentum;
            }
        }

        private void StopTorque()
        {
            if (throttle)
            {
                throttle.torque = 0;
            }
        }

        private void CloseThrottle()
        {
            if (throttle)
            {
                throttle.relativeForce = new Vector2(0, 0);
            }
        }

        private void OpenThrottle()
        {
            if (throttle)
            {
                throttle.relativeForce = new Vector2(0, thrust);

                LowerFuel(Time.fixedDeltaTime);
            }
        }

        public void OnSettingsLoaded(GameSettings data)
        {
            this.momentum = data.momentum;
            this.thrust = data.thrust;
            rigidbody2D.mass = data.mass;

            SetFuel(data);
        }

        private void OnGameStarted(LevelSettings obj)
        {
            BindButtons();

            PrepareFuel();

            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;

            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = obj.StartPosition;
        }

        private void OnGameEnded(bool success)
        {
            UnbindButtons();   
        }

        private void SetFuel(GameSettings data)
        {
            maxFuel = data.fuel;
        }

        private void LowerFuel(float value)
        {
            currentFuel -= value;

            CheckSufficientFuel();
        }

        private void CheckSufficientFuel()
        {
            if (currentFuel <= 0)
            {
                OnFuelEndedListeners?.Invoke();
            }
        }

        private void PrepareFuel()
        {
            currentFuel = maxFuel;
        }

        public float GetFuelPercentage()
        {
            return currentFuel / maxFuel;
        }
    }
}