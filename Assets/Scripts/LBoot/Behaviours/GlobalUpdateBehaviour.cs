//
// GlobalUpdateBehaviour.cs
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
using UnityEngine;
using SLua;

namespace LBoot
{
    [CustomLuaClassAttribute]
    public class GlobalUpdateBehaviour : MonoBehaviour
    {
        Action<int> updateAction = null;
        Action<int> fixedUpdateAction = null;
        Action<int> lateUpdateAction = null;
        Action<int> finalUpdateAction = null;

        int updateCounter = 0;
        int fixedUpdateCounter = 0;
        int lateUpdateCounter = 0;
        int finalUpdateCounter = 0;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);  
        }

        public Action<int> UpdateAction
        {
            get
            {
                return updateAction;
            }
            set
            {
                updateAction = value;
            }
        }

        public Action<int> FixedUpdateAction
        {
            get
            {
                return fixedUpdateAction;
            }
            set
            {
                fixedUpdateAction = value;
            }
        }

        public Action<int> LateUpdateAction
        {
            get
            {
                return lateUpdateAction;
            }
            set
            {
                lateUpdateAction = value;
            }
        }

        public Action<int> FinalUpdateAction
        {
            get
            {
                return finalUpdateAction;
            }
            set
            {
                finalUpdateAction = value;
            }
        }

        void Update()
        {
            updateCounter += 1;
            if (updateAction != null)
            {
                updateAction(updateCounter);
            }
        }

        void FixedUpdate()
        {
            fixedUpdateCounter += 1;
            if (fixedUpdateAction != null)
            {
                fixedUpdateAction(fixedUpdateCounter);
            }
        }

        void LateUpdate()
        {
            lateUpdateCounter += 1;
            if (lateUpdateAction != null)
            {
                lateUpdateAction(lateUpdateCounter);
            }

            finalUpdateCounter += 1;
            if (finalUpdateAction != null)
            {
                finalUpdateAction(finalUpdateCounter);
            }
        }
    }
}