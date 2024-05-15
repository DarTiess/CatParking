using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class UserInput : MonoBehaviour
    {
        private bool _isMouseDown;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isMouseDown = true;
                _eventBus.Invoke(new MouseDown());
            }

            if (_isMouseDown)
            {
                _eventBus.Invoke(new MouseMove());
            }

            if (Input.GetMouseButtonUp(0))
            {
                _isMouseDown = false;
                _eventBus.Invoke(new MouseUp());
            }
        }
    }
}