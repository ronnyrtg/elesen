using System;
using System.Collections.Generic;
using System.Linq;
using DataTables.Mvc;

namespace TradingLicense.Web.Helpers
{
    public static class StringHelper
    {
        public static string GetOrderByString(this IOrderedEnumerable<Column> sortedColumns)
        {
            // Sorting
            var orderByString = String.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != String.Empty ? "," : String.Empty;
                orderByString += (column.Data) +
                                 (column.SortDirection ==
                                  Column.OrderDirection.Ascendant ? " asc" : " desc");
            }

            return orderByString;
        }

        public static List<int> ToIntList(this string initial)
        {
            if (string.IsNullOrEmpty(initial))
            {
                return new List<int>();
            }

            string[] ids = initial.Split(',');
            return ids.Select(x => Convert.ToInt32(x)).ToList();
        }
    }
}