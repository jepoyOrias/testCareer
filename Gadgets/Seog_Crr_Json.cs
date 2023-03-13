using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CareerCom.Cool.Gadgets
{
    public class Seog_Crr_Json
    {
        private Seog_Crr_Log objLog = new Seog_Crr_Log();

        public string SalSerializeObjectToJson(object obj, bool bolPreventException = false)
        {
            if (bolPreventException)
            {
                try
                {
                    return JsonConvert.SerializeObject(obj);
                }
                catch
                {
                    return "error";
                }
            }
            else
            {
                return JsonConvert.SerializeObject(obj);
            }
        }

        public string SalSerializeDataTableToJson(DataTable dtbData, HashSet<string> hshIncludeColumn = null, HashSet<string> hshExcludeColumn = null)
        {
            if (hshIncludeColumn == null && hshExcludeColumn == null)
            {
                return SalSerializeObjectToJson(dtbData);
            }
            else
            {
                return JsonConvert.SerializeObject(dtbData, new CustomDataTableConverter(hshIncludeColumn, hshExcludeColumn));
            }
        }

        public string SalSerializeDataSetToJson(DataSet dstData, HashSet<string> hshIncludeColumn = null, HashSet<string> hshExcludeColumn = null)
        {
            if (hshIncludeColumn == null && hshExcludeColumn == null)
            {
                return SalSerializeObjectToJson(dstData);
            }
            else
            {
                return JsonConvert.SerializeObject(dstData, new CustomDataSetConverter(hshIncludeColumn, hshExcludeColumn));
            }
        }

        #region JsonConverters

        public class CustomDataTableConverter : DataTableConverter
        {
            private HashSet<string> _hshIncludeColumn = null;
            private HashSet<string> _hshExcludeColumn = null;

            public CustomDataTableConverter(HashSet<string> hshIncludeColumn, HashSet<string> hshExcludeColumn)
            {
                _hshIncludeColumn = hshIncludeColumn;
                _hshExcludeColumn = hshExcludeColumn;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                DataTable table = (DataTable)value;
                DefaultContractResolver resolver = serializer.ContractResolver as DefaultContractResolver;

                writer.WriteStartArray();

                foreach (DataRow row in table.Rows)
                {
                    writer.WriteStartObject();
                    foreach (DataColumn column in row.Table.Columns)
                    {
                        if (_hshIncludeColumn != null && !_hshIncludeColumn.Contains(column.ColumnName))
                        {
                            continue;
                        }

                        if (_hshExcludeColumn != null && _hshExcludeColumn.Contains(column.ColumnName))
                        {
                            continue;
                        }

                        object columnValue = row[column];

                        if (serializer.NullValueHandling == NullValueHandling.Ignore && (columnValue == null || columnValue == DBNull.Value))
                        {
                            continue;
                        }

                        writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(column.ColumnName) : column.ColumnName);
                        serializer.Serialize(writer, columnValue);
                    }
                    writer.WriteEndObject();
                }

                writer.WriteEndArray();
            }
        }

        public class CustomDataSetConverter : DataSetConverter
        {
            private HashSet<string> _hshIncludeColumn = null;
            private HashSet<string> _hshExcludeColumn = null;

            public CustomDataSetConverter(HashSet<string> hshIncludeColumn, HashSet<string> hshExcludeColumn)
            {
                _hshIncludeColumn = hshIncludeColumn;
                _hshExcludeColumn = hshExcludeColumn;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                DataSet dataSet = (DataSet)value;
                DefaultContractResolver resolver = serializer.ContractResolver as DefaultContractResolver;

                CustomDataTableConverter converter = new CustomDataTableConverter(_hshIncludeColumn, _hshExcludeColumn);

                writer.WriteStartObject();

                foreach (DataTable table in dataSet.Tables)
                {
                    writer.WritePropertyName((resolver != null) ? resolver.GetResolvedPropertyName(table.TableName) : table.TableName);

                    converter.WriteJson(writer, table, serializer);
                }

                writer.WriteEndObject();
            }
        }

        #endregion

    }
}