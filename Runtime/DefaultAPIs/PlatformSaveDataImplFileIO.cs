using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Radish.Logging;

namespace Radish.PlatformAPI.DefaultAPIs
{
    /// <summary>
    /// A default implementation for reading and writing save data to the local OS user account's storage area.
    /// This will work at least on all desktop and mobile operating systems.
    /// Useful if shipping a non-steam build of something.
    /// </summary>
    [PublicAPI]
    public sealed class PlatformSaveDataImplFileIO : IPlatformSaveData
    {
        private static readonly ILogger Logger = LogManager.GetLoggerForType(typeof(PlatformSaveDataImplFileIO));
        
        private readonly string m_UserPath;
        
        public PlatformSaveDataImplFileIO(string rootDataPath, string userDirName)
        {
            m_UserPath = Path.Combine(rootDataPath, userDirName);

            try
            {
                Directory.CreateDirectory(m_UserPath);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Failed to set up data directories");
            }
        }

#pragma warning disable CS0067
        public event IPlatformSaveData.SaveDataDynamicChangeDelegate OnSaveDataChanged;
#pragma warning restore CS0067

        public bool BeginDataWrite()
        {
            return true;
        }

        public bool EndDataWrite()
        {
            return true;
        }

        public Stream OpenDataStream(string name, IPlatformSaveData.OpenMode mode)
        {
            FileAccess access = 0;
            var fm = FileMode.Open;
            if (mode.HasFlagT(IPlatformSaveData.OpenMode.Read))
                access = FileAccess.Read;
            if (mode.HasFlagT(IPlatformSaveData.OpenMode.Write))
            {
                access = access == FileAccess.Read ? FileAccess.ReadWrite : FileAccess.Write;
                fm = FileMode.OpenOrCreate; // If we're writing we need to create the file if it's not there
            }

            if (access == 0)
                throw new IOException($"{nameof(mode)} must be either Read, Write or both");

            try
            {
                var f = File.Open(Path.Combine(m_UserPath, name), fm, access);
                return f;
            }
            catch (Exception ex)
            {
                Logger.Warn("Failed to open '{0}': {1}", name, ex.Message);
                return null;
            }
        }

        public bool DoesDataExist(string name)
        {
            return File.Exists(Path.Combine(m_UserPath, name));
        }

        public void DeleteData(string name)
        {
            try
            {
                File.Delete(Path.Combine(m_UserPath, name));
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Failed to delete '{0}'", name);
            }
        }

        public IEnumerable<string> GetDataNames()
        {
            try
            {
                return Directory.EnumerateFiles(m_UserPath);
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Failed to get data names");
                return Enumerable.Empty<string>();
            }
        }

        public bool isSupported => true;
    }
}