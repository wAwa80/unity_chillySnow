using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*
 * 
 * ���ģʽ����
1. ģ�黯�ܹ�
? �Զ�ע�᣺ͨ�������Զ���������ģ�飬�����ֶ�ע��
? ͳһ�������ڣ�����ģ����ѭ��ͬ�ĳ�ʼ������
? ����ϣ�ģ��֮��ͨ�����ཻ������������
 * 
 * 
 * 3. ����ע��˼��
���Ʒ�ת����ܿ���ģ��Ĵ�������������

���й�������ģ��ʵ����Setup��ͳһ����
 * 
 */

namespace JuiceInternal
{
	/// <summary>
	/// ����һ��ģ�黯��ܵĳ�ʼ��ϵͳ����������Ϸ����ʱ�Զ����ú���������ģ��
	/// </summary>
	internal static class Setup
	{
		private static bool setupDone;

		private static bool startupDone;

		private static readonly HashSet<ModuleBase> modules = new HashSet<ModuleBase>();

		/// <summary>
		/// 1. �Զ���ʼ��ϵͳ
		/// 
		/// ��������ǰִ�У�ȷ������ģ������Ϸ��������ǰ����ɳ�ʼ��
		/// ����ִ�б�֤��ͨ��setupDone��־��ֹ�ظ���ʼ��
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void SetupAll()
		{
			if (setupDone)
			{
				return;
			}
			//����ʱ���������ڵ���ģʽ���Զ�����������
			if (Settings.Get().debug && Settings.Get().resetOnStart)
			{
				PlayerPrefs.DeleteAll();
			}
			//GDPR���ȴ����ȳ�ʼ����˽�Ϲ����ģ��
			typeof(GDPR).GetMethod("Setup", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
			//�Զ���������ģ��
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type type in types)
			{
				Type moduleType = GetModuleType(type);
				if (moduleType != null)
				{
					//ģ��ʵ����, ÿ��ģ�鶼���Լ���internal_Setup����, ��ʵ�������ڼ����й�����ʹ��
					ModuleBase moduleBase = (ModuleBase)moduleType.GetMethod("internal_Setup", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
					if (moduleBase != null)
					{
						modules.Add(moduleBase);
					}
				}
			}
			setupDone = true;
		}

		/// <summary>
		/// �������غ�ִ��
		/// ˳��֤������ģ���ʼ����ɺ��ִ����Ϸ�����߼�
		/// �쳣��ȫ������ģ�������ʧ�ܲ���Ӱ������ģ��
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
		private static void GameStarted()
		{
			if (startupDone)
			{
				return;
			}
			foreach (ModuleBase module in modules)
			{
				try
				{
					module.GetType().GetMethod("OnGameStarted", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(module, null);
				}
				catch (Exception ex)
				{
					Log.Warning(module.GetType().ToString(), $"Could not perform game startup successfully. Exception was thrown : \"{ex.Message}\" ({ex.StackTrace})");
				}
			}
			startupDone = true;
		}

		/// <summary>
		/// ģ��ʶ���߼�
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		private static Type GetModuleType(Type type)
		{
			if (type.ContainsGenericParameters)
			{
				return null;
			}
			for (type = type.BaseType; type != null; type = type.BaseType)
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Module<>))
				{
					return type;
				}
			}
			return null;
		}
	}
}
