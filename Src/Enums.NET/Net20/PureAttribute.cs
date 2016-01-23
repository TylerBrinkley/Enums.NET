namespace System.Diagnostics.Contracts
{
#if NET20 || NET35
    [Conditional("CONTRACTS_FULL")]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate)]
    internal sealed class PureAttribute : Attribute
    {
    }
#endif
}
