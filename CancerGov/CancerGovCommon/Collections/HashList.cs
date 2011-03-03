using System;
using System.Collections;

namespace CancerGov.Collections
{
	/// <summary>
	/// HashList : a hybrid hashtable collection with arraylist enumeration.
	/// This not a strongly type collection.
	/// 
	/// Created By:		Greg Andres
	/// Create Date:	01/13/2004
	///
	/// </summary>
	public class HashList : System.Collections.Hashtable
	{
		public HashList(){}

		public new IEnumerator GetEnumerator()
		{
			return new HashListEnumerator(this);
		}
	}

	#region HashListEnumerator Class 

	public class HashListEnumerator : System.Collections.IEnumerator
	{
		ArrayList hashArray;
		int index;

		public HashListEnumerator(HashList hl)
		{
			hashArray = new ArrayList(hl.Values);
			index = -1;			
		}

		public object Current
		{
			get {return hashArray[index];}
		}

		public bool MoveNext()
		{
			++index;
			return (index < hashArray.Count) ? true : false;
		}

		public void Reset()
		{
			index = -1;
		}
	}

	#endregion

	public class StringHashList : HashList
	{
		public StringHashList(){}

		public new IEnumerator GetEnumerator()
		{
			return base.GetEnumerator();
		}

		public void Add(string key, object value)
		{
			base.Add(key.Trim().ToLower(), value);
		}

		public bool Contains(string key)
		{
			return base.Contains(key.Trim().ToLower());
		}

		public bool ContainsKey(string key)
		{
			return base.ContainsKey(key.Trim().ToLower());
		}

		public object this[string key]
		{
			get {return base[key.Trim().ToLower()];}
		}
	}

	public class IntHashList : HashList {
		public IntHashList(){}

		public new IEnumerator GetEnumerator() {
			return base.GetEnumerator();
		}

		public void Add(int key, object value) {
			base.Add(key, value);
		}

		public bool Contains(int key) {
			return base.Contains(key);
		}

		public bool ContainsKey(int key) {
			return base.ContainsKey(key);
		}

		public object this[int key] {
			get {return base[key];}
		}
	}

}
