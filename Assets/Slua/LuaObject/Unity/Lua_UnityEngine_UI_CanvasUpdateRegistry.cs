﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_UI_CanvasUpdateRegistry : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterCanvasElementForLayoutRebuild_s(IntPtr l) {
		try {
			UnityEngine.UI.ICanvasElement a1;
			checkType(l,1,out a1);
			UnityEngine.UI.CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild(a1);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryRegisterCanvasElementForLayoutRebuild_s(IntPtr l) {
		try {
			UnityEngine.UI.ICanvasElement a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.UI.CanvasUpdateRegistry.TryRegisterCanvasElementForLayoutRebuild(a1);
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RegisterCanvasElementForGraphicRebuild_s(IntPtr l) {
		try {
			UnityEngine.UI.ICanvasElement a1;
			checkType(l,1,out a1);
			UnityEngine.UI.CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild(a1);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int TryRegisterCanvasElementForGraphicRebuild_s(IntPtr l) {
		try {
			UnityEngine.UI.ICanvasElement a1;
			checkType(l,1,out a1);
			var ret=UnityEngine.UI.CanvasUpdateRegistry.TryRegisterCanvasElementForGraphicRebuild(a1);
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int UnRegisterCanvasElementForRebuild_s(IntPtr l) {
		try {
			UnityEngine.UI.ICanvasElement a1;
			checkType(l,1,out a1);
			UnityEngine.UI.CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild(a1);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsRebuildingLayout_s(IntPtr l) {
		try {
			var ret=UnityEngine.UI.CanvasUpdateRegistry.IsRebuildingLayout();
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int IsRebuildingGraphics_s(IntPtr l) {
		try {
			var ret=UnityEngine.UI.CanvasUpdateRegistry.IsRebuildingGraphics();
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_instance(IntPtr l) {
		try {
			pushValue(l,UnityEngine.UI.CanvasUpdateRegistry.instance);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.UI.CanvasUpdateRegistry");
		addMember(l,RegisterCanvasElementForLayoutRebuild_s);
		addMember(l,TryRegisterCanvasElementForLayoutRebuild_s);
		addMember(l,RegisterCanvasElementForGraphicRebuild_s);
		addMember(l,TryRegisterCanvasElementForGraphicRebuild_s);
		addMember(l,UnRegisterCanvasElementForRebuild_s);
		addMember(l,IsRebuildingLayout_s);
		addMember(l,IsRebuildingGraphics_s);
		addMember(l,"instance",get_instance,null,false);
		createTypeMetatable(l,null, typeof(UnityEngine.UI.CanvasUpdateRegistry));
	}
}
