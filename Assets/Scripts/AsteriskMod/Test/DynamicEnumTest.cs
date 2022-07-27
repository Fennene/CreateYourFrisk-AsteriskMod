using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace AsteriskMod
{
    internal static class DynamicEnumTest
    {
        internal static void StaticTest(Script script)
        {
            UserData.RegisterType<TestEnum>();
            DynValue testEnum = UserData.CreateStatic<TestEnum>();
            script.Globals.Set("Enum", testEnum);
        }

        internal static void DynamicTest(Script script)
        {
            AssemblyName assemblyName = new AssemblyName { Name = StaticInits.MODFOLDER };
            System.AppDomain domain = System.AppDomain.CurrentDomain;
            AssemblyBuilder assembly = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assembly.DefineDynamicModule("FileName");
            EnumBuilder enumBuilder = module.DefineEnum("TestEnum", TypeAttributes.Public, typeof(int));

            string[] _ = new[] { "A", "B", "C", "D" };
            for (var i = 0; i < _.Length; i++)
            {
                enumBuilder.DefineLiteral(_[i], i + 1);
            }

            System.Type enumType = enumBuilder.CreateType();
            UserData.RegisterType(enumType);
            DynValue testEnum = UserData.CreateStatic(enumType);
            script.Globals.Set("Enum", testEnum);
        }
    }
}
