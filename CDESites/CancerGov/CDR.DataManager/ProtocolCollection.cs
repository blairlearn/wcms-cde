using System;
using System.Configuration;
using System.Collections;

using CancerGov.Text;
using CancerGov.Exceptions;

namespace CancerGov.CDR.DataManager
{
	/// <summary>
	/// This retrieves protocols by either SearchID or List of Protocol IDs
	/// </summary>
	public class ProtocolCollection : ICollection
	{

		private ArrayList alProtocols;
		private int iTotalResults = 0;
		private int iProtocolSearchID = -1;

		#region Properties
		public int TotalResults {
			get {return iTotalResults;}
			set {iTotalResults = value;}
		}

		public int ProtocolSearchID {
			get {return iProtocolSearchID;}
			set {iProtocolSearchID = value;}
		}

		public int Count {
			get {return alProtocols.Count;}
		}

		public Protocol this[int index] {
			get{return (Protocol)alProtocols[index];}
		}

		public bool IsSynchronized {
			get {return alProtocols.IsSynchronized;}
		}

		public object SyncRoot {
			get {return alProtocols.SyncRoot;}
		}
		#endregion

		#region Constructor

		public ProtocolCollection() {
			alProtocols = new ArrayList();
		}

		public ProtocolCollection(int protocolSearchID) {
			alProtocols = new ArrayList();
			this.iProtocolSearchID = protocolSearchID;
		}

		#endregion


		#region Methods
		public void CopyTo(Array array, int index) {
			alProtocols.CopyTo(array, index);
		}
		
		public void Add(Protocol p) {
			alProtocols.Add(p);
		}

		public IEnumerator GetEnumerator() {
			return alProtocols.GetEnumerator();
		}
		#endregion


	}

}
