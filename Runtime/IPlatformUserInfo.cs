namespace Radish.PlatformAPI
{
    public interface IPlatformUserInfo : IOptionalAPI
    {
        string localUserDisplayName { get; }
    }
    
    internal class NullPlatformUserInfoImpl : IPlatformUserInfo
    {
        public bool isSupported => false;
        public string localUserDisplayName { get; }
    }
}