using System.ComponentModel;



namespace UO_Atlas
{
    public delegate void StatusChangedEventHandler(int percentOfProgress, string statusMessage);

    public interface IStatusProvider
    {
        event StatusChangedEventHandler StatusChanged;
    }
}