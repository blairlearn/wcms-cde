using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using CancerGov.UI;
using CancerGov.Common.ErrorHandling;

using NCI.Util;

namespace CancerGov.CDR.DrugDictionary
{
    /// <summary>
    /// This Class is the business layer that interfaces between the user interface and
    /// the database layer. 
    /// </summary>
    public static class DrugDictionaryManager
    {
        /// <summary>
        /// This methods filters the information passed to it in order to refine the query
        /// that will be called in the database layer.
        /// </summary>
        /// <param name="criteria">The partial text used to query the database</param>
        /// <param name="maxRows">The maximum number of rows that the database will return. a value of zero will return the entire set</param>
        /// <param name="contains">indicator as to whether the text is to be searched starting from the beginning or anywhere
        ///                        in the string</param>
        /// <returns>Returns the search results</returns>
        public static DrugDictionaryCollection Search(string criteria, int maxRows, int curPage, bool bOtherNames, bool contains)
        {
            DrugDictionaryCollection dc = new DrugDictionaryCollection();

            // Find out how we should search for the string
            if (Strings.Clean(criteria) != null)
            {
                // replace any '[' with '[[]'
                //criteria = criteria.Replace("[", "[[]");

                // put the '%' at the end to indicate that the search starts
                // with the criteria passed.
                criteria += "%";

                // put the '%' at the beginning to indicate that the search
                // data contains the criteria passed
                if (contains)
                    criteria = "%" + criteria;

                // The stored procedure needs to change to reflect a 0 number of
                // rows to mean the entire set.
                if (maxRows <= 0)
                {
                    maxRows = 9999;
                }

                // The stored procedure need to reflect a cur pag of 0 to be 1
                if (curPage == 0)
                {
                    curPage = 1;
                }

                try
                {
                    int queryCount;

                    int matchCount = 0;

                    // Call the database layer and get data
                    DataTable dt = DrugDictionaryQuery.Search(
                            criteria,
                            maxRows,
                            curPage,
                            (bOtherNames ? 'Y' : 'N'),
                            out matchCount
                    );

                    // Set the value on the collection
                    dc.matchCount = matchCount;

                    // Now get a new collection with only one unique termIDs in the collection
                    if (bOtherNames)
                    {
                        concatenateDuplicateEntries(dt, dc);
                    }
                    else
                    {
                        // use Linq to move information from the dataTable
                        // into the DrugDictionaryCollection
                        dc.AddRange(
                            from entry in dt.AsEnumerable()
                            select GetEntryFromDR(entry)
                        );
                    }

                }
                catch (Exception ex)
                {
                    CancerGovError.LogError("DrugDictionatyManager", 2, ex);
                    throw ex;
                }
            }

            // return the collection
            return dc;
        }


        /// <summary>
        /// This methods filters the information passed to it in order to refine the query
        /// that will be called in the database layer.
        /// </summary>
        /// <param name="criteria">The partial text used to query the database</param>
        /// <param name="maxRows">The maximum number of rows that the database will return. a value of zero will return the entire set</param>
        /// <param name="contains">indicator as to whether the text is to be searched starting from the beginning or anywhere
        ///                        in the string</param>
        /// <returns>Returns the search results</returns>
        public static DrugDictionaryCollection SearchNameOnly(string criteria, int maxRows, bool contains)
        {
            DrugDictionaryCollection dc = new DrugDictionaryCollection();

            // Find out how we should search for the string
            if (Strings.Clean(criteria) != null)
            {
                // put the '%' at the end to indicate that the search starts
                // with the criteria passed.
                criteria += "%";

                // put the '%' at the beginning to indicate that the search
                // data contains the criteria passed
                if (contains)
                    criteria = "%" + criteria;

                // The stored procedure needs to change to reflect a 0 number of
                // rows to mean the entire set.
                if (maxRows <= 0)
                {
                    maxRows = 9999;
                }

                try
                {
                    // Call the database layer and get data
                    DataTable dt = DrugDictionaryQuery.SearchNameOnly(criteria, maxRows);

                    // use Linq to move information from the dataTable
                    // into the DrugDictionaryCollection
                    dc.AddRange(
                        from entry in dt.AsEnumerable()
                        select GetEntryFromDR(entry)
                    );

                }
                catch (Exception ex)
                {
                    CancerGovError.LogError("DrugDictionatyManager", 2, ex);
                    throw ex;
                }
            }

            // return the collection
            return dc;
        }

        /// <summary>
        /// This method calls the database layer for a single item and returns
        /// the drugDictionaryDataItem
        /// </summary>
        /// <param name="termID"></param>
        /// <returns></returns>
        public static DrugDictionaryDataItem GetDefinitionByTermID(int termID)
        {
            DrugDictionaryDataItem di = null;

            try
            {
                // Call the database layer and get data
                DataSet ds =
                    DrugDictionaryQuery.GetDefinitionByTermID(termID);

                // Only do something if we have data
                if (ds.Tables.Count > 0)
                {

                    // Get the entry record
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        dr["TermID"] = termID;

                        // build the data item
                        di = GetEntryFromDR(ds.Tables[0].Rows[0]);

                        // Get the display names
                        if (ds.Tables.Count > 1)
                            GetDisplayNames(di, ds.Tables[1]);
                    }
                    else if (ds.Tables[0].Rows.Count > 0)
                    {
                        throw new Exception("GetDefinitionByTermID returned more than 1 record.");
                    }
                }
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("DrugDictionaryManager", 2, ex);
                throw ex;
            }

