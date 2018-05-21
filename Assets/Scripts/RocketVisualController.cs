using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Falcon
{
    public class RocketVisualController : MonoBehaviourSingleton<RocketVisualController>
    {

        private SpriteRenderer renderer;

        public override void OnAwake()
        {
            base.OnAwake();

            renderer = GetComponent<SpriteRenderer>();

            BindListeners();
        }

        private void BindListeners()
        {
            GameManager.Instance.OnGameEndedListeners += OnGameEnded;
            GameManager.Instance.OnGameStartedListeners += OnGameStarted;
        }

        private void OnGameStarted(LevelSettings obj)
        {
            if (renderer)
            {
                renderer.color = Color.white;
            }
        }

        private void OnGameEnded(bool success)
        {
            if (renderer)
            {
                Color color = success ? Color.green : Color.red;

                renderer.color = color;
            }
        }

        private void OnDestroy()
        {
            UnbindListeners();
        }

        private void UnbindListeners()
        {
            GameManager.Instance.OnGameEndedListeners -= OnGameEnded;
            GameManager.Instance.OnGameStartedListeners -= OnGameStarted;
        }
    }
}
