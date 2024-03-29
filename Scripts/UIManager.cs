using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
using CodeControl;
using EasyMobile.Demo;
using JetBrains.Annotations;
using UI.Dialog;

namespace UI
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        private Dictionary<Type, DialogBase> dialogMap = new Dictionary<Type, DialogBase>();

        protected override void Awake()
        {
            base.Awake();
            var dialogs = transform.GetComponentsInChildren<DialogBase>(true);
            for (int i = 0; i < dialogs.Length; i++)
            {
                Type type = dialogs[i].GetType();
                
                if (dialogMap.ContainsKey(type) == false)
                {
                    dialogMap.Add(type, dialogs[i]);
                }
            }

            ResetDialog();
        }

        public void ResetDialog()
        {
            foreach (KeyValuePair<Type, DialogBase> keyValuePair in dialogMap)
            {
                keyValuePair.Value.gameObject.SetActive(false);
            }
        }

        public void RequestDialogEnter<T>() where T : DialogBase
        {
            SetActiveUI<T>(true);
        }

        public void RequestDialogExit<T>() where T : DialogBase
        {
            SetActiveUI<T>(false);
        }

        void SetActiveUI<T>(bool isActive) where T : DialogBase
        {
            Type type = typeof(T);
            if (dialogMap.ContainsKey(type))
            {
                var dialog = dialogMap[type];
                dialog.gameObject.SetActive(isActive);
            }
        }
    }
}