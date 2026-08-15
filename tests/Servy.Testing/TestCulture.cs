using System.Globalization;
using System.Runtime.CompilerServices;

namespace Servy.Testing
{
    /// <summary>
    /// Pins the test process culture/UI-culture to en-US so that tests which assert
    /// on resource strings remain deterministic regardless of the host OS region.
    /// Production resource fallback (zh-CN satellite) is unaffected.
    /// </summary>
    internal static class TestCulture
    {
#pragma warning disable CA2255 // The ModuleInitializer attribute should not be used in libraries
        [ModuleInitializer]
        public static void Init()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
        }
#pragma warning restore CA2255
    }
}
