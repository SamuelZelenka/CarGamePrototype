using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ListExtension
{

	public static void OffsetListBackward<T>(this List<T> inputList, int offset)
	{
		int length = inputList.Count;
		offset = offset % length;
		inputList.ReverseList(0, length - 1);
		inputList.ReverseList(0, offset - 1);
		inputList.ReverseList(offset, length - 1);
	}

	public static void ReverseList<T>(this List<T> list, int start, int end)
	{
		while (start < end)
		{
			T temp = list[start];
			list[start] = list[end];
			list[end] = temp;

			start++;
			end--;
		}
	}
}

