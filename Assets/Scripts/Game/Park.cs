using UnityEngine;

namespace DefaultNamespace
{
    public class Park: MonoBehaviour
    {
        [SerializeField] private ParticleSystem fx;
        private ParticleSystem.MainModule _fxMainModule;
        private SpriteRenderer _spriteRenderer;
        private Route _route;
        public Route Route => _route;

        public void Initialize(Route route)
        {
            _route = route;
            _fxMainModule = fx.main;
        }

        private void OnValidate()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetColor(Color color)
        {
            _spriteRenderer.color = color;
        }

        public void StartFx()
        {
            _fxMainModule.startColor = _route.Color;
            fx.Play();
        }
    }
}