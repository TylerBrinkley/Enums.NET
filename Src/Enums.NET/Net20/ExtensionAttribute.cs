namespace System.Runtime.CompilerServices
{
#if NET20
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
    internal class ExtensionAttribute : Attribute
    {
    }
#endif
}
