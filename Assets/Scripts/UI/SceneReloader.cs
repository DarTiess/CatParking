using Infrastructure.Level.EventsBus;
using Infrastructure.Level.EventsBus.Signals;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
