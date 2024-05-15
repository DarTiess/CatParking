using Infrastructure.EventsBus;
using Infrastructure.EventsBus.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
   public class SceneReloader : MonoBehaviour
   {
      private Button _button;
      private IEventBus _eventBus;

      [Inject]
      public void Construct(IEventBus eventBus)
      {
         _eventBus = eventBus;
      }

      private void Start()
      {
         _button = GetComponent<Button>();
         _button.onClick.AddListener(RestartScene);
      }

      private void RestartScene()
      {
         _eventBus.Invoke(new RestartScene());
      }
   }
}
