using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteriskMod.ModdingHelperTools
{
    internal static class SimInstance
    {
        internal static FakeStaticInits FakeStaticInits;
        internal static FakeFileLoader FakeFileLoader;

        internal static void Initialize()
        {
            FakeStaticInits = new FakeStaticInits();
            FakeFileLoader = new FakeFileLoader();
        }

        internal static BattleSimulator BattleSimulator;

        internal static FakeUnitaleUtil FakeUnitaleUtil;

        internal static FakeSpriteFontRegistry FakeSpriteFontRegistry;
        internal static FakeSpriteRegistry FakeSpriteRegistry;

        internal static SPCreateUI SPCreateUI;
        internal static SPTargetDelUI SPTargetDelUI;
        internal static SPControllerUI SPControllerUI;

        internal static void Prepare()
        {
            BattleSimulator = new BattleSimulator();

            FakeUnitaleUtil = new FakeUnitaleUtil();

            FakeSpriteFontRegistry = new FakeSpriteFontRegistry();
            FakeSpriteRegistry = new FakeSpriteRegistry();

            SPCreateUI = new SPCreateUI();
            SPTargetDelUI = new SPTargetDelUI();
            SPControllerUI = new SPControllerUI();
        }

        internal static void Dispose()
        {
            SimMenuMover.Dispose();
            SimMenuOpener.Dispose();
            SimMenuWindowManager.Dispose();

            SPControllerUI = null;
            SPTargetDelUI = null;
            SPCreateUI = null;
            SimSprProjSimMenu.Dispose();

            AnimFrameCounter.Dispose();

            FakeArenaUtil.Dispose();

            FakeSpriteRegistry = null;
            FakeSpriteFontRegistry = null;

            FakeUnitaleUtil = null;

            BattleSimulator = null;

            FakeFileLoader = null;
            FakeStaticInits = null;
        }
    }
}
