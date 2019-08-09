﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_ParticleSystem_ColorOverLifetimeModule : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.ColorOverLifetimeModule o;
			o=new UnityEngine.ParticleSystem.ColorOverLifetimeModule();
			pushValue(l,o);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_enabled(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.ColorOverLifetimeModule self;
			checkValueType(l,1,out self);
			pushValue(l,self.enabled);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_enabled(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.ColorOverLifetimeModule self;
			checkValueType(l,1,out self);
			bool v;
			checkType(l,2,out v);
			self.enabled=v;
			setBack(l,self);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_color(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.ColorOverLifetimeModule self;
			checkValueType(l,1,out self);
			pushValue(l,self.color);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_color(IntPtr l) {
		try {
			UnityEngine.ParticleSystem.ColorOverLifetimeModule self;
			checkValueType(l,1,out self);
			UnityEngine.ParticleSystem.MinMaxGradient v;
			checkValueType(l,2,out v);
			self.color=v;
			setBack(l,self);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.ParticleSystem.ColorOverLifetimeModule");
		addMember(l,"enabled",get_enabled,set_enabled,true);
		addMember(l,"color",get_color,set_color,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.ParticleSystem.ColorOverLifetimeModule),typeof(System.ValueType));
	}
}