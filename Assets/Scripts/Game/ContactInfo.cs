using UnityEngine;

namespace Game
{
    public struct ContactInfo
    {
        public bool Contacted;
        public Vector3 Point;
        public Collider Collider;
        public Transform Transform;
    }
}