using System;
using MoonSharp.Interpreter;
using UnityEngine;

namespace AsteriskMod.Lua
{
    public class Global
    {
        public DynValue this[string key]
        {
            get { return LuaScriptBinder.GetBattle(null, key); }
            set { LuaScriptBinder.SetBattle(null, key, value); }
        }

        public DynValue Call(string function, DynValue[] args = null, bool checkExist = false) { return Call(null, function, args, checkExist); }

        public DynValue Call(string function, DynValue arg) { return Call(null, function, new[] { arg }); }

        public DynValue Call(Script caller, string function, DynValue[] args = null, bool checkExist = false)
        {
            DynValue func =  LuaScriptBinder.GetFunction(caller, function);
            if (func == null)
            {
                if (checkExist && !GlobalControls.retroMode)
                    UnitaleUtil.DisplayLuaError("Global.Call", "Attempted to call the function \"" + function + "\", but it didn't exist.");
                return DynValue.Nil;
            }
            if (args != null)
            {
                DynValue d = DynValue.Nil;
                try { d = func.Function.Call(args); }
                catch (Exception e)
                {
                    if (args[0].Type == DataType.Table && args.Length == 1)
                    {
                        DynValue[] argsNew = UnitaleUtil.TableToDynValueArray(args[0].Table);
                        try { d = func.Function.Call(argsNew); }
                        catch (InterpreterException ex)
                        {
                            UnitaleUtil.DisplayLuaError("Global.Call", ex.DecoratedMessage == null ?
                                                                        ex.Message :
                                                                        UnitaleUtil.FormatErrorSource(ex.DecoratedMessage, ex.Message) + ex.Message,
                                                                    ex.DoNotDecorateMessage);
                        }
                        catch (Exception ex)
                        {
                            if (!GlobalControls.retroMode)
                                UnitaleUtil.DisplayLuaError("Global.Call: calling the function " + function, "This is a " + ex.GetType() + " error. Contact a developer and show them this screen, this must be an engine-side error.\n\n" + ex.Message + "\n\n" + ex.StackTrace + "\n");
                        }
                    }
                    else if (e.GetType() == typeof(InterpreterException) || e.GetType().BaseType == typeof(InterpreterException) || e.GetType().BaseType.BaseType == typeof(InterpreterException))
                    {
                        UnitaleUtil.DisplayLuaError("Global.Call", ((InterpreterException)e).DecoratedMessage == null ?
                                                                ((InterpreterException)e).Message :
                                                                UnitaleUtil.FormatErrorSource(((InterpreterException)e).DecoratedMessage, ((InterpreterException)e).Message) + ((InterpreterException)e).Message,
                                                                ((InterpreterException)e).DoNotDecorateMessage);
                    }
                    else if (!GlobalControls.retroMode)
                        UnitaleUtil.DisplayLuaError("Global.Call: calling the function " + function, "This is a " + e.GetType() + " error. Contact a dev and show them this screen, this must be an engine-side error.\n\n" + e.Message + "\n\n" + e.StackTrace + "\n");
                }
                return d;
            }
            else
            {
                DynValue d = DynValue.Nil;
                try { d = func.Function.Call(args); }
                catch (InterpreterException ex)
                {
                    UnitaleUtil.DisplayLuaError("Global.Call", ex.DecoratedMessage == null ?
                                                                ex.Message :
                                                                UnitaleUtil.FormatErrorSource(ex.DecoratedMessage, ex.Message) + ex.Message,
                                                            ex.DoNotDecorateMessage);
                }
                catch (Exception ex)
                {
                    if (GlobalControls.retroMode) return d;
                    if (ex.GetType().ToString() == "System.IndexOutOfRangeException" && ex.StackTrace.StartsWith("  at (wrapper stelemref) object:stelemref (object,intptr,object)"
                                                                                                               + "\r\n  at MoonSharp.Interpreter.DataStructs.FastStack`1[MoonSharp.Interpreter.DynValue].Push"))
                        UnitaleUtil.DisplayLuaError("Global.Call: calling the function " + function, "<b>Possible infinite loop</b>\n\nThis is a " + ex.GetType() + " error.\n\n"
                                                                                                     + "You almost definitely have an infinite loop in your code. A function tried to call itself infinitely. It could be a normal function or a metatable function."
                                                                                                     + "\n\n\nFull stracktrace (see CYF output log at <b>" + Application.persistentDataPath + "/output_log.txt</b>):\n\n" + ex.StackTrace);
                    else
                        UnitaleUtil.DisplayLuaError("Global.Call: calling the function " + function, "This is a " + ex.GetType() + " error. Contact a dev and show them this screen, this must be an engine-side error.\n\n" + ex.Message + "\n\n" + ex.StackTrace + "\n");
                }
                return d;
            }
        }
    }
}
