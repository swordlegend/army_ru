//
// AppLifecycle.cs
//
// Author:
//       duwenjie
//

//
// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
//
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using System.IO;
using UnityEngine;
using SLua;
using CinemaSuite.Common;

namespace LBoot
{
    /// <summary>
    /// AppLifecycle
    /// </summary>
    [CustomLuaClassAttribute]
    public class AppLifecycle
    {
        public static void BootstrapApp(GameObject initGameObject)
        {
            LogUtil.Debug("== BootstrapApp");
            new BootstrapApp(initGameObject).Bootstrap();
        }

        public static void RestartApp(bool withUpdate)
        {
            LogUtil.Debug("== RestartApp");
            new RestartApp(withUpdate).Restart();
        }

        public static void PauseApp()
        {
            LogUtil.Debug("== PauseApp");
            new PauseApp().Pause();
        }

        public static void ResumeApp()
        {
            LogUtil.Debug("== ResumeApp");
            new ResumeApp().Resume();
        }

        public static void ShutdownApp()
        {
            LogUtil.Debug("== ShutdownApp");
            new ShutdownApp().Shutdown();
        }

        public static void ReceivedMemoryWarning()
        {
            LogUtil.Debug("== ReceivedMemoryWarning");
            new ReceivedMemoryWarning().Notify();
        }
    }

    /// <summary>
    ///
    /// </summary>
    internal class BootstrapApp
    {
        internal GameObject initGameObject = null;

        public BootstrapApp(GameObject initGameObject)
        {
            this.initGameObject = initGameObject;
            LBootApp.initGameObject = initGameObject;
        }

        public void Bootstrap()
        {
            InitUnity();
            PrepareLua(StartLua);
            // StartLua();
        }

        void InitUnity()
        {
            Application.targetFrameRate = LBootApp.TARGET_FRAME_RATE; // not useful if vsync is set
            BundleEncoder.Init(LBootApp.BUNDLE_KEY, LBootApp.BUNDLE_IV);

            if (initGameObject != null)
            {
                /*
				var scheduler = initGameObject.GetComponent<SchedulerBehaviour>();
				if (scheduler != null) {
					GlobalScheduler.Init(scheduler);
				} else {
					LogUtil.Error("BootstrapApp: initUnity failed! no Scheduler component found");
				}
				*/
                var updater = initGameObject.GetComponent<GlobalUpdateBehaviour>();
                if (updater == null)
                {
                    LogUtil.Error("BootstrapApp: initUnity failed! no GlobalUpdateBehaviour component found");
                }
            }
            else
            {
                LogUtil.Error("BootstrapApp: initUnity failed! initGameObject is null");
            }
        }

        public static void PrepareLua(Action callback)
        {
            var luaBridge = new LuaBridge();
            LBootApp.luaBridge = luaBridge;
            luaBridge.init(null, delegate()
                {
                    LoadConfig();
                    if (callback != null)
                    {
                        callback();
                    }
                });
        }

        private static void LoadConfig()
        {
            var luaBridge = LBootApp.luaBridge;

            var configSourceName = "config.lua";
            byte[] configSource = FileUtils.GetDataFromStreamingAssets(configSourceName);

            // load script if in production
            if (Application.isEditor)
            {
                luaBridge.DoBuffer(configSource, configSourceName);
                luaBridge.DoString("SCRIPT = 'debug'; MODE = 'development'; ");
                luaBridge.DoString("PLATFORM = 'editor';");
                luaBridge.DoString("SDK = 'standard';");
            }
            else
            {
                if (Debug.isDebugBuild)
                {
                    luaBridge.DoString("SCRIPT = 'compiled'; MODE = 'development'; ");
                    luaBridge.DoBuffer(configSource, configSourceName);
                }
                else
                {
                    luaBridge.DoBuffer(configSource, configSourceName);
#if UNITY_ANDROID
                     luaBridge.DoString("SCRIPT = 'compiled'; MODE = 'production'; ");
#endif
                }

                #if UNITY_ANDROID && ! UNITY_EDITOR
				luaBridge.DoString("PLATFORM = 'android';");
                #elif UNITY_IPHONE && ! UNITY_EDITOR
				luaBridge.DoString("PLATFORM = 'ios';");
                #elif UNITY_STANDALONE_OSX && ! UNITY_EDITOR
                luaBridge.DoString("PLATFORM = 'osx';");
                #else
                luaBridge.DoString("PLATFORM = 'unknown';");
                #endif
            }

        }

