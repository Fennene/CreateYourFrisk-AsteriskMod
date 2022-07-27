using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace AsteriskMod.GlobalScript
{
    public class DynamicEnums
    {
        private static Dictionary<string, Type> _rawEnums;
        private static Dictionary<string, DynValue> _enums;

        private static bool TryGetEnumString(string fullPath, out string[] enumData)
        {
            enumData = new string[0];

            string[] fileData;
            try   { fileData = File.ReadAllLines(fullPath); }
            catch { return false; }

            List<string> enumNames = new List<string>(fileData.Length);
            for (var i = 0; i < fileData.Length; i++)
            {
                string line = fileData[i].Trim();
                string name = "";
                bool isNameDefined = false;
                for (var j = 0; j < line.Length; j++)
                {
                    char c = line[j];
                    if (c == ' ' || c == ',')
                    {
                        isNameDefined = true;
                    }
                    else if (c == '/')
                    {
                        if (j != line.Length - 1 && line[j + 1] == '/')
                        {
                            break;
                        }
                        name += c;
                        break;
                    }
                    else if (!isNameDefined)
                    {
                        name += c;
                    }
                    else
                    {
                        name += "*";
                        break;
                    }
                }
                if (name.IsNullOrWhiteSpace()) continue;
                if (!AsteriskUtil.IsValidVariableName(name)) continue;
                enumNames.Add(name);
            }

            if (enumNames.Count == 0) return false;
            enumData = enumNames.ToArray();

            return true;
        }

        private static Type CreateEnum(string fileName, string[] enumData)
        {
            string modDirName = AsteriskUtil.ConvertToVariableName(StaticInits.MODFOLDER);
            AssemblyName assemblyName = new AssemblyName { Name = modDirName };
            System.AppDomain domain = System.AppDomain.CurrentDomain;
            AssemblyBuilder assembly = domain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
            ModuleBuilder module = assembly.DefineDynamicModule("GlobalEnums");
            EnumBuilder enumBuilder = module.DefineEnum(modDirName + ".GlobalEnums." + fileName , TypeAttributes.Public, typeof(int));
            for (var i = 0; i < enumData.Length; i++)
            {
                enumBuilder.DefineLiteral(enumData[i], i + 1);
            }
            return enumBuilder.CreateType();
        }

        internal static void Load()
        {
            string enumDirPath = FileLoader.pathToModFile("Lua/Globals/Enums");

            if (!Directory.Exists(enumDirPath) || !AsteriskEngine.LuaCodeStyle.loadGlobalEnum)
            {
                Debug.Log("AsteriskMod.DynamicEnums - Globals/Enums enumFiles\nNo Enum Files.\n");

                _rawEnums = new Dictionary<string, Type>(0);
                _enums = new Dictionary<string, DynValue>(0);
            }
            else
            {
                string[] enumFiles = AsteriskUtil.GetFilesWithoutExtension(enumDirPath, "", "*.enum").ToArray();

                string logText = "AsteriskMod.DynamicEnums - Globals/Enums enumFiles";
                for (var i = 0; i < enumFiles.Length; i++)
                {
                    logText += "\n" + enumFiles[i] + ".lua";
                }
                Debug.Log(logText + "\n");

                _rawEnums = new Dictionary<string, Type>(enumFiles.Length);
                _enums = new Dictionary<string, DynValue>(enumFiles.Length);
                for (var i = 0; i < enumFiles.Length; i++)
                {
                    string[] enumData;
                    if (!TryGetEnumString(FileLoader.pathToModFile("Lua/Globals/Enums/" + enumFiles[i] + ".enum"), out enumData)) continue;
                    Type dynamicEnum = CreateEnum(enumFiles[i], enumData);
                    _rawEnums.Add(enumFiles[i], dynamicEnum);
                    UserData.RegisterType(dynamicEnum);
                    _enums.Add(enumFiles[i], UserData.CreateStatic(dynamicEnum));
                }
            }
        }

        public bool Exists(string fileName)
        {
            if (fileName.IsNullOrWhiteSpace()) throw new CYFException("Enums' index shouldn't be nil, empty string or white space string.");
            return _enums.ContainsKey(fileName);
        }

        public DynValue Get(string fileName)
        {
            if (!Exists(fileName)) throw new CYFException("Enum File Globals/Enums/" + fileName + ".enum is not found.");
            return _enums[fileName];
        }
        public DynValue this[string fileName] { get { return Get(fileName); } }
    }
}
