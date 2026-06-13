using System.Reflection;

namespace Decoupler.WebApi;

internal static class AssemblyReference
{
    public static Assembly GetAssembly() => typeof(AssemblyReference).Assembly;
}