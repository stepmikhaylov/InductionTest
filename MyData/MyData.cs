using System.Collections.Generic;

namespace InductionTest.MyData
{
    public class MyData
	{
		public int MyInt;
		public string MyString;
	}

	public abstract class MyDataComparer : IComparer<MyData>
	{
		public static MyDataComparer ByMyInt { get; } = new ByMyIntComparer();
		public static MyDataComparer ByMyString { get; } = new ByMyStringComparer();

		public abstract int Compare(MyData x, MyData y);

		class ByMyIntComparer : MyDataComparer
		{
			public override int Compare(MyData x, MyData y)
				=> x != null
					? y != null
						? x.MyInt.CompareTo(y.MyInt)
						: 1
					: y != null
						? -1
						: 0;
		}

		class ByMyStringComparer : MyDataComparer
		{
			public override int Compare(MyData x, MyData y)
				=> x != null
					? y != null
						? string.Compare(x.MyString, y.MyString)
						: 1
					: y != null
						? -1
						: 0;
		}
	}

	public static class MyDataExtensions
	{
		public static void SortByMyInt(this List<MyData> lst)
			=> lst.Sort(MyDataComparer.ByMyInt);

		public static void SortByMyString(this List<MyData> lst)
			=> lst.Sort(MyDataComparer.ByMyString);
	}
}
