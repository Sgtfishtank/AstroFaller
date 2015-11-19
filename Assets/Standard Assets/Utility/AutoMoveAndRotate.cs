using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class AutoMoveAndRotate : MonoBehaviour
    {
		public bool rotateAroundParent;
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
			Destroy (this);
		}


        // Update is called once per frame
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if (ignoreTimescale)
            {
                deltaTime = (Time.realtimeSinceStartup - m_LastRealTime);
                m_LastRealTime = Time.realtimeSinceStartup;
            }
            transform.Translate(moveUnitsPerSecond.value*deltaTime, moveUnitsPerSecond.space);
			if (rotateAroundParent) 
			{
				transform.RotateAround(transform.parent.position, rotateDegreesPerSecond.value, 50 * deltaTime);
			}
			else 
			{
				transform.Rotate(rotateDegreesPerSecond.value*deltaTime, moveUnitsPerSecond.space);
			}
        }


        [Serializable]
        public class Vector3andSpace
        {
            public Vector3 value;
            public Space space = Space.Self;
        }
    }
}
