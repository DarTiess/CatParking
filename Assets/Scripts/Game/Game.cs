using System;
using System.Collections.Generic;
using Infrastructure.EventsBus;
using Infrastructure.EventsBus.Signals;
using Infrastructure.Installers;
using UnityEngine;
using Zenject;

namespace Game
{
    public class Game: MonoBehaviour, IGame
    {
      
        public List<Route> ReadyRoutes => _readyRoutes;
        
        private List<Route> _readyRoutes=new ();
        private int _totalRoutes;
        private CatSettings _catSetting;
        private IEventBus _eventBus;
        private int _catFinished;

        public event Action<Route> OnCatEntersPark; 

        [Inject]
        public void Construct(CatSettings catSettings, IEventBus eventBus )
        {
            _catSetting = catSettings;
            _eventBus = eventBus;

        }

        private void Start()
        {
            var routes = transform.GetComponentsInChildren<Route>();
            _totalRoutes = routes.Length;
            _catFinished = 0;

            for (int i = 0; i < routes.Length; i++)
            {
               routes[i].Initialize(_eventBus, this,_catSetting, i);
               routes[i].CatFinished += CatFinished;
            }
        }

        private void CatFinished()
        {
            _catFinished++;
            if (_catFinished >= _totalRoutes)
            {
                _eventBus.Invoke(new Win());
            }
        }

        public void RegisterRoute(Route route)
        {
            _readyRoutes.Add(route);
            if (_readyRoutes.Count == _totalRoutes)
                MoveAllCats();
        }

        private void MoveAllCats()
        {
            foreach (Route route in _readyRoutes)
            {
                route.Cat.Move(route.LinePoints);
            }
        }
    }
}