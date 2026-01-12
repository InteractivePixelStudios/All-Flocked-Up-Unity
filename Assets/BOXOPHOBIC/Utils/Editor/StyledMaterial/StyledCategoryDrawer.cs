// Cristian Pop - https://boxophobic.com/

using UnityEngine;
using UnityEditor;
using System;

namespace Boxophobic.StyledGUI
{
    public class StyledCategoryDrawer : MaterialPropertyDrawer
    {
        public bool isEnabled = true;
        public bool showDot = false;

        public string category;
        public string colapsable;
        public string conditions = "";
        public string dotColor = "";
        public string infoText = "";
        public float top;
        public float down;

        public StyledCategoryDrawer(string category)
        {
            this.category = category;
            this.colapsable = "false";
            this.conditions = "";
            this.dotColor = "";
            this.top = 10;
            this.down = 10;
        }

        public StyledCategoryDrawer(string category, string colapsable)
        {
            this.category = category;
            this.colapsable = colapsable;
            this.conditions = "";
            this.dotColor = "";
            this.top = 10;
            this.down = 10;
        }

        public StyledCategoryDrawer(string category, float top, float down)
        {
            this.category = category;
            this.colapsable = "false";
            this.conditions = "";
            this.dotColor = "";
            this.top = top;
            this.down = down;
        }

        public StyledCategoryDrawer(string category, string colapsable, float top, float down)
        {
            this.category = category;
            this.colapsable = colapsable;
            this.conditions = "";
            this.dotColor = "";
            this.top = top;
            this.down = down;
        }
        public StyledCategoryDrawer(string category, string colapsable, string infoText, float top, float down)
        {
            this.category = category;
            this.colapsable = colapsable;
            this.conditions = "";
            this.dotColor = "";
            this.infoText = infoText;
            this.top = top;
            this.down = down;
        }

        public StyledCategoryDrawer(string category, string colapsable, string conditions, string dotColor, float top, float down)
        {
            this.category = category;
            this.colapsable = colapsable;
            this.conditions = conditions;
            this.dotColor = dotColor;
            this.infoText = "";
            this.top = top;
            this.down = down;
        }

        public StyledCategoryDrawer(string category, string colapsable, string conditions, string dotColor, string infoText, float top, float down)
        {
            this.category = category;
            this.colapsable = colapsable;
            this.conditions = conditions;
            this.dotColor = dotColor;
            this.infoText = infoText;
            this.top = top;
            this.down = down;
        }

        public override void OnGUI(Rect position, MaterialProperty prop, String label, MaterialEditor materialEditor)
        {
            GUI.enabled = true;
            //GUI.color = Color.white;
            //GUI.contentColor = Color.white;
            EditorGUI.indentLevel = 0;

            if (conditions != "")
            {
                showDot = false;

                Material material = materialEditor.target as Material;

                string[] split = conditions.Split(char.Parse(" "));

                for (int i = 0; i < split.Length; i++)
                {
                    var property = split[i];

                    if (material.HasProperty(property))
                    {
                        if (material.GetFloat(property) > 0)
                        {
                            showDot = true;
                            break;
                        }
                    }
                }

                DrawInspector(prop);
            }
            else
            {
                DrawInspector(prop);
            }
        }

        public override float GetPropertyHeight(MaterialProperty prop, string label, MaterialEditor editor)
        {
            return -2;
        }

        void DrawInspector(MaterialProperty prop)
        {
            bool isColapsable = false;

            if (colapsable == "true")
            {
                isColapsable = true;
            }

            //bool isEnabled = true;

            if (prop.floatValue < 0.5f)
            {
                isEnabled = false;
            }
            else
            {
                isEnabled = true;
            }

            if (showDot)
            {
                isEnabled = StyledGUI.DrawInspectorCategory(category, isEnabled, isColapsable, dotColor, infoText, top, down);
            }
            else
            {
                if (infoText != "")
                {
                    isEnabled = StyledGUI.DrawInspectorCategory(category, isEnabled, isColapsable, infoText, top, down);
                }
                else
                {
                    isEnabled = StyledGUI.DrawInspectorCategory(category, isEnabled, isColapsable, top, down);
                }

            }

            if (isEnabled)
            {
                prop.floatValue = 1;
            }
            else
            {
                prop.floatValue = 0;
            }
        }
    }
}
