using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    [RequireComponent(typeof(Animator))]
    public class Cat: MonoBehaviour
    {
        [SerializeField] private Transform _bottom;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private SkinnedMeshRenderer _meshRenderer;
        public Transform BottomTransform => _bottom;
        public Route Route => _route;
        private Route _route;
        private float _durationMultiplier;
        private Animator _animator;
        private CatAnimation _catAnimation;
        private float _explosionForce;
        private float _explosionRadius;
        private float _explosionPosition;
        private float _explosionAngle;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _catAnimation = new CatAnimation(_animator);
            _catAnimation.Wait();
        }

        public void Initialize(Route route, ActionSettings settings)
        {
            _route = route;
            _durationMultiplier = settings.DurationMultiplier;
            _explosionForce = settings.ExplosionForce;
            _explosionRadius = settings.ExplosionRadius;
            _explosionPosition = settings.ExplosionPosition;
            _explosionAngle = settings.ExplosionAngle;

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out Cat otherCat))
            {
                _rigidbody.DOKill(false);
                Vector3 hitPoint = collision.contacts[0].point;
                AddExplosionForce(hitPoint);
                _route.CatFailed();
                _catAnimation.Idle();
            }
        }

        private void AddExplosionForce(Vector3 point)
        {
            _rigidbody.AddExplosionForce(_explosionForce, point, _explosionRadius);
            _rigidbody.AddForceAtPosition(Vector3.up *_explosionPosition, point, ForceMode.Impulse);
            _rigidbody.AddTorque(new Vector3(GetRandomAngle(), GetRandomAngle(), GetRandomAngle()));
        }

        private float GetRandomAngle()
        {
            float angle = _explosionAngle;
            float rnd = Random.value;
            return rnd > .5f ? angle : -angle;
        }

        public void Move(Vector3[] path)
        {
            _rigidbody.DOLocalPath(path,_durationMultiplier * path.Length)
                      .OnStart(() =>
                      {
                          _catAnimation.Move();
                      })
                      .SetLookAt(.01f, false)
                      .SetEase(Ease.Linear)
                      .OnComplete(() =>
                      {
                          _catAnimation.Wait();
                          _route.CatOnPlace();
                      });
            
        }

        public void SetColor(Color color)
        {
            _meshRenderer.materials[0].color=color;
        }
    }
}