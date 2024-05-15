using UnityEngine;
using Zenject;

namespace Infrastructure.Installers
{
    [CreateAssetMenu(fileName = "SettingsInstaller", menuName = "Installers/SettingsInstaller")]
    public class SettingsInstaller : ScriptableObjectInstaller<SettingsInstaller>
    {
        public CatSettings CatSettings;
        public override void InstallBindings()
        {
            Container.BindInstance(CatSettings);
        }
    }
}