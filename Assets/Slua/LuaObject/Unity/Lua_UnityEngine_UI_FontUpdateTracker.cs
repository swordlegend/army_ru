﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_UI_FontUpdateTracker : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TrackText_s(IntPtr l) {
		try {
			UnityEngine.UI.Text a1;
			checkType(l,1,out a1);
			UnityEngine.UI.FontUpdateTracker.TrackText(a1);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UntrackText_s(IntPtr l) {
		try {
			UnityEngine.UI.Text a1;
			checkType(l,1,out a1);
			UnityEngine.UI.FontUpdateTracker.UntrackText(a1);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.UI.FontUpdateTracker");
		addMember(l,TrackText_s);
		addMember(l,UntrackText_s);
		createTypeMetatable(l,null, typeof(UnityEngine.UI.FontUpdateTracker));
	}
}
