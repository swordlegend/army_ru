﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_AnimationClip : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			UnityEngine.AnimationClip o;
			o=new UnityEngine.AnimationClip();
			pushValue(l,o);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SampleAnimation(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			UnityEngine.GameObject a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.SampleAnimation(a1,a2);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SetCurve(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			System.String a1;
			checkType(l,2,out a1);
			System.Type a2;
			checkType(l,3,out a2);
			System.String a3;
			checkType(l,4,out a3);
			UnityEngine.AnimationCurve a4;
			checkType(l,5,out a4);
			self.SetCurve(a1,a2,a3,a4);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EnsureQuaternionContinuity(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			self.EnsureQuaternionContinuity();
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int ClearCurves(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			self.ClearCurves();
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddEvent(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			UnityEngine.AnimationEvent a1;
			checkType(l,2,out a1);
			self.AddEvent(a1);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_length(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			pushValue(l,self.length);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_frameRate(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			pushValue(l,self.frameRate);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_frameRate(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			float v;
			checkType(l,2,out v);
			self.frameRate=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_wrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			pushEnum(l,(int)self.wrapMode);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_wrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			UnityEngine.WrapMode v;
			checkEnum(l,2,out v);
			self.wrapMode=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_localBounds(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			pushValue(l,self.localBounds);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_localBounds(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			UnityEngine.Bounds v;
			checkValueType(l,2,out v);
			self.localBounds=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_legacy(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			pushValue(l,self.legacy);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_legacy(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			bool v;
			checkType(l,2,out v);
			self.legacy=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_humanMotion(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			pushValue(l,self.humanMotion);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_empty(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			pushValue(l,self.empty);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_events(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			pushValue(l,self.events);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_events(IntPtr l) {
		try {
			UnityEngine.AnimationClip self=(UnityEngine.AnimationClip)checkSelf(l);
			UnityEngine.AnimationEvent[] v;
			checkType(l,2,out v);
			self.events=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.AnimationClip");
		addMember(l,SampleAnimation);
		addMember(l,SetCurve);
		addMember(l,EnsureQuaternionContinuity);
		addMember(l,ClearCurves);
		addMember(l,AddEvent);
		addMember(l,"length",get_length,null,true);
		addMember(l,"frameRate",get_frameRate,set_frameRate,true);
		addMember(l,"wrapMode",get_wrapMode,set_wrapMode,true);
		addMember(l,"localBounds",get_localBounds,set_localBounds,true);
		addMember(l,"legacy",get_legacy,set_legacy,true);
		addMember(l,"humanMotion",get_humanMotion,null,true);
		addMember(l,"empty",get_empty,null,true);
		addMember(l,"events",get_events,set_events,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.AnimationClip),typeof(UnityEngine.Motion));
	}
}
