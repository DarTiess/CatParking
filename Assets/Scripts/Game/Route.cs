using System;
using Game.CatView;
using Infrastructure.EventsBus;
using Infrastructure.EventsBus.Signals;
using Infrastructure.Installers;
using UnityEngine;

namespace Game
{
    public class Route: MonoBehaviour
    {
        [SerializeField] private Line _line;
        [SerializeField] private Park _park;
        [SerializeField] private Cat _cat;

        public bool IsActive => _isActive;
        public Line Line => _line;
        public Cat Cat => _cat;
        public Vector3[] LinePoints => _linePoints;
        public Color Color => _color;

        private bool _isActive=true;
        private Color _color;
        private CatSettings _catSetting;
        private IEventBus _eventBus;
        private Vector3[] _linePoints;
        private IGame _game;

        public event Action CatFinished;

        public void Initialize(IEventBus eventBus, IGame game, CatSettings catSettings, int index)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<ParkLinkedToLine>(OnParkLinkedToLineHandler);
            _game = game;
            _catSetting = catSettings;
            _color = _catSetting.Color[index];
            SetMainColor();
        }

        private void OnDisable()
        {
            _eventBus.Unsubscribe<ParkLinkedToLine>(OnParkLinkedToLineHandler);
        }

        private void OnParkLinkedToLineHandler(ParkLinkedToLine obj)
        {
            if (obj.Route == this)
            {
                _linePoints = obj.LinePoints.ToArray();
               _game.RegisterRoute(this);
            }
        }
        public void Deactivate()
        {
            _isActive = false;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying &&
                _line != null && _cat != null && _park != null)
            {
                _line.SetStartEndPosition(_cat.BottomTransform.position, _park.transform.position);
            }
        }
#endif
        private void SetMainColor()
        {
            _line.SetColor(_color);
            _park.SetColor(_color);
            _cat.SetColor(_color);
            _cat.Initialize(this, _catSetting.ActionSettings);
            _park.Initialize(this);
        }

        public void CatOnPlace()
        {
            _park.StartFx();
            CatFinished?.Invoke();
        }

        public void CatFailed()
        {
            _eventBus.Invoke(new Failed());
        }
    }
}