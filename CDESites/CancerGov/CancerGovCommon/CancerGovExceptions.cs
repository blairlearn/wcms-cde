using System;

namespace CancerGov.Exceptions {
 
	public class InvalidGuidException : System.Exception {
		public InvalidGuidException() : base(){}
		public InvalidGuidException(string message) : base(message){}
	}

	public class NullViewException : System.Exception {
		public NullViewException() : base(){}
		public NullViewException(string message) : base(message){}
	}

	public class InvalidViewIDException : System.Exception {
		public InvalidViewIDException() : base(){}
		public InvalidViewIDException(string message) : base(message){}
	}

	/// <summary>
	/// This is for SQLCommands timing out.
	/// </summary>
	public class SqlTimeoutException : System.Exception {
		public SqlTimeoutException() : base(){}
		public SqlTimeoutException(string message) : base(message){}
	}

	/// <summary>
	/// this is for when fetching a single protocol breaks
	/// </summary>
    [Obsolete("Use CancerGov.CDR.ClinicalTrials.Search.ProtocolFetchFailureException instead")]
	public class ProtocolFetchFailureException : System.Exception {
		public ProtocolFetchFailureException() : base(){}
		public ProtocolFetchFailureException(string message) : base(message){}
	}

	/// <summary>
	/// this is for when fetching a single protocol breaks
	/// </summary>
	public class ProtocolInvalidProtocolSearchIDException : System.Exception {
		public ProtocolInvalidProtocolSearchIDException() : base(){}
		public ProtocolInvalidProtocolSearchIDException(string message) : base(message){}
	}

	/// <summary>
	/// this is for fetching a protocol results in an empty table
	/// </summary>
    [Obsolete("Use CancerGov.CDR.ClinicalTrials.Search.ProtocolTableEmptyException instead")]
    public class ProtocolTableEmptyException : System.Exception
    {
		public ProtocolTableEmptyException() : base(){}
		public ProtocolTableEmptyException(string message) : base(message){}
	}

	/// <summary>
	/// this is for fetching a protocol results in too many or too little tables
	/// </summary>
    [Obsolete("Use CancerGov.CDR.ClinicalTrials.Search.ProtocolTableMiscountException instead")]
    public class ProtocolTableMiscountException : System.Exception
    {
		public ProtocolTableMiscountException() : base(){}
		public ProtocolTableMiscountException(string message) : base(message){}
	}

	/// <summary>
	/// this is for fetching a protocol results in too many or too little tables
	/// </summary>
	public class SearchCriteriaInvalidXmlException : System.Exception {
		public SearchCriteriaInvalidXmlException() : base(){}
		public SearchCriteriaInvalidXmlException(string message) : base(message){}
	}

	/// <summary>
	/// this is for fetching a protocol results in too many or too little tables
	/// </summary>
	public class SearchCriteriaFetchFailureException : System.Exception {
		public SearchCriteriaFetchFailureException() : base(){}
		public SearchCriteriaFetchFailureException(string message) : base(message){}
	}


}
