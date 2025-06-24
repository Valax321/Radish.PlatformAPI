using JetBrains.Annotations;
using Radish.PlatformAPI.DefaultAPIs;
using UnityEngine;

namespace Radish.PlatformAPI
{
    [PublicAPI]
    public sealed class StandalonePlatformSubsystem : IPlatformSubsystem
    {
        public string name => nameof(StandalonePlatformSubsystem);

        public IPlatformSaveData saveData { get; } = new PlatformSaveDataImplFileIO(
            Application.persistentDataPath,
            "local",
            "local"
        );

        public bool Initialize() => true;

        public void Update()
        {
        }
    }
}