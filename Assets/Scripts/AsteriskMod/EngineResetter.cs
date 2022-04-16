namespace AsteriskMod
{
    public class EngineResetter
    {
        public static void Initialize()
        {
            CYFEngine.Initialize();
            UIController.InitalizeButtonManager();
        }

        public static void Revert()
        {
            CYFEngine.Reset();
        }
    }
}
