﻿using UnityEngine;
using System;
using LuaInterface;
using SLua;
using System.Collections.Generic;
public class Lua_UnityEngine_AnimationCurve : LuaObject {
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int constructor(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			UnityEngine.AnimationCurve o;
			if(argc==2){
				UnityEngine.Keyframe[] a1;
				checkValueParams(l,2,out a1);
				o=new UnityEngine.AnimationCurve(a1);
				pushValue(l,o);
				return 1;
			}
			else if(argc==1){
				o=new UnityEngine.AnimationCurve();
				pushValue(l,o);
				return 1;
			}
			return error(l,"New object failed.");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Evaluate(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Single a1;
			checkType(l,2,out a1);
			var ret=self.Evaluate(a1);
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int AddKey(IntPtr l) {
		try {
			int argc = LuaDLL.lua_gettop(l);
			if(argc==2){
				UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
				UnityEngine.Keyframe a1;
				checkValueType(l,2,out a1);
				var ret=self.AddKey(a1);
				pushValue(l,ret);
				return 1;
			}
			else if(argc==3){
				UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
				System.Single a1;
				checkType(l,2,out a1);
				System.Single a2;
				checkType(l,3,out a2);
				var ret=self.AddKey(a1,a2);
				pushValue(l,ret);
				return 1;
			}
			return error(l,"No matched override function to call");
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int MoveKey(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			UnityEngine.Keyframe a2;
			checkValueType(l,3,out a2);
			var ret=self.MoveKey(a1,a2);
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int RemoveKey(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			self.RemoveKey(a1);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int SmoothTangents(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			System.Int32 a1;
			checkType(l,2,out a1);
			System.Single a2;
			checkType(l,3,out a2);
			self.SmoothTangents(a1,a2);
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Constant_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			var ret=UnityEngine.AnimationCurve.Constant(a1,a2,a3);
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int Linear_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=UnityEngine.AnimationCurve.Linear(a1,a2,a3,a4);
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int EaseInOut_s(IntPtr l) {
		try {
			System.Single a1;
			checkType(l,1,out a1);
			System.Single a2;
			checkType(l,2,out a2);
			System.Single a3;
			checkType(l,3,out a3);
			System.Single a4;
			checkType(l,4,out a4);
			var ret=UnityEngine.AnimationCurve.EaseInOut(a1,a2,a3,a4);
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_keys(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			pushValue(l,self.keys);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_keys(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			UnityEngine.Keyframe[] v;
			checkType(l,2,out v);
			self.keys=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_length(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			pushValue(l,self.length);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_preWrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			pushEnum(l,(int)self.preWrapMode);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_preWrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			UnityEngine.WrapMode v;
			checkEnum(l,2,out v);
			self.preWrapMode=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int get_postWrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			pushEnum(l,(int)self.postWrapMode);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int set_postWrapMode(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			UnityEngine.WrapMode v;
			checkEnum(l,2,out v);
			self.postWrapMode=v;
			return 0;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static public int getItem(IntPtr l) {
		try {
			UnityEngine.AnimationCurve self=(UnityEngine.AnimationCurve)checkSelf(l);
			int v;
			checkType(l,2,out v);
			var ret = self[v];
			pushValue(l,ret);
			return 1;
		}
		catch(Exception e) {
			return error(l,e);
		}
	}
	static public void reg(IntPtr l) {
		getTypeTable(l,"UnityEngine.AnimationCurve");
		addMember(l,Evaluate);
		addMember(l,AddKey);
		addMember(l,MoveKey);
		addMember(l,RemoveKey);
		addMember(l,SmoothTangents);
		addMember(l,Constant_s);
		addMember(l,Linear_s);
		addMember(l,EaseInOut_s);
		addMember(l,getItem);
		addMember(l,"keys",get_keys,set_keys,true);
		addMember(l,"length",get_length,null,true);
		addMember(l,"preWrapMode",get_preWrapMode,set_preWrapMode,true);
		addMember(l,"postWrapMode",get_postWrapMode,set_postWrapMode,true);
		createTypeMetatable(l,constructor, typeof(UnityEngine.AnimationCurve));
	}
}
