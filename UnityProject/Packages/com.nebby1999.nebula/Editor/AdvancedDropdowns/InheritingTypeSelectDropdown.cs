using System.Collections.Generic;
using System;
using UnityEditor.IMGUI.Controls;
using UnityEditor;
using System.Linq;

public class InheritingTypeSelectDropdown : AdvancedDropdown
{
    private const string ROOT_ITEM_KEY = "StateType";

    public event Action<Item> onItemSelected;

    public Type requiredBaseType { get; private set; }

    protected override AdvancedDropdownItem BuildRoot()
    {
        var types = TypeCache.GetTypesDerivedFrom(requiredBaseType);


        var items = new Dictionary<string, Item>();
        var rootItem = new Item(ROOT_ITEM_KEY, ROOT_ITEM_KEY, ROOT_ITEM_KEY, ROOT_ITEM_KEY);
        items.Add(ROOT_ITEM_KEY, rootItem);

        foreach (var assemblyQualifiedName in types.Select(x => x.AssemblyQualifiedName).OrderBy(x => x))
        {
            var itemFullName = assemblyQualifiedName.Split(',')[0];
            while (true)
            {
                var lastDotIndex = itemFullName.LastIndexOf('.');
                if (!items.ContainsKey(itemFullName))
                {
                    var typeName =
                        lastDotIndex == -1 ? itemFullName : itemFullName.Substring(lastDotIndex + 1);
                    var item = new Item(typeName, typeName, itemFullName, assemblyQualifiedName);
                    items.Add(itemFullName, item);
                }

                if (itemFullName.IndexOf('.') == -1) break;

                itemFullName = itemFullName.Substring(0, lastDotIndex);
            }
        }

        foreach (var item in items)
        {
            if (item.Key == ROOT_ITEM_KEY)
                continue;

            var fullName = item.Key;
            if (fullName.LastIndexOf('.') == -1)
            {
                rootItem.AddChild(item.Value);
            }
            else
            {
                var parentName = fullName.Substring(0, fullName.LastIndexOf('.'));
                items[parentName].AddChild(item.Value);
            }
        }

        return rootItem;
    }

    protected override void ItemSelected(AdvancedDropdownItem item)
    {
        onItemSelected?.Invoke((Item)item);
    }


    public InheritingTypeSelectDropdown(AdvancedDropdownState state, Type requiredBaseType) : base(state)
    {
        this.requiredBaseType = requiredBaseType;
        var minSize = minimumSize;
        minSize.y = 200;
        minimumSize = minSize;
    }

    public class Item : AdvancedDropdownItem
    {
        public string typeName { get; }
        public string fullName { get; }
        public string assemblyQualifiedName { get; }

        public Item(string displayName, string typeName, string fullName, string assemblyQualifiedName) : base(
displayName)
        {
            this.typeName = typeName;
            this.fullName = fullName;
            this.assemblyQualifiedName = assemblyQualifiedName;
        }
    }
}