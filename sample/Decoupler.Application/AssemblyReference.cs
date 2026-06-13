using System.Reflection;

namespace Decoupler.Application;

internal static class AssemblyReference
{
    public static Assembly GetAssembly() => typeof(AssemblyReference).Assembly;
}