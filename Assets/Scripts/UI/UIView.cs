using System;
using DG.Tweening;
using Game;
using Infrastructure.EventsBus;
using Infrastructure.EventsBus.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIView: MonoBehaviour
    {
        [SerializeField] private CanvasGroup availableLineCanvas;
        [SerializeField] private Image availableLineFill;

        [SerializeField] private Image fadePanel;
        [SerializeField] private float fadeDuration;
        private bool _isActiveLine;
        private Route _activeRoute;
        private IEventBus _eventBus;

        [Inject]
        public void Construct(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<BeginDraw>(OnBeginDrawHandler);
            _eventBus.Subscribe<Draw>(OnDrawHandler);
            _eventBus.Subscribe<EndDraw>(OnEndDrawHandler);
        }

        private void Start()
        {
            fadePanel.DOFade(0, fadeDuration).From(1f)
                     .OnComplete(() =>
                     {
                         fadePanel.gameObject.SetActive(false);
                     });
            availableLineCanvas.alpha = 0f;
        }

        private void OnBeginDrawHandler(BeginDraw obj)
        {
            _activeRoute = obj.Route;
            availableLineFill.color = _activeRoute.Color;
            availableLineFill.fillAmount = 1f;
            FadeLine(1f, 0f);
            _isActiveLine = true;
        }

        private void FadeLine(float to, float from)
        {
            availableLineCanvas.DOFade(to, fadeDuration).From(from);
        }

        private void OnDrawHandler(Draw obj)
        {
            if(!_isActiveLine)
                return;

            float maxLineLenght = _activeRoute.MaxLineLenght;
            float lineLenght = _activeRoute.Line.Lenght;

            availableLineFill.fillAmount = 1 - (lineLenght / maxLineLenght);
        }

        private void OnEndDrawHandler(EndDraw obj)
        {
            if(!_isActiveLine)
                return;

            _isActiveLine = false;
            _activeRoute = null;
            FadeLine(0f,1f);        }
    }
}