using StreamIntegrationApp.API;

namespace TerrariaServerPatch
{
    public class TerrariaServerPatch: CSharpPatch
    {
        public override string TargetAssembly()
        {
            return "TerrariaServer.exe";
        }

        public override string TargetClass()
        {
            return "LaunchInitializer";
        }

        public override string TargetMethod()
        {
            return "LoadParameters";
        }

        public override PatchTarget TargetLocation()
        {
            return PatchTarget.START;
        }
    }
}