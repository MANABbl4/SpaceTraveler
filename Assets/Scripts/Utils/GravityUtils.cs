using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Utils
{
    public static class GravityUtils
    {
        public static KeyValuePair<Vector3, Vector3> CalculateGravity<T>(IEnumerable<T> gravityObjects, Vector3 objPosition, Vector3 objVelocity, Vector3 objAcc, float dt) where T : IGravityObject
        {
            Vector3 a = Vector3.zero;

            foreach (var gravityObj in gravityObjects)
            {
                var dist = gravityObj.Position - objPosition;
                var dir = dist.normalized;
                a += dir * (float)(gravityObj.GetMass() / dist.sqrMagnitude);
            }

            var dS1 = (a + objAcc) * dt * dt / 2;
            var dS2 = objVelocity * dt;

            return new KeyValuePair<Vector3, Vector3>(dS1 + dS2, (a + objAcc) * dt);
        }
    }
}
