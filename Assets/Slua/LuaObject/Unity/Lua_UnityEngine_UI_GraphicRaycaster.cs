﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_UI_GraphicRaycaster : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Raycast(IntPtr l) {
		try {
			UnityEngine.UI.GraphicRaycaster self=(UnityEngine.UI.GraphicRaycaster)checkSelf(l);
			UnityEngine.EventSystems.PointerEventData a1;
			checkType(l,2,out a1);
			System.Collections.Generic.List<UnityEngine.EventSystems.RaycastResult> a2;
			checkType(l,3,out a2);
			self.Raycast(a1,a2);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_sortOrderPriority(IntPtr l) {
		try {
			UnityEngine.UI.GraphicRaycaster self=(UnityEngine.UI.GraphicRaycaster)checkSelf(l);
			pushValue(l,self.sortOrderPriority);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_renderOrderPriority(IntPtr l) {
		try {
			UnityEngine.UI.GraphicRaycaster self=(UnityEngine.UI.GraphicRaycaster)checkSelf(l);
			pushValue(l,self.renderOrderPriority);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_ignoreReversedGraphics(IntPtr l) {
		try {
			UnityEngine.UI.GraphicRaycaster self=(UnityEngine.UI.GraphicRaycaster)checkSelf(l);
			pushValue(l,self.ignoreReversedGraphics);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_ignoreReversedGraphics(IntPtr l) {
		try {
			UnityEngine.UI.GraphicRaycaster self=(UnityEngine.UI.GraphicRaycaster)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.ignoreReversedGraphics=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_blockingObjects(IntPtr l) {
		try {
			UnityEngine.UI.GraphicRaycaster self=(UnityEngine.UI.GraphicRaycaster)checkSelf(l);
			pushEnum(l,(int)self.blockingObjects);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_blockingObjects(IntPtr l) {
		try {
			UnityEngine.UI.GraphicRaycaster self=(UnityEngine.UI.GraphicRaycaster)checkSelf(l);
			UnityEngine.UI.GraphicRaycaster.BlockingObjects v;
			checkEnum(l,2,out v);
			self.blockingObjects=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_eventCamera(IntPtr l) {
		try {
			UnityEngine.UI.GraphicRaycaster self=(UnityEngine.UI.GraphicRaycaster)checkSelf(l);
			pushValue(l,self.eventCamera);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.UI.GraphicRaycaster");
		addMember(l,Raycast);
		addMember(l,"sortOrderPriority",get_sortOrderPriority,null,true);
		addMember(l,"renderOrderPriority",get_renderOrderPriority,null,true);
		addMember(l,"ignoreReversedGraphics",get_ignoreReversedGraphics,set_ignoreReversedGraphics,true);
		addMember(l,"blockingObjects",get_blockingObjects,set_blockingObjects,true);
		addMember(l,"eventCamera",get_eventCamera,null,true);
		createTypeMetatable(l,null, typeof(UnityEngine.UI.GraphicRaycaster),typeof(UnityEngine.EventSystems.BaseRaycaster));
	}
}
