﻿using Game.CatView;
using Infrastructure.EventsBus;
using Infrastructure.EventsBus.Signals;
using UnityEngine;
using Zenject;

namespace Game
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

        private void OnDisable()
        {
            _eventBus.Unsubscribe<MouseDown>(OnMouseDown);
            _eventBus.Unsubscribe<MouseMove>(OnMouseMove);
            _eventBus.Unsubscribe<MouseUp>(OnMouseUp);
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
                    _currentLine.ShowLine();
                    
                    _eventBus.Invoke(new BeginDraw(_currentRoute));
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

            if (_currentLine.Lenght >= _currentRoute.MaxLineLenght)
            {
                _currentLine.Clear();
                MouseUpHandler();
                return;
            }
            _currentLine.AddPoints(newPoint);
            _eventBus.Invoke(new Draw());

            bool isPark = contactInfo.Collider.TryGetComponent(out Park park);
            if (isPark)
            {
                Route parkRoute = park.Route;
                if (parkRoute == _currentRoute)
                {
                    _currentLine.AddPoints(contactInfo.Transform.position);
                    _eventBus.Invoke(new Draw());
                }
                else
                {
                    _currentLine.Clear();
                }
                MouseUpHandler();
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
            _eventBus.Invoke(new EndDraw());
        }

        private void ResetDrawer()
        {
            _currentLine = null;
            _currentRoute = null;
        }
    }
}