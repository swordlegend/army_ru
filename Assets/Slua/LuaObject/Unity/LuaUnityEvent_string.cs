﻿
using System;
using System.Collections.Generic;
using LuaInterface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SLua
{
    public class LuaUnityEvent_string : LuaObject
    {

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int AddListener(IntPtr l)
        {
            try
            {
                UnityEngine.Events.UnityEvent<string> self = checkSelf<UnityEngine.Events.UnityEvent<string>>(l);
                UnityEngine.Events.UnityAction<string> a1;
                checkType(l, 2, out a1);
                self.AddListener(a1);
				// pushValue(l,true);
                // return 1;
				return 0;
            }
            catch (Exception e)
            {
				return error(l,e);
            }
        }
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int RemoveListener(IntPtr l)
        {
            try
            {
                UnityEngine.Events.UnityEvent<string> self = checkSelf<UnityEngine.Events.UnityEvent<string>>(l);
                UnityEngine.Events.UnityAction<string> a1;
                checkType(l, 2, out a1);
                self.RemoveListener(a1);
				// pushValue(l,true);
                // return 1;
				return 0;
            }
            catch (Exception e)
            {
                return error(l,e);
            }
        }
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static public int Invoke(IntPtr l)
        {
            try
            {
                UnityEngine.Events.UnityEvent<string> self = checkSelf<UnityEngine.Events.UnityEvent<string>>(l);
                string o;
                checkType(l,2,out o);
                self.Invoke(o);
				// pushValue(l,true);
                // return 1;
				return 0;
            }
            catch (Exception e)
            {
                return error(l,e);
            }
        }
        static public void reg(IntPtr l)
        {
            getTypeTable(l, typeof(LuaUnityEvent_string).FullName);
            addMember(l, AddListener);
            addMember(l, RemoveListener);
            addMember(l, Invoke);
            createTypeMetatable(l, null, typeof(LuaUnityEvent_string), typeof(UnityEngine.Events.UnityEventBase));
        }

        static bool checkType(IntPtr l,int p,out UnityEngine.Events.UnityAction<string> ua) {
            LuaDLL.luaL_checktype(l, p, LuaTypes.LUA_TFUNCTION);
            LuaDelegate ld;
            checkType(l, p, out ld);
            if (ld.d != null)
            {
                ua = (UnityEngine.Events.UnityAction<string>)ld.d;
                return true;
            }
			l = LuaState.get(l).L;
            ua = (string v) =>
            {
                int error = pushTry(l);
                pushValue(l, v);
                ld.pcall(1, error);
                LuaDLL.lua_settop(l,error - 1);
            };
            ld.d = ua;
            return true;
        }
    }
}
