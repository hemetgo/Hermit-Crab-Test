using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HemetToolkit
{
    public static class ListToolkit
    {
        public static List<T> Shuffle<T>(List<T> _list)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                T temp = _list[i];
                int randomIndex = Random.Range(i, _list.Count);
                _list[i] = _list[randomIndex];
                _list[randomIndex] = temp;
            }

            return _list;
        }

        public static List<T> Shuffle<T>(System.Random random, List<T> _list)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                T temp = _list[i];
                int randomIndex = random.Next(i, _list.Count);
                _list[i] = _list[randomIndex];
                _list[randomIndex] = temp;
            }

            return _list;
        }
    }

    public static class Transform2DMathToolkit
	{
        public static void RotateAt(Transform obj, Transform target)
        {
            var angle = GetAngle(obj, target.transform.position);
            obj.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public static void RotateAt(Transform obj, Vector3 target)
        {
            var angle = GetAngle(obj, target);
            obj.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        private static float GetAngle(Transform obj, Vector3 target)
        {
            target.z = 0f;

            Vector3 objectPos = obj.position;
            target.x = target.x - objectPos.x;
            target.y = target.y - objectPos.y;

            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            return angle;
        }

        public static float GetAngleBetweenTwoPoints(Vector2 origin, Vector2 target)
        {
            Vector2 v2 = (target - origin).normalized;
            float angle = Mathf.Atan2(v2.y, v2.x) * Mathf.Rad2Deg;
            if (angle < 0)
                angle = 360 + angle;
            angle = 360 - angle;

            return angle + 90;
        }
    }

    public static class SpriteRendererToolkit
    {
        public static IEnumerator Blink(SpriteRenderer renderer, float blinkTime, float blinkInterval)
        {
            int blinkTimes = (int)(blinkTime / blinkInterval);
            renderer.enabled = false;

            for (int i = 0; i < blinkTimes; i++)
            {
                yield return new WaitForSeconds(blinkInterval);
                renderer.enabled = !renderer.enabled;
            }

            renderer.enabled = true;
        }
    }
}
