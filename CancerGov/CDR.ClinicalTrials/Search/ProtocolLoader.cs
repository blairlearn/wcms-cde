using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using CancerGov.CDR.DataManager;

namespace CancerGov.CDR.ClinicalTrials.Search
{
    /// <summary>
    /// Provides methods for loading protocol data into business objects.
    /// 
    /// This class draws heavily upon the logic in CancerGov.CDR.DataManager.SqlProtocolListAdapter
    /// and CancerGov.CDR.DataManager.SqlProtocolAdapter.
    /// </summary>
    static class ProtocolLoader
    {
        public static ProtocolCollection BuildProtocolCollection(DataSet protocolData, int protocolSearchID,
            ProtocolVersions audience, string sectionList)
        {
            if (protocolData == null)
                throw new ArgumentNullException("protocolData");

            bool bStudySitesExist = false;
            bool bSectionsExist = false;
            bool bProtocolsExist = false;

            //Check to see what tables exist so we do not get nulls
            //while we are checking on tables, lets setup some relationships...
            switch (protocolData.Tables.Count)
            {
                case 3:
                    {
                        bStudySitesExist = true;
                        bSectionsExist = true;
                        bProtocolsExist = true;

                        DataRelation drProtocolToSections = new DataRelation("ProtocolToSections", protocolData.Tables[0].Columns["ProtocolID"], protocolData.Tables[1].Columns["ProtocolID"]);
                        protocolData.Relations.Add(drProtocolToSections);

                        break;
                    }
                case 2:
                    {
                        bSectionsExist = true;
                        bProtocolsExist = true;

                        DataRelation drProtocolToSections = new DataRelation("ProtocolToSections", protocolData.Tables[0].Columns["ProtocolID"], protocolData.Tables[1].Columns["ProtocolID"]);
                        protocolData.Relations.Add(drProtocolToSections);

                        break;
                    }
                case 1:
                    {
                        bProtocolsExist = true;
                        break;
                    }
                default:
                    {
                        throw new ProtocolTableMiscountException(protocolData.Tables.Count.ToString());
                    }
            }

            ProtocolCollection collection = new ProtocolCollection();

            Protocol protocol;
            foreach (DataRow row in protocolData.Tables[0].Rows)
            {
                protocol = GetProtocol(protocolData, row, bStudySitesExist, bSectionsExist, bProtocolsExist, protocolSearchID, audience, sectionList);
                collection.Add(protocol);
            }

            return collection;
        }

        /// <summary>
        /// Creates a protocol from its datarow representation.
        /// </summary>
        /// <param name="drProtocol">A Datarow from the protocol table</param>
        /// <returns>Protocol</returns>
        static private Protocol GetProtocol(DataSet protocolData, DataRow protocolRow,
            bool bStudySitesExist, bool bSectionsExist, bool bProtocolsExist, int protocolSearchID,
            ProtocolVersions audience, string sectionList)
        {
            DataRow[] drarrSections = null;
            DataView dvStudySites = null;

            if (bSectionsExist)
            {
                drarrSections = protocolRow.GetChildRows("ProtocolToSections");
            }

            if (bStudySitesExist)
            {
                //Setup StudySites dataview
                dvStudySites = new DataView(protocolData.Tables[2]);
                dvStudySites.RowFilter = "ProtocolID = " + protocolRow["ProtocolID"].ToString();
                dvStudySites.Sort = "Country, State, City, OrganizationName asc";
            }

            Protocol pProtocol = null;

            //Fix this
            if (protocolSearchID > 0)
            {
                if (bSectionsExist && bStudySitesExist)
                {
                    pProtocol = new Protocol(protocolSearchID, protocolRow, drarrSections, dvStudySites, audience, sectionList);
                }
                else if (bSectionsExist)
                {
                    pProtocol = new Protocol(protocolSearchID, protocolRow, drarrSections, audience, sectionList);
                }
                else
                {
                    pProtocol = new Protocol(protocolSearchID, protocolRow, audience, sectionList);
                }
            }

            return pProtocol;

        }

    }
}
