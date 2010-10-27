using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace CancerGov.CDR.ClinicalTrials.Helpers
{

    public class CTLookupList : List<CTLookupListItem>
    {
        public CTLookupList(DataTable data)
        {
            LoadDataSource(data);
        }

        private void LoadDataSource(DataTable data)
        {
            foreach (DataRow dr in data.Rows)
            {
                this.Add(new CTLookupListItem(dr[0].ToString(), dr[1].ToString(), dr[2].ToString()));
            }
        }
    }
}
