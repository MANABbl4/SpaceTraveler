using UnityEngine;

namespace Scripts
{
    public interface IGravityObject
    {
        float GetMass();

        Vector3 Position { get; }
    }
}