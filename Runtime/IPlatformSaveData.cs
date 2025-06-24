using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

namespace Radish.PlatformAPI
{
    [PublicAPI]
    public interface IPlatformSaveData : IOptionalAPI
    {
        public delegate void SaveDataDynamicChangeDelegate(IReadOnlyList<FileChange> changedFiles);

        public class FileChange
        {
            public string name { get; }
            public ChangeType type { get; }

            public FileChange(string name, ChangeType type)
            {
                this.name = name;
                this.type = type;
            }
        }

        public enum ChangeType
        {
            Updated,
            Deleted
        }
        
        [Flags, PublicAPI]
        public enum OpenMode
        {
            Read = 1 << 0,
            Write = 1 << 1,
        }
        
        #region Cloud Sync

        public event SaveDataDynamicChangeDelegate onSaveDataChanged;
        
        #endregion

        #region User Data

        /// <summary>
        /// Notifies the subsystem that we're about to begin writing user data.
        /// Be sure to call <see cref="EndDataWrite"/> when all required files have been written.
        /// </summary>
        bool BeginDataWrite();

        /// <summary>
        /// Notifies the subsystem that we're done writing user data.
        /// <seealso cref="BeginDataWrite"/>
        /// </summary>
        bool EndDataWrite();

        /// <summary>
        /// Opens a <see cref="Stream"/> for save data that will be saved to the cloud if cloud save functionality is supported
        /// on the current platform.
        /// <remarks>Note: this will not necessarily be a file on disk!
        /// A file with the given name might not actually exist anywhere and should just be treated as a key into a table.</remarks>
        /// </summary>
        /// <param name="name">The name (with extension) of the data to be saved.</param>
        /// <param name="mode">Flags controlling whether the data should be readable or writable.
        /// Both <see cref="OpenMode.Read"/> and <see cref="OpenMode.Write"/> can be ANDed to allow read and write at the same time.
        /// If neither mode is specified, an exception will be thrown.</param>
        /// <returns>A <see cref="Stream"/> representing the data.</returns>
        [CanBeNull]
        Stream OpenDataStream(string name, OpenMode mode);

        /// <summary>
        /// Checks if user data with the given name exists.
        /// </summary>
        /// <param name="name">The name of the data to check for.</param>
        /// <returns>True if data was found, otherwise false.</returns>
        bool DoesDataExist(string name);
        
        /// <summary>
        /// Deletes user data with the given name if it exists.
        /// </summary>
        /// <param name="name">The name of the data to delete.</param>
        void DeleteData(string name);

        /// <summary>
        /// Gets the names of all user data.
        /// </summary>
        /// <returns>Enumerable user data names.</returns>
        IEnumerable<string> GetDataNames();

        #endregion
    }
    
    internal class NullPlatformSaveDataImpl : IPlatformSaveData
    {
        public bool isSupported => false;

        public event IPlatformSaveData.SaveDataDynamicChangeDelegate onSaveDataChanged;

        public bool BeginDataWrite()
        {
            throw new NotImplementedException();
        }

        public bool EndDataWrite()
        {
            throw new NotImplementedException();
        }

        public Stream OpenDataStream(string name, IPlatformSaveData.OpenMode mode)
        {
            throw new NotImplementedException();
        }

        public bool DoesDataExist(string name)
        {
            throw new NotImplementedException();
        }

        public void DeleteData(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetDataNames()
        {
            throw new NotImplementedException();
        }
    }
}