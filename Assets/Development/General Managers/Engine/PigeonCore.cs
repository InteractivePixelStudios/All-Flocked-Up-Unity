using UnityEngine;

namespace PigeonCore
{

    public static class GameCore
    {
        public static bool EnableDebug = 
#if UNITY_EDITOR
    true;  // Always ON in the editor
#else
    false; // Always OFF in the build
#endif

        //Gets returns pigeon gameobject
        public static GameObject GetPigeon()
        {
            var pigeon = GameObject.FindGameObjectWithTag("Player");
            return pigeon;
        }

        //returns pigeon transform
        public static Transform GetPigeonTransform()
        {
            var pigeonTranform = GameObject.FindGameObjectWithTag("Player").transform;
            return pigeonTranform;
        }

        //returns the world position of input object
        public static Vector3 GetObjectWorldPosition(GameObject gameObject)
        {
            var objectPos =  gameObject.transform.position;
            return objectPos;
        }

        //returns the float distance between 2 gameobjects
        public static float GetDistanceToObject(GameObject startPoint, GameObject endPoint)
        {
            var startPointLocation = startPoint.transform.position;
            var endPointLocation = endPoint.transform.position;
            return Vector3.Distance(startPointLocation, endPointLocation);  
        }
        
        //returns the questLog component
        public static QuestLog GetQuestLog()
        {
            var questLog = GameObject.FindAnyObjectByType<QuestLog>();
            return questLog;
        }

        //returns colored debug logs.... ***********ONLY IN EDITOR NOT IN BUILD********
        public static void ColorDebug(string debugLog, Color color)
        {
            if (!EnableDebug) return;
            string newLog = FormatLog(debugLog, color);
            Debug.Log(newLog);
            
        }
        //formats log with color
        public static string FormatLog(string log, Color color) 
        {
            string hexColor = ColorUtility.ToHtmlStringRGB(color);
            return $"<b><color = {hexColor}>{log}</b>";

        } 

        public static bool GetIsFlying()
        {
            var comp = GameObject.FindGameObjectWithTag("Player");
            //uncomment and remove above var after sebastian pushes flight changes
            //var comp = GameObject.FindFirstObjectByType<PlayerFlightMovement>().isFlying;
            return comp;
        }
        
    }
}