using System.Data;
using System.Dynamic;
using System.Reflection;

namespace MauiApp1.Components;

public static class DataTableToListConverter
{
    public static IEnumerable<ExpandoObject> ToExpandoObject(this DataTable dt)
    {
        List<ExpandoObject> list = new();
        foreach (DataRow row in dt.Rows)
        {
            ExpandoObject expandoObject = new();
            foreach (DataColumn col in dt.Columns)
            {
                if (col.DataType.Name.Equals("DateTime"))
                {
                    expandoObject.TryAdd(col.ColumnName,
                        row[col.ColumnName] is DBNull ? null : row.ItemArray[col.Ordinal]);
                }
                else
                {
                    expandoObject.TryAdd(col.ColumnName, row.ItemArray[col.Ordinal]);
                }
            }

            list.Add(expandoObject);
        }

        return list;
    }
}

public static class ListExtensions
{
    public static IEnumerable<T> MatchWithAnyProperty<T, TK>(this IEnumerable<T> list, TK value)
    {
        var argType = typeof(TK);
        var properties = typeof(T).GetProperties().Where(x => x.PropertyType.IsAssignableFrom(argType));
        var a = list.Cast<ExpandoObject>().Where(x => x.Any(y =>
            y.Value != null && y.Value.ToString()!
                .Contains(Convert.ToString(value)!.ToLower(), StringComparison.OrdinalIgnoreCase))).Select(z => z);
        var b = (IEnumerable<T>)a;
        return b;
    }
}

public static class ListToDataTableConverter
{
    public static DataTable ToDataTable<T>(this List<T> items)
    {
        DataTable dataTable = new("Table");
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var prop in props)
        {
            dataTable.Columns.Add(prop.Name);
        }

        foreach (var item in items)
        {
            var values = new object[props.Length];
            for (var i = 0; i < props.Length; i++)
            {
                values[i] = props[i].GetValue(item, null);
            }

            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

    public static DataTable ToDataTable<T>(this List<T> items, List<string> excludeColumnName)
    {
        DataTable dataTable = new(typeof(T).Name);

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(a => a.PropertyType != typeof(object) && !excludeColumnName.Contains(a.Name))
            .Select(a => new
            {
                Property = a,
                Type = Nullable.GetUnderlyingType(a.PropertyType) ?? a.PropertyType
            }).ToArray();

        foreach (var prop in props)
        {
            dataTable.Columns.Add(prop.Property.Name, prop.Type);
        }

        foreach (var item in items)
        {
            var values = new object[props.Length];

            for (var i = 0; i < props.Length; i++)
            {
                var value = props[i].Property.GetValue(item, null);
                if (props[i].Type == typeof(DateTime))
                {
                    value = ((DateTime)value!).ToString("dd-MMM-yyyy");
                }

                values[i] = value;
            }

            dataTable.Rows.Add(values);
        }

        return dataTable;
    }

    public static DataTable ToDataTable(this IEnumerable<ExpandoObject> objects)
    {
        var dt = new DataTable("Table1");

        var enumerable = objects.ToList();
        var properties = new HashSet<string>(enumerable.SelectMany(o => ((IDictionary<string, object>)o).Keys));

        foreach (var prop in properties)
        {
            dt.Columns.Add(prop);
        }

        var accessors = properties.ToDictionary(p => p, p => new Func<ExpandoObject, object>(o =>
        {
            ((IDictionary<string, object>)o).TryGetValue(p, out var value);
            return value;
        }));

        // Add rows to DataTable
        foreach (var obj in enumerable)
        {
            var row = dt.NewRow();

            foreach (var prop in properties)
            {
                row[prop] = accessors[prop](obj);
            }

            dt.Rows.Add(row);
        }

        return dt;
    }

    public static DataTable ToDataTable(this IEnumerable<ExpandoObject> objects, IEnumerable<string> columnNames,
        bool includeColumns = true)
    {
        var dt = new DataTable("Table1");
        var enumerable = objects.ToList();
        var properties = new HashSet<string>(enumerable.SelectMany(o => ((IDictionary<string, object>)o).Keys));
        if (includeColumns)
        {
            properties.IntersectWith(columnNames);
        }
        else
        {
            properties.ExceptWith(columnNames);
        }
        foreach (var prop in properties)
        {
            dt.Columns.Add(prop);
        }
        var accessors = properties.ToDictionary(p => p, p => new Func<ExpandoObject, object>(o =>
        {
            ((IDictionary<string, object>)o).TryGetValue(p, out var value);
            return value;
        }));
        foreach (var obj in enumerable)
        {
            var row = dt.NewRow();

            foreach (var prop in properties)
            {
                row[prop] = accessors[prop](obj);
            }

            dt.Rows.Add(row);
        }
        return dt;
    }
}