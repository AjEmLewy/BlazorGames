using System.Collections;
using System.Collections.Generic;

namespace Gejms.Client.Shared.Models.GameObjectComponents;

public class ComponentsCollection : IEnumerable
{
    private readonly Dictionary<Type, IComponent> components = new();

    public void Add<T>(T component) where T : IComponent
    {
        var type = typeof(T);
        if (!components.ContainsKey(type))
            components.Add(type, null);
        components[type] = component;
    }

    public T Get<T>() where T : class, IComponent
    {
        var type = typeof(T);
        if (!components.ContainsKey(type))
            throw new ArgumentException("missing type " + type.ToString());
        else
            return components[type] as T;
    }

    public IEnumerator GetEnumerator()
    {
        return components.Values.GetEnumerator();
    }
}
