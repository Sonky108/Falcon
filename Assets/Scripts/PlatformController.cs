using System;
using UnityEngine;

namespace Falcon
{
    public class PlatformController : MonoBehaviour
    {

        public event Action<bool> OnPlayerLandingListeners;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var playerTag = SettingsUtils.PlayerTag;

            var isPlayer = collision.gameObject.CompareTag(playerTag);

            if (isPlayer)
            {
                var result = IsLandingSuccesful(collision);

                OnPlayerLandingListeners?.Invoke(result);
            }
        }

        private bool IsLandingSuccesful(Collision2D rocket)
        {
            bool result = false;

            var transform = rocket.gameObject.transform;
            var rigidbody = rocket.gameObject.GetComponent<Rigidbody2D>();

            Debug.Assert(rigidbody != null);

            var properPosition = IsTransformProperlyPositioned(transform);
            var properVelocity = IsProperVelocity(rocket);

            result = properPosition && properVelocity;

            return result;
        }

        private bool IsTransformProperlyPositioned(Transform rocket)
        {
            var maxDeviationDegree = GameRulesUtils.MaxDeviationDegree;

            return Mathf.DeltaAngle(rocket.eulerAngles.z, 0) <= maxDeviationDegree;
        }

        private bool IsProperVelocity(Collision2D collision)
        {
            var maxVelocityMagnitude = GameRulesUtils.MaxVelocityMagnitude;

            return Math.Abs(collision.relativeVelocity.magnitude) <= maxVelocityMagnitude;
        }
    }
}
