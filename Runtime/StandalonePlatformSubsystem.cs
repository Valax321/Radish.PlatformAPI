using JetBrains.Annotations;
using Radish.PlatformAPI.DefaultAPIs;
using UnityEngine;

namespace Radish.PlatformAPI
{
    [PublicAPI]
    public sealed class StandalonePlatformSubsystem : IPlatformSubsystem
    {
        public string name => nameof(StandalonePlatformSubsystem);

        public IPlatformSaveData userData => m_SaveDataImpl;
        public IPlatformSaveData localData => m_SaveDataImpl;
        
        // On standalone platforms the persistentDataPath is already in a user folder, so we can
        // share the same path for both user and local data.
        private IPlatformSaveData m_SaveDataImpl = new PlatformSaveDataImplFileIO(
            Application.persistentDataPath,
            "local"
        );

        public bool Initialize() => true;

        public void Update()
        {
        }
    }
}