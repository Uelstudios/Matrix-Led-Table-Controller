using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MatrixLedTableController
{
    class CustomInterface
    {
        public List<string> items;
        public List<CustomInterfaceItem> interfaceItems;

        public CustomInterface()
        {
            items = new List<string>();
            interfaceItems = new List<CustomInterfaceItem>();
        }

        public CustomInterface AddLabel(string text)
        {
            interfaceItems.Add(new CustomInterfaceItemLabel(text));
            return AddItem("label", "", text, "");
        }

        public CustomInterface AddSpacer(bool breakLine)
        {
            interfaceItems.Add(new CustomInterfaceItemSpacer(20, breakLine));
            return AddItem("spacer", "", "", breakLine ? "true" : "false");
        }

        public CustomInterface AddButton(string id, string text)
        {
            interfaceItems.Add(new CustomInterfaceItemButton(id, text));
            return AddItem("button", id, text, "");
        }

        public CustomInterface AddEditText(string id, string text, string hint)
        {
            interfaceItems.Add(new CustomInterfaceItemEditText(id, text, hint));
            return AddItem("editText", id, text, hint);
        }

        public CustomInterface AddSlider(string id, int value, int min, int max)
        {
            interfaceItems.Add(new CustomInterfaceItemSlider(id, value, min, max));
            return AddItem("slider", id, "", string.Format("{0}~{1}~{2}", value, 0, max));
        }

        public CustomInterface AddList(string id, string text, string[] items)
        {
            interfaceItems.Add(new CustomInterfaceItemList(id, text, items));

            string itemString = string.Empty;

            for (int i = 0; i < items.Length; i++)
            {
                itemString += items[i];
                if (i < items.Length - 1) itemString += "~";
            }

            return AddItem("list", id, "", itemString);
        }

        public CustomInterface AddDrawingboard(string id)
        {
            interfaceItems.Add(new CustomInterfaceItemDrawingboard(id));
            return this;
        }

        private CustomInterface AddItem(string type, string name, string label, string data)
        {
            items.Add(string.Format("{0}:{1}:{2}:{3}", type, name, label, data));
            return this;
        }

        public string toJson()
        {
            return JArray.FromObject(interfaceItems).ToString();
        }

        public class CustomInterfaceItem
        {
            public string type;

            public CustomInterfaceItem(string type)
            {
                this.type = type;
            }
        }

        public class CustomInterfaceItemLabel : CustomInterfaceItem
        {
            public string text;

            public CustomInterfaceItemLabel(string text) : base("label")
            {
                this.text = text;
            }
        }

        public class CustomInterfaceItemSpacer : CustomInterfaceItem
        {
            public int height;
            public bool showLine;

            public CustomInterfaceItemSpacer(int height, bool showLine) : base("spacer")
            {
                this.height = height;
                this.showLine = showLine;
            }
        }

        public class CustomInterfaceItemButton : CustomInterfaceItem
        {
            public string id;
            public string text;

            public CustomInterfaceItemButton(string id, string text) : base("button")
            {
                this.id = id;
                this.text = text;
            }
        }

        public class CustomInterfaceItemEditText : CustomInterfaceItem
        {
            public string id;
            public string text;
            public string hint;

            public CustomInterfaceItemEditText(string id, string text, string hint) : base("editText")
            {
                this.id = id;
                this.text = text;
                this.hint = hint;
            }
        }

        public class CustomInterfaceItemSlider : CustomInterfaceItem
        {
            public string id;
            public int value;
            public int min;
            public int max;

            public CustomInterfaceItemSlider(string id, int value, int min, int max) : base("slider")
            {
                this.id = id;
                this.value = value;
                this.max = max;
                this.min = min;
            }
        }

        public class CustomInterfaceItemList : CustomInterfaceItem
        {
            public string id;
            public string text;
            public string[] items;

            public CustomInterfaceItemList(string id, string text, string[] items) : base("list")
            {
                this.id = id;
                this.text = text;
                this.items = items;
            }
        }

        public class CustomInterfaceItemDrawingboard : CustomInterfaceItem
        {
            public string id;

            public CustomInterfaceItemDrawingboard(string id) : base("drawingboard")
            {
                this.id = id;
            }

            public static Position ParsePosition(string data)
            {
                string[] split = data.Split(',');
                int x = int.Parse(split[0]);
                int y = int.Parse(split[1]);
                return new Position(x, y);
            }

            public static Apps.PixelColor ParseColor(string data)
            {
                string[] split = data.Split(',');
                int R = int.Parse(split[2]);
                int G = int.Parse(split[3]);
                int B = int.Parse(split[4]);
                return new Apps.PixelColor(R, G, B);
            }
        }
    }
}
