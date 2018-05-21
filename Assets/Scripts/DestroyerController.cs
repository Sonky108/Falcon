using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Falcon
{
    public class DestroyerController : MonoBehaviourSingleton<DestroyerController>
    {

        public event Action OnPlayerTriggeredListeners;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var playerTag = SettingsUtils.PlayerTag;

            var isPlayer = collision.gameObject.CompareTag(playerTag);

            if (isPlayer)
            {
                OnPlayerTriggeredListeners?.Invoke();
            }
        }
    }
}
