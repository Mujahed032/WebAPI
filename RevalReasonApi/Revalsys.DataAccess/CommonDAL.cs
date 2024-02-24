using System.Data;
using System.Reflection;

namespace Revalsys.DataAccess
{
    public class CommonDAL
    {
        internal AppDb _db;
        public int CommandTimeout
        {
            get { return _db._CommandTimeout; }
        }
        public CommonDAL(AppDb Db)
        {
            _db = Db;
        }
        #region ConvertToList
        public static List<ListDTO> ConvertToList<ListDTO>(List<DataRow> rows)
        {
            List<ListDTO>? lst = null;

            if (rows != null)
            {
                lst = new List<ListDTO>();
                foreach (DataRow row in rows)
                {
                    ListDTO item = CreateItem<ListDTO>(row);
                    lst.Add(item);
                }
            }
        #pragma warning disable CS8603 // Possible null reference return.
            return lst;
            #pragma warning restore CS8603 // Possible null reference return.
        }

           public static ListDTO CreateItem<ListDTO>(DataRow row)
              {
            ListDTO? obj = default;
            if (row != null)
            {
                obj = Activator.CreateInstance<ListDTO>();
                foreach (DataColumn column in row.Table.Columns)
                {
                #pragma warning disable CS8602 // Dereference of a possibly null reference.
                    FieldInfo? prop = obj.GetType().GetField(column.ColumnName);
                #pragma warning restore CS8602 // Dereference of a possibly null reference.
                    if (prop != null)
                    {

                        object value = row[column.ColumnName];
                #pragma warning disable CS8602 // Dereference of a possibly null reference.
                        if (value.ToString().Trim() != string.Empty)
                        {
                            prop.SetValue(obj, value);
                        }
                    #pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }
                    else
                    {
                        PropertyInfo? objprop = obj.GetType().GetProperty(column.ColumnName);
                        if (objprop != null)
                        {
                            object value = row[column.ColumnName];
                     #pragma warning disable CS8602 // Dereference of a possibly null reference.
                            if (value.ToString().Trim() != string.Empty)
                            {
                                objprop.SetValue(obj, value, null);
                            }
                      #pragma warning restore CS8602 // Dereference of a possibly null reference.
                        }
                    }
                }
            }
            #pragma warning disable CS8603 // Possible null reference return.
            return obj;
            #pragma warning restore CS8603 // Possible null reference return.
            }
        #endregion
        //*********************************************************************************************************
        //Purpose            :  This DAL Method is used to get the primary id by GUID so we can merge this while updating the ReasonTable and to update the table through primary Id
        //Layer	             :  DAL
        //Method Name        :	GetReason PrimaryId
        //Input Parameters   :  Id
        //Return Values      :  
        // --------------------------------------------------------------------------------------------------------
        //    Version            Author                 Date               Remarks       
        //  ------------------------------------------------------------------------------------------------------
        //    1.0	          Md Mujahed Ul Islam       17 Nov 2023        Creation
        //*********************************************************************************************************

        public int GetReasonPrimaryId(string strId)
        {

            using (var sqlCmd = _db.connection.CreateCommand())
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandTimeout = _db._CommandTimeout;
                sqlCmd.CommandText = "uspGetReasonPrimaryId";
                sqlCmd.Parameters.Add("@Id", SqlDbType.NVarChar).Value = strId;
                _db.connection.Open();
                var result = sqlCmd.ExecuteScalar();
                _db.connection.Close();
                if (result != null)
                {
                    return Convert.ToInt32(result);
                }
                else
                {
                    return 0;
                }
            }


        }
    }
}
