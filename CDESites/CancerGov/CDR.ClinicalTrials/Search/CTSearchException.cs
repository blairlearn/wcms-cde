using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CancerGov.CDR.ClinicalTrials.Search
{
    /// <summary>
    /// Abstract base class for all clinical trial search exceptions.
    /// </summary>
    public abstract class CTSearchException : Exception
    {
        public CTSearchException() : base() { }
        public CTSearchException(String message) : base(message) { }
        public CTSearchException(String message, Exception innerException) : base(message, innerException) { }
    }

    /// <summary>
    /// This is for CTSearch being attempted with invalid parameters
    /// </summary>
    public class ProtocolSearchParameterException : CTSearchException
    {
        public ProtocolSearchParameterException() : base() { }
        public ProtocolSearchParameterException(string message) : base(message) { }
    }

    /// <summary>
    /// This is for CTSearching not returning a protocol search id.
    /// </summary>
    public class NullProtocolSearchIDException : CTSearchException
    {
        public NullProtocolSearchIDException() : base() { }
        public NullProtocolSearchIDException(string message) : base(message) { }
    }

    /// <summary>
    /// this is for protocol searches that fail while doing a protocol search
    /// </summary>
    public class ProtocolSearchExecutionFailureException : CTSearchException
    {
        public ProtocolSearchExecutionFailureException() : base() { }
        public ProtocolSearchExecutionFailureException(string message) : base(message) { }
    }

    /// <summary>
    /// This is for attempts to retrieve protocol searches when the search ID is invalid.
    /// </summary>
    public class ProtocolSearchIDNotValidException : CTSearchException
    {
        public ProtocolSearchIDNotValidException() : base() { }
        public ProtocolSearchIDNotValidException(string message) : base(message) { }
    }

    /// <summary>
    /// this is for when fetching protocols breaks
    /// </summary>
    public class ProtocolFetchFailureException : CTSearchException
    {
        public ProtocolFetchFailureException() : base() { }
        public ProtocolFetchFailureException(string message) : base(message) { }
    }

    /// <summary>
    /// this is for fetching a protocol results in an empty table
    /// </summary>
    public class ProtocolTableEmptyException : CTSearchException
    {
        public ProtocolTableEmptyException() : base() { }
        public ProtocolTableEmptyException(string message) : base(message) { }
    }

    /// <summary>
    /// this is for fetching a protocol results in too many or too little tables
    /// </summary>
    public class ProtocolTableMiscountException : CTSearchException
    {
        public ProtocolTableMiscountException() : base() { }
        public ProtocolTableMiscountException(string message) : base(message) { }
    }

    /// <summary>
    /// this is for fetching a protocol results in too many or too little tables
    /// </summary>
    public class ProtocolNullPrintCacheIDException : CTSearchException
    {
        public ProtocolNullPrintCacheIDException() : base() { }
        public ProtocolNullPrintCacheIDException(string message) : base(message) { }
    }

    /// <summary>
    /// this is for fetching a protocol results in too many or too little tables
    /// </summary>
    public class ProtocolPrintCacheFailureException : CTSearchException
    {
        public ProtocolPrintCacheFailureException() : base() { }
        public ProtocolPrintCacheFailureException(string message) : base(message) { }
    }
}
