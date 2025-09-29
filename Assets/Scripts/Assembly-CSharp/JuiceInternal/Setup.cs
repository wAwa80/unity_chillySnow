using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/*
 * 
 * 设计模式分析
1. 模块化架构
? 自动注册：通过反射自动发现所有模块，无需手动注册
? 统一生命周期：所有模块遵循相同的初始化流程
? 松耦合：模块之间通过基类交互，降低依赖
 * 
 * 
 * 3. 依赖注入思想
控制反转：框架控制模块的创建和生命周期

集中管理：所有模块实例由Setup类统一管理
 * 
 */

namespace JuiceInternal
{
	/// <summary>
	/// 这是一个模块化框架的初始化系统，负责在游戏启动时自动设置和启动所有模块
	/// </summary>
	internal static class Setup
	{
		private static bool setupDone;

		private static bool startupDone;

		private static readonly HashSet<ModuleBase> modules = new HashSet<ModuleBase>();

		/// <summary>
		/// 1. 自动初始化系统
		/// 
		/// 场景加载前执行：确保所有模块在游戏场景加载前就完成初始化
		/// 单次执行保证：通过setupDone标志防止重复初始化
		/// </summary>
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void SetupAll()
		{
			if (setupDone)
			{
				return;
			}
			//开发时数据清理：在调试模式下自动清空玩家数据
			if (Settings.Get().debug && Settings.Get().resetOnStart)
			{
				PlayerPrefs.DeleteAll();
			}
			//GDPR优先处理：先初始化隐私合规相关模块
			typeof(GDPR).GetMethod("Setup", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
			//自动发现所有模块
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			foreach (Type type in types)
			{
				Type moduleType = GetModuleType(type);
				if (moduleType != null)
				{
					//模块实例化, 每个模块都有自己的internal_Setup方法, 将实例保存在集合中供后续使用
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
		/// 场景加载后执行
		/// 顺序保证：所有模块初始化完成后才执行游戏启动逻辑
		/// 异常安全：单个模块的启动失败不会影响其他模块
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
		/// 模块识别逻辑
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
