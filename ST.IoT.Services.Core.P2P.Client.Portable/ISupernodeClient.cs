namespace ST.IoT.Services.Core.P2P.Client.Portable
{
    public interface ISupernodeClient
    {
        void Announce(string msg);
    }
}