            return di;
        }

        /// <summary>
        /// This method is a public call
        /// It calls the database query and uses the preferred name to get the previous 
        /// and next term names.
        /// </summary>
        /// <param name="di"></param>
        /// <param name="nRows"></param>
        public static void GetTermNeighbors(DrugDictionaryDataItem di, int nRows)
        {
            try
            {
                // Call the database layer and get data
                DataTable dt = DrugDictionaryQuery.GetTermNeighbors(
                        di.PreferredName, nRows);

                // use Linq to move information from the dataTable
                // into the DrugDictionaryDataItem
                di.PreviousNeighbors.AddRange(
                    from entry in dt.AsEnumerable()
                    where entry["isPrevious"].ToString() == "Y"
                    select GetEntryFromDR(entry)
                );

                // use Linq to move information from the dataTable
                // into the DrugDictionaryDataItem
                di.NextNeighbors.AddRange(
                    from entry in dt.AsEnumerable()
                    where entry["isPrevious"].ToString() == "N"
                    select GetEntryFromDR(entry)
                );
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("DrugDictionatyManager", 2, ex);
                throw ex;
            }
        }

        /// <summary>
        /// This method is a public call for use by the GetDefinitionByTermID method
        /// It takes the information from the second table in the call to usp_GetTermDefinition
        /// This builds the DisplayName dictionary list
        /// </summary>
        /// <param name="di"></param>
        /// <param name="dt"></param>
        private static void GetDisplayNames(DrugDictionaryDataItem di, DataTable dt)
        {
            try
            {
                // Initialize our vars
                string lastDisplayName = string.Empty;
                List<string> listOfNames = null;

                // We will not use Linq since this is a more complex list
                foreach (DataRow dr in dt.Rows)
                {
                    if (!lastDisplayName.Equals(dr["displayName"].ToString()))
                    {
                        // Save anything that we might have
                        if (!lastDisplayName.Equals(string.Empty))
                        {
                            di.DisplayNames.Add(lastDisplayName, listOfNames);
                        }

                        // Get a new list of names
                        lastDisplayName = dr["displayName"].ToString();
                        listOfNames = new List<string>();
                    }

                    // Add the name to the list
                    listOfNames.Add(dr["otherName"].ToString());
                }

                // See if we need to add anything to the dictionary
                if (!lastDisplayName.Equals(string.Empty))
                {
                    di.DisplayNames.Add(lastDisplayName, listOfNames);
                }
            }
            catch (Exception ex)
            {
                CancerGovError.LogError("DrugDictionatyManager", 2, ex);
                throw ex;
            }
        }

        /// <summary>
        /// This method takes the collection of Datarows and combines all the "otherName" fields into
        /// a single field for the same termID, creating one DataItem.
        /// </summary>
        /// <param name="dt">The DataTable that contains the rows</param>
        /// <param name="dc">The Collection object we will add the Data Items</param>
        private static void concatenateDuplicateEntries(DataTable dt, DrugDictionaryCollection dc)
        {
            if (dt.Rows.Count > 0)
            {
                // Get the first row
                DataRow dr = dt.Rows[0];

                // Set our new names to an empty string
                string newNames = string.Empty;

                // Walk the data table
                foreach (DataRow aDR in dt.Rows)
                {
                    // Get the value from the data row so we don't have to always look it up
                    var otherName = aDR["OtherName"];

                    // If we have a match concatenate the otherName
                    if ((int)dr["TermID"] == (int)aDR["TermID"])
                    {
                        // Only do this if the otherName has a value
                        if (!DBNull.Value.Equals(otherName) && !string.IsNullOrEmpty(otherName.ToString()))
                        {
                            // Attach the next other Name to this one
                            if (!string.IsNullOrEmpty(newNames))
                                newNames += "; ";

                            // concatenate the new 'other name'
                            newNames += otherName.ToString();
                        }
                    }
                    else
                    {
                        // Now create a new data item and put it into the new collection
                        dr["OtherName"] = newNames;
                        dc.Add(GetEntryFromDR(dr));

                        // Set the other data row
                        dr = aDR;

                        // Get the first other name
                        newNames = (DBNull.Value.Equals(otherName) ? string.Empty : otherName.ToString());
                    }
                }

                // Get the last one
                // Now create a new data item and put it into the new collection
                dr["OtherName"] = newNames;
                dc.Add(GetEntryFromDR(dr));
            }
        }

        /// <summary>
        /// Extracts information from a DataRow and builds a new
        /// DrugDictionaryDataItem
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>DrugDictionaryDataItem</returns>
        private static DrugDictionaryDataItem GetEntryFromDR(DataRow dr)
        {
            // return the new object
            DrugDictionaryDataItem ddi = new DrugDictionaryDataItem(
                Strings.ToInt(dr["TermID"]),
                Strings.Clean(dr["PreferredName"]),
                Strings.Clean(dr["OtherName"]),
                Strings.Clean(dr["DefinitionHTML"]),
                Strings.Clean(dr["PrettyURL"])
            );

            // return the created object
            return ddi;
        }

    }
}
