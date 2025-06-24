using System.IO;
using JetBrains.Annotations;
using Radish.PlatformAPI.DefaultAPIs;

namespace Radish.PlatformAPI
{
    [PublicAPI]
    public class EditorPlatformSubsystem : IPlatformSubsystem
    {
        public string name => nameof(EditorPlatformSubsystem);
        
        public IPlatformSaveData userData { get; }
        public IPlatformSaveData localData { get; }

        public EditorPlatformSubsystem()
        {
            userData = new PlatformSaveDataImplFileIO(Path.Combine(Directory.GetCurrentDirectory(), "SaveData"),
                "user");
            localData = new PlatformSaveDataImplFileIO(Path.Combine(Directory.GetCurrentDirectory(), "SaveData"),
                "local");
        }

        public bool Initialize() => true;

        public void Update()
        {
        }
    }
}