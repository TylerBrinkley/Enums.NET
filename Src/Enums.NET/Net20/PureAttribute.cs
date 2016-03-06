#if NET20 || NET35
namespace System.Diagnostics.Contracts
{
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate)]
    internal sealed class PureAttribute : Attribute
    {
    }
}
#endif