using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

namespace HIEngine
{
    /// <summary>
    /// A console that displays the contents of Unity's debug log.
    /// </summary>
    public class HConsole : MonoBehaviour
	{
		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		public static readonly Version version = new Version(1, 0);

		struct ConsoleMessage
		{
			public enum LogType
			{
				Log = 0,
				Info = 1,
				Warning = 2,
				Error = 3,
				Assert = 4,
				Exception = 5,
			}

			public readonly string message;
			public readonly string stackTrace;
			public readonly ConsoleMessage.LogType type;
			public readonly int level;

			public ConsoleMessage(string message, string stackTrace, ConsoleMessage.LogType type, int level = 0)
			{
				this.message = message;
				this.stackTrace = stackTrace;
				this.type = type;
				this.level = level;
			}
		}
        
		public string path = "ConsoleLog.txt"; 
		private static StreamWriter sw = null;

		public KeyCode toggleKey = KeyCode.Pause; //控制log显隐的按钮

		public Color errorColor = Color.red;
		public Color assertColor = Color.black;
		public Color warningColor = Color.yellow;
		public Color logColor = Color.white;
		public Color infoColor = Color.white;
		public Color exceptionColor = Color.blue;
		public Color infoColor1 = Color.green;
		public Color infoColor2 = Color.red;
		public Color infoColor3 = Color.yellow;

		private static List<ConsoleMessage> entries = new List<ConsoleMessage>(); 
		Vector2 scrollPos;
		public static bool show = false;
		bool collapse; //折叠连续位置重复的

		// Visual elements:

		const int margin = 20; //窗口给屏幕留的空白
		Rect windowRect = new Rect(margin, margin, Screen.width - (2 * margin), Screen.height - (2 * margin));

		GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
		GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

		void Update()
		{
			if (Input.GetKeyDown(toggleKey))
			{
				show = !show;
			}
		}

		#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		void OnGUI()
		{
			if (!show)
			{
				return;
			}

			GUI.backgroundColor = Color.black;
			GUI.DrawTexture(windowRect, Resources.Load<Texture>("Textures/Texture_Black"));
			windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console");
		}
		#endif

		/// <summary>
		/// A window displaying the logged messages.
		/// </summary>
		/// <param name="windowID">The window's ID.</param>
		void ConsoleWindow(int windowID)
		{
			scrollPos = GUILayout.BeginScrollView(scrollPos);

			// Go through each logged entry
			for (int i = 0; i < entries.Count; i++)
			{
				ConsoleMessage entry = entries[i];

				// If this message is the same as the last one and the collapse feature is chosen, skip it
				// 是否：跳过与前一日志信息重复的
				if (collapse && i > 0 && entry.message == entries[i - 1].message)
				{
					continue;
				}

				// Change the text colour according to the log type
				switch (entry.type)
				{
				case ConsoleMessage.LogType.Error:
					GUI.contentColor = errorColor;
					break;
				case ConsoleMessage.LogType.Assert:
					GUI.contentColor = assertColor;
					break;
				case ConsoleMessage.LogType.Warning:
					GUI.contentColor = warningColor;
					break;
				case ConsoleMessage.LogType.Log:
					GUI.contentColor = logColor;
					break;
				case ConsoleMessage.LogType.Exception:
					GUI.contentColor = exceptionColor;
					break;
				case ConsoleMessage.LogType.Info:
					{
						switch (entry.level)
						{ 
						case 0:
							GUI.contentColor = infoColor;
							break;
						case 1:
							GUI.contentColor = infoColor1;
							break;
						case 2:
							GUI.contentColor = infoColor2;
							break;
						case 3:
							GUI.contentColor = infoColor3;
							break;
						}
						break;
					}
				default:
					GUI.contentColor = logColor;
					break;
				}

				GUILayout.Label(entry.message);
			}

			GUI.contentColor = Color.white;

			GUILayout.EndScrollView();

			GUILayout.BeginHorizontal();

			// Clear button
			if (GUILayout.Button(clearLabel))
			{
				entries.Clear(); //清除所有日志信息
			}

			// Collapse toggle
			collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

			GUILayout.EndHorizontal();

			// Set the window to be draggable by the top title bar
			GUI.DragWindow(new Rect(0, 0, 10000, 20));
		}

		/// <summary>
		/// Logged messages are sent through this callback function.
		/// </summary>
		/// <param name="message">The message itself.</param>
		/// <param name="stackTrace">A trace of where the message came from.</param>
		/// <param name="type">The type of message: error/exception, warning, or assert.</param>
		void HandleLog(string message, string stackTrace, ConsoleMessage.LogType type)
		{
			ConsoleMessage entry = new ConsoleMessage(message, stackTrace, type);
			entries.Add(entry);
		}

		public static void Error(string str)
		{
			ConsoleMessage entry = new ConsoleMessage(str, null, ConsoleMessage.LogType.Error);
			entries.Add(entry);
		}

		public static void Assert(string str)
		{
			ConsoleMessage entry = new ConsoleMessage(str, null, ConsoleMessage.LogType.Assert);
			entries.Add(entry);
		}

		public static void Warning(string str)
		{
			ConsoleMessage entry = new ConsoleMessage(str, null, ConsoleMessage.LogType.Warning);
			entries.Add(entry);
		}

		public static void Log(string str)
		{
			ConsoleMessage entry = new ConsoleMessage(str, null, ConsoleMessage.LogType.Log);
			entries.Add(entry);
		}

		public static void Info(string str, int level = 0)
		{
			ConsoleMessage entry = new ConsoleMessage(str, null, ConsoleMessage.LogType.Info, level);
			entries.Add(entry);
		}

		public static void Exception(string str)
		{
			ConsoleMessage entry = new ConsoleMessage(str, null, ConsoleMessage.LogType.Exception);
			entries.Add(entry);
		}

		public static void WriteFile(string format, params object[] paramList)
		{
			if (sw != null)
			{
				sw.WriteLine(format, paramList);
				sw.Flush();
			}
		}
		#else
		#endif
	}
}
	