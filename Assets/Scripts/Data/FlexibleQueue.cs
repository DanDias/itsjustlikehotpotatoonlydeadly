using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://stackoverflow.com/questions/1594375/is-there-a-better-way-to-implement-a-remove-method-for-a-queue
public class FlexibleQueue<T>
{
    LinkedList<T> list;

    public FlexibleQueue()
    {
        list = new LinkedList<T>();
    }

    public FlexibleQueue(IEnumerable<T> collection)
    {
        list = new LinkedList<T>(collection);
    }

	public void Enqueue(T t)
    {
        list.AddLast(t);
    }

    public T Dequeue()
    {
        var result = list.First.Value;
        list.RemoveFirst();
        return result;
    }

    public T Peek()
    {
        return list.First.Value;
    }

    public T Last()
    {
        return list.Last.Value;
    }

    public bool Remove(T t)
    {
        return list.Remove(t);
    }

    public void Clear()
    {
        list.Clear();
    }

    public int Count { get { return list.Count; } }
}
