using StreamIntegrationApp.API;

namespace TerrariaActions
{
    public class TerrariaPatch: CSharpPatch
    {
        public override string TargetAssembly()
        {
            return "Terraria.exe";
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