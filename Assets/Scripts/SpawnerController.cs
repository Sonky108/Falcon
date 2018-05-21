using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Falcon
{
    public class SpawnerController : MonoBehaviour
    {

        private CircleCollider2D circleCollider2D;

        private void Awake()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
        }

        public Vector3 GetRandomPosition()
        {
            var position = Random.insideUnitCircle * circleCollider2D.radius + (Vector2)circleCollider2D.transform.position;

            return position;
        }
    }
}