        void StartLua()
        {
            ComponentReflectionExt.Init();
            var luaBridge = LBootApp.luaBridge;
            var runDebugScript = luaBridge.DoString("return rawget(_G, 'SCRIPT') == 'debug'");

            if (true.Equals(runDebugScript))
            {
                LogUtil.Debug("running debug scripts");
                var fname = "debug.lua";
                byte[] scriptSource = FileUtils.GetDataFromStreamingAssets(fname);
                luaBridge.DoBuffer(scriptSource, fname);
            }
            else
            {
                LogUtil.Debug("running compiled scripts");
                var fname = "cl.lc";
                byte[] scriptSource = FileUtils.GetDataFromStreamingAssets(fname);
                luaBridge.DoAesBuffer(scriptSource, fname, LBootApp.LUA_KEY, LBootApp.LUA_IV);
                luaBridge.Call("__main", new object[1] { "" });
            }

// 4 times faster for FastGetter FastSetter than using reflection
//            var go = new GameObject("hello");
//            var t = go.transform;
//            t.position = Vector3.one;
//            var startTime = Time.realtimeSinceStartup;
//            object value = null;
//            for (var i = 0; i < 1000; i++)
//            {
//                var positionProperty = ReflectionHelper.GetProperty(t.GetType(), "position");
//                value = positionProperty.GetValue(t, null);
//            }
//            Debug.LogError("Reflection getter position=" + value.ToString());
//            var time = Time.realtimeSinceStartup;
//            Debug.LogError("reflection getter duration = " + ((time - startTime) * 1000).ToString() + "(ms)");
//
//            startTime = Time.realtimeSinceStartup;
//
//            for (var i = 0; i < 1000; i++)
//            {
//                 value = ComponentReflectionExt.FastGetter(t, "position");
//            }
//            Debug.LogError("FastGetter position=" + value.ToString());
//
//            time = Time.realtimeSinceStartup;
//            Debug.LogError("Component FastGetter duration = " + ((time - startTime) * 1000).ToString() + "(ms)");
//
//            var angle = Vector3.one * 100;
//            startTime = Time.realtimeSinceStartup;
//            for (var i = 0; i < 1000; i++)
//            {
//                var positionProperty = ReflectionHelper.GetProperty(t.GetType(), "localEulerAngles");
//                positionProperty.SetValue(t, angle, null);
//            }
//
//            time = Time.realtimeSinceStartup;
//            Debug.LogError("reflection setter localEulerAngles=" + t.localEulerAngles.ToString());
//            Debug.LogError("reflection setter duration = " + ((time - startTime) * 1000).ToString() + "(ms)");
//            t.localEulerAngles = Vector3.one;
//            angle = new Vector3(199, 199, 199);
//            startTime = Time.realtimeSinceStartup;
//            for (var i = 0; i < 1000; i++)
//            {
//                ComponentReflectionExt.FastSetter(t, "localEulerAngles", angle);
//            }
//
//            time = Time.realtimeSinceStartup;
//            Debug.LogError("FastGetter localEulerAngles=" + t.localEulerAngles.ToString());
//            Debug.LogError("Component FastSetter duration = " + ((time - startTime) * 1000).ToString() + "(ms)");

        }
    }

    /// <summary>
    /// //
    /// </summary>
    internal class RestartApp
    {
        bool withUpdate = true;

        public RestartApp(bool withUpdate)
        {
            this.withUpdate = withUpdate;
        }

        public void Restart()
        {
            ShutdownApp.DisposeLua();
            BootstrapApp.PrepareLua(() =>
                {
                    var luaBridge = LBootApp.luaBridge;

                    if (withUpdate)
                    {
                        luaBridge.Call("__main", new object[1] { "" });
                    }
                    else
                    {
                        luaBridge.Call("__main", new object[1] { "" });
                    }
                });


        }
    }

    /// <summary>
    /// //
    /// </summary>
    internal class PauseApp
    {
        public void Pause()
        {
            var luaBridge = LBootApp.luaBridge;
            if (luaBridge != null)
            {
                luaBridge.Call("didEnterBackground");
            }
        }
    }

    /// <summary>
    /// //
    /// </summary>
    internal class ResumeApp
    {
        public void Resume()
        {
            var luaBridge = LBootApp.luaBridge;
            if (luaBridge != null)
            {
                luaBridge.Call("willEnterForeground");
            }
        }
    }

    /// <summary>
    /// //
    /// </summary>
    public class ShutdownApp
    {
        public void Shutdown()
        {
            var luaBridge = LBootApp.luaBridge;

            if (luaBridge != null)
            {
                luaBridge.Call("applicationWillTerminate");
                DisposeLua();
            }
        }

        public static void DisposeLua()
        {
            if (LBootApp.luaBridge != null)
            {
                LBootApp.luaBridge.Dispose();
                LBootApp.luaBridge = null;
            }
        }
    }

    /// <summary>
    /// //
    /// </summary>
    internal class ReceivedMemoryWarning
    {
        public void Notify()
        {
            var luaBridge = LBootApp.luaBridge;
            if (luaBridge != null)
            {
                luaBridge.Call("applicationDidReceiveMemoryWarning");
            }
        }
    }

}

