using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Util {
    public static float Epsilon = 0.01f;

    public static float angleTo(Vector3 v1, Vector3 v2, Vector3 vn) {
        return Mathf.Atan2(
                Vector3.Dot(Vector3.Cross(v1, v2), vn),
                Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }

    public static float easeInOutQuad(float t) {
        if (t < 0.5) return 2*t*t;
        return -1+(4-2*t)*t;
    }

    public static float easeInQuad(float t) {
        return t*t;
    }

    public static float easeOutQuad(float t) {
        return t*(2-t);
    }

    public static float easeInSine(float t) {
        float r = Mathf.Sin(t * Mathf.PI / 2);
        return r*r;
    }

    public static float easeOutSine(float t) {
        float r = Mathf.Cos(t * Mathf.PI / 2);
        return r*r;
    }

    public static void shuffle<T>(IList<T> arr) {
        for (int i = 0; i < arr.Count; i++) {
            int j = (int)Random.Range(i, arr.Count);
            T tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
    }

    public static T randomElemIn<T>(IList<T> arr) {
        int i = (int)Random.Range(0, arr.Count);
        return arr[i];
    }

    public static Quaternion smoothDampQuat(Quaternion current, Quaternion target, float smoothTime, ref float angVel, float maxSpeed = Mathf.Infinity) {
        return smoothDampQuat(current, target, smoothTime, ref angVel, maxSpeed, Time.deltaTime);
    }

    public static Quaternion smoothDampQuat(Quaternion current, Quaternion target, float smoothTime, ref float angVel, float maxSpeed, float deltaTime) {
        float angle = Quaternion.Angle(current, target);
        float newAngle = Mathf.SmoothDamp(angle, 0.0f, ref angVel, smoothTime, maxSpeed, deltaTime);
        float lerp = 1.0f;
        if (Mathf.Abs(angle) > 0) {
            lerp = 1.0f - newAngle / angle;
        }
        return Quaternion.Slerp(current, target, lerp);
    }

    public static float Mod(float dividend, float divisor) {
        return (dividend%divisor + divisor)%divisor;
    }

    public static int Mod(int dividend, int divisor) {
        return (dividend%divisor + divisor)%divisor;
    }

    /* This returns the number of degrees needed to rotate from one angle to
     * another in a given direction. Note that if the angles are the same, this
     * function will always return 360, not 0. */
    public static float DegreesInDirection(float a1, float a2, float direction) {
        if (direction > 0.0f) {
            if (a2 > a1) {
                return a2 - a1;
            } else {
                return a2 + 360 - a1;
            }
        } else if (direction < 0.0f) {
            if (a1 > a2) {
                return a1 - a2;
            } else {
                return a1 + 360 - a2;
            }
        } else {
            return Mathf.Abs(a2 - a1);
        }
    }

    public static void Log(string format, params object[] args) {
        Debug.Log(string.Format(format, args));
    }

    public static void Log(object arg) {
        Debug.Log(arg.ToString());
    }

    public static void LogWarning(string format, params object[] args) {
        Debug.LogWarning(string.Format(format, args));
    }

    public static void LogWarning(object arg) {
        Debug.LogWarning(arg.ToString());
    }

    public static void LogError(string format, params object[] args) {
        Debug.LogError(string.Format(format, args));
    }

    public static void LogError(object arg) {
        Debug.LogError(arg.ToString());
    }

    public static void SetCursorLocked(bool locked) {
        Cursor.lockState = (locked ? CursorLockMode.Locked : CursorLockMode.None);
        Cursor.visible = !locked;
    }
}
