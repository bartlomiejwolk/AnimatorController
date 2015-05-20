using System;
using System.Diagnostics;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace AnimatorControllerEx {

    public enum InfoType {

        Warning,
        Error

    }

    public static class Utilities {

        /// <summary>
        /// </summary>
        /// <remarks>
        ///     http://forum.unity3d.com/threads/assert-class-for-debugging.59010/
        /// </remarks>
        /// <param name="assertion"></param>
        /// <param name="assertString"></param>
        [Conditional("UNITY_EDITOR")]
        public static void Assert(Func<bool> assertion, string assertString) {
            if (!assertion()) {
                var myTrace = new StackTrace(true);
                var myFrame = myTrace.GetFrame(1);
                var assertInformation = "Filename: " + myFrame.GetFileName()
                                        + "\nMethod: " + myFrame.GetMethod()
                                        + "\nLine: "
                                        + myFrame.GetFileLineNumber();

                // Output message to Unity log window.
                UnityEngine.Debug.Log(assertString + "\n" + assertInformation);
                // Break only in play mode.
                if (Application.isPlaying) {
                    UnityEngine.Debug.Break();
                }
#if UNITY_EDITOR
                if (EditorUtility.DisplayDialog(
                    "Assert!",
                    assertString + "\n" + assertInformation,
                    "Close")) {
                }
#endif
            }
        }

        public static bool FloatsEqual(
            float a,
            float b,
            float epsilon = 0.001f) {

            return Mathf.Abs(a - b) < epsilon;
        }

        public static object InvokeMethodWithReflection(
            object target,
            string methodName,
            object[] parameters) {

            // Get method metadata.
            var methodInfo = target.GetType().GetMethod(
                methodName,
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = methodInfo.Invoke(target, parameters);

            return result;
        }

        /// <summary>
        ///     Log info about missing reference.
        /// </summary>
        /// <param name="sourceCo">
        ///     MonoBehaviour from which this method was called.
        /// </param>
        /// <param name="fieldName">
        ///     Name of the field that the reference is missing.
        /// </param>
        /// <param name="detailedInfo">Additional custom information.</param>
        /// <param name="type">
        ///     Type of the Debug.Log() used to output the message.
        /// </param>
        public static void MissingReference(
            MonoBehaviour sourceCo,
            string fieldName,
            string detailedInfo = "",
            InfoType type = InfoType.Error) {

            // Message to display.
            // todo use StringBuilder
            var message = "Component reference is missing in: "
                          + sourceCo.transform.root
                          + " / "
                          + sourceCo.gameObject.name
                          + " '"
                          + sourceCo.GetType()
                          + "'"
                          + " / "
                          + "Field name: " + fieldName
                          + " / "
                          + "Message: " + detailedInfo;

            switch (type) {
                case InfoType.Warning:
                    UnityEngine.Debug.LogWarning(message, sourceCo.transform);
                    break;

                case InfoType.Error:
                    UnityEngine.Debug.LogError(message, sourceCo.transform);
                    break;
            }
        }

    }

}
