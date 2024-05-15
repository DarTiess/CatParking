using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Installers
{
    [System.Serializable]
    public class CatSettings
    {
        [SerializeField] private List<Color> color;
        [SerializeField] private ActionSettings actionSettings;

        public List<Color> Color => color;
        public ActionSettings ActionSettings => actionSettings;

    }

    [System.Serializable]
    public class ActionSettings
    {
        [SerializeField] private float durationMultiplier;
        [SerializeField] private float explosionForce=400f;
        [SerializeField] private float explosionRadius=3f;
        [SerializeField] private float explosionPosition=2f;
        [SerializeField] private float explosionAngle=10f;
    
        public float DurationMultiplier=>durationMultiplier;
        public float ExplosionForce => explosionForce;
        public float ExplosionRadius => explosionRadius;
        public float ExplosionPosition => explosionPosition;
        public float ExplosionAngle => explosionAngle;
    }
}