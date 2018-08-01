﻿namespace Reportr.Components.Collections
{
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    /// <summary>
    /// Represents the data for a single table grouping
    /// </summary>
    [JsonObject]
    [DataContract]
    public sealed class TableGrouping : IEnumerable<TableRow>
    {
        /// <summary>
        /// Constructs the table grouping with the details
        /// </summary>
        /// <param name="columns">The columns</param>
        /// <param name="rows">The rows</param>
        public TableGrouping
            (
                TableColumn[] columns,
                params TableRow[] rows
            )
        {
            SetData(columns, rows);
        }

        /// <summary>
        /// Constructs the table grouping with the details
        /// </summary>
        /// <param name="groupingValues">The grouping values</param>
        /// <param name="rows">The rows</param>
        public TableGrouping
            (
                Dictionary<TableColumn, object> groupingValues,
                params TableRow[] rows
            )
        {
            Validate.IsNotNull(groupingValues);
            Validate.IsNotNull(rows);
            
            this.GroupingValues = groupingValues;

            var columns = groupingValues.Select
            (
                pair => pair.Key
            );

            SetData(columns, rows);
        }
        
        /// <summary>
        /// Gets the grouping values by column
        /// </summary>
        public Dictionary<TableColumn, object> GroupingValues
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets an array of the rows in the grouping
        /// </summary>
        [DataMember]
        public TableRow[] Rows { get; private set; }

        /// <summary>
        /// Gets an array of row cells representing the grouping totals
        /// </summary>
        [DataMember]
        public TableCell[] Totals { get; private set; }

        /// <summary>
        /// Sets the table grouping data
        /// </summary>
        /// <param name="columns">The queries columns</param>
        /// <param name="rows">The rows generated by the table</param>
        private void SetData
            (
                IEnumerable<TableColumn> columns,
                params TableRow[] rows
            )
        {
            Validate.IsNotNull(columns);
            Validate.IsNotNull(rows);

            if (false == columns.Any())
            {
                throw new ArgumentException
                (
                    "At least one column is required to populate the table."
                );
            }

            foreach (var column in columns.ToList())
            {
                var name = column.Name;

                var matchCount = columns.Count
                (
                    s => s.Name.ToLower() == name.ToLower()
                );

                if (matchCount > 1)
                {
                    var message = "The column name '{0}' can only be used once.";

                    throw new ArgumentException
                    (
                        String.Format
                        (
                            message,
                            name
                        )
                    );
                }
            }

            ValidateRows
            (
                columns.ToArray(),
                rows
            );
            
            this.Rows = rows;
        }

        /// <summary>
        /// Validates a collection of rows against columns
        /// </summary>
        /// <param name="columns">The columns</param>
        /// <param name="rows">The rows to validate</param>
        private void ValidateRows
            (
                TableColumn[] columns,
                TableRow[] rows
            )
        {
            var columnCount = columns.Length;

            foreach (var row in rows)
            {
                var cellCount = row.Cells.Length;

                if (cellCount != columnCount)
                {
                    var message = "{0} cells were expected, but {1} were found.";

                    throw new InvalidOperationException
                    (
                        String.Format
                        (
                            message,
                            columnCount,
                            cellCount
                        )
                    );
                }

                // Ensure the column names match those in the cells
                foreach (var column in columns)
                {
                    var name = column.Name;

                    var matchFound = row.Cells.Any
                    (
                        c => c.Column.Name.ToLower() == name.ToLower()
                    );

                    if (false == matchFound)
                    {
                        var message = "No data was found for the column '{0}'.";

                        throw new InvalidOperationException
                        (
                            String.Format
                            (
                                message,
                                name
                            )
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Gets the row at the index specified
        /// </summary>
        /// <param name="index">The row index (zero based)</param>
        /// <returns>The matching table row</returns>
        public TableRow this[int index]
        {
            get
            {
                return this.Rows[index];
            }
        }

        /// <summary>
        /// Gets an enumerator for the collection of rows
        /// </summary>
        /// <returns>The enumerator</returns>
        public IEnumerator<TableRow> GetEnumerator()
        {
            return this.Rows.ToList().GetEnumerator();
        }

        /// <summary>
        /// Gets a generic enumerator for the collection of rows
        /// </summary>
        /// <returns>The generic enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}