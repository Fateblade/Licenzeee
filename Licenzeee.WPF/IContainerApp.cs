using Prism.Ioc;

namespace Fateblade.Licenzeee.WPF
{
    internal interface IContainerApp 
    {
        IContainerProvider Container { get; }
    }
}
