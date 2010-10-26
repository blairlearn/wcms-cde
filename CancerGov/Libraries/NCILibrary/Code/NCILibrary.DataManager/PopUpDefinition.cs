using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Data;
using CancerGov.UI;
namespace NCI.DataManager
{
    public class PopUpDefinition
    {
        public void test()
        {

        }
        public  ArrayList GetDefinition(string type, string param, string pdqVersion, string language)
        {
            ArrayList returnvalue = new ArrayList(3);
            //string version = (pdqVersion == PDQVersion.HealthProfessional) ? "Health Professional" : "Patient";
            string version = pdqVersion;
            SqlConnection dbh = new SqlConnection(ConfigurationSettings.AppSettings["CDRDbConnectionString"]);
            SqlCommand sth = new SqlCommand("usp_GetGlossaryDefinition", dbh);
            sth.CommandType = CommandType.StoredProcedure;

            if (type == "term")
            {
                SqlParameter param_term = new SqlParameter("@Term", SqlDbType.VarChar, 255);
                param_term.Value = param;
                sth.Parameters.Add(param_term);

                SqlParameter param_audience = new SqlParameter("@Audience", SqlDbType.VarChar, 50);
                param_audience.Value = version;
                sth.Parameters.Add(param_audience);

                SqlParameter param_language = new SqlParameter("@Language", SqlDbType.VarChar, 50);
                param_language.Value = language.ToString().ToUpper();
                sth.Parameters.Add(param_language);
            }
            else if (type == "id")
            {
                SqlParameter param_id = new SqlParameter("@ID", SqlDbType.VarChar, 50);
                param_id.Value = param;
                sth.Parameters.Add(param_id);

                SqlParameter param_audience = new SqlParameter("@Audience", SqlDbType.VarChar, 50);
                param_audience.Value = version;
                sth.Parameters.Add(param_audience);

                SqlParameter param_language = new SqlParameter("@Language", SqlDbType.VarChar, 50);
                param_language.Value = language.ToString().ToUpper();
                sth.Parameters.Add(param_language);
            }
            else
            {
                throw new Exception("Unknown type (" + type + ") in get_definition()");
            }

            dbh.Open();

            SqlDataReader rows = sth.ExecuteReader();
            if (rows.Read())
            {
                returnvalue.Add(rows[1]); // Name
                returnvalue.Add(rows[2]); // Pronounciation
                returnvalue.Add(rows[3]); // Definition
                returnvalue.Add(rows[4]); // MediaHtml
                rows.Close();
                dbh.Close();
            }
            else
            {
                rows.Close();
                dbh.Close();
                return null;
            }

            return returnvalue;
        }
    }
}
