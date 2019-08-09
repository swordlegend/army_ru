﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_UI_Dropdown_OptionDataList : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.UI.Dropdown.OptionDataList o;
			o=new UnityEngine.UI.Dropdown.OptionDataList();
			pushValue(l,o);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_options(IntPtr l) {
		try {
			UnityEngine.UI.Dropdown.OptionDataList self=(UnityEngine.UI.Dropdown.OptionDataList)checkSelf(l);
			pushValue(l,self.options);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_options(IntPtr l) {
		try {
			UnityEngine.UI.Dropdown.OptionDataList self=(UnityEngine.UI.Dropdown.OptionDataList)checkSelf(l);
			List<UnityEngine.UI.Dropdown.OptionData> v;
			checkType(l,2,out v);
			self.options=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.UI.Dropdown.OptionDataList");
		addMember(l,"options",get_options,set_options,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.UI.Dropdown.OptionDataList));
	}
}