using System.Collections.Generic;

public static class Tools
{
	public static void Add<T>(this T item, ref List<T> list)
	{
		if(list == null) list = new List<T>();

		list.Add(item);
	}

	public static void Remove<T>(this T item, ref List<T> list)
	{
		if (list == null) list = new List<T>();

		list.Remove(item);
	}
}

