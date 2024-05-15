using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class LinesDrawer: MonoBehaviour
    {
        [SerializeField] private int intercatableLayer;
        private Line _currentLine;
        private Route _currentRoute;

        private RaycastDetector _raycastDetector = new();
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<MouseDown>(OnMouseDown);
            _eventBus.Subscribe<MouseMove>(OnMouseMove);
            _eventBus.Subscribe<MouseUp>(OnMouseUp);
        }

        private void OnMouseDown(MouseDown obj)
        {
            ContactInfo contactInfo = _raycastDetector.RayCast(intercatableLayer);
            if (contactInfo.Contacted)
            {
                bool isCat = contactInfo.Collider.TryGetComponent(out Cat cat);
                if (isCat && cat.Route.IsActive)
                {
                    _currentRoute = cat.Route;
                    _currentLine = _currentRoute.Line;
                    _currentLine.Initialize();
                }
            }
        }

        private void OnMouseMove(MouseMove obj)
        {
            if (_currentRoute == null)
                return;
            
            ContactInfo contactInfo = _raycastDetector.RayCast(intercatableLayer);
            if (!contactInfo.Contacted)
                return;
           
            Vector3 newPoint = contactInfo.Point;
            _currentLine.AddPoints(newPoint);

            bool isPark = contactInfo.Collider.TryGetComponent(out Park park);
            if (isPark)
            {
                Route parkRoute = park.Route;
                if (parkRoute == _currentRoute)
                {
                    _currentLine.AddPoints(contactInfo.Transform.position);
                    MouseUpHandler();
                }
                else
                {
                    _currentLine.Clear();
                }
            }
        }

        private void OnMouseUp(MouseUp obj)
        {
            MouseUpHandler();
        }

        private void MouseUpHandler()
        {
            if (_currentRoute == null)
                return;
            
            ContactInfo contactInfo = _raycastDetector.RayCast(intercatableLayer);
          
            if (!contactInfo.Contacted)
            {
                _currentLine.Clear();
            }
            else
            {
                bool isPark = contactInfo.Collider.TryGetComponent(out Park park);

                if (!isPark || _currentLine.PointsCount < 2)
                {
                    _currentLine.Clear();
                }
                else
                {
                    _eventBus.Invoke(new ParkLinkedToLine(_currentRoute,_currentLine.Points));
                    _currentRoute.Deactivate();
                }
            }

            ResetDrawer();
        }

        private void ResetDrawer()
        {
            _currentLine = null;
            _currentRoute = null;
        }
    }
}