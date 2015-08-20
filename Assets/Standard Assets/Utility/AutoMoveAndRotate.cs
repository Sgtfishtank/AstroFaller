using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class AutoMoveAndRotate : MonoBehaviour
    {
        public Vector3andSpace moveUnitsPerSecond;
        public Vector3andSpace rotateDegreesPerSecond;
        public bool ignoreTimescale;
		public Vector2 xValues;
		public Vector2 yValues;
		public Vector2 zValues;
        private float m_LastRealTime;


        private void Start()
        {
            m_LastRealTime = Time.realtimeSinceStartup;
			rotateDegreesPerSecond.value = new Vector3 (UnityEngine.Random.Range (xValues.x, xValues.y), UnityEngine.Random.Range (yValues.x, yValues.y), UnityEngine.Random.Range (zValues.x, zValues.y));
        }


        // Update is called once per frame
        private void Update()
        {
			return;
            float deltaTime = Time.deltaTime;
            if (ignoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - m_LastRealTime);
                m_LastRealTime = Time.realtimeSinceStartup;
            }
            transform.Translate(moveUnitsPerSecond.value*deltaTime, moveUnitsPerSecond.space);
            transform.Rotate(rotateDegreesPerSecond.value*deltaTime, moveUnitsPerSecond.space);
        }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }
}
