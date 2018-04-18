﻿namespace Reportr.Data.Querying
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    /// <summary>
    /// Represents the results of the execution of a query
    /// </summary>
    public class QueryResults : IEnumerable<QueryRow>
    {
        /// <summary>
        /// Constructs the query results with the details
        /// </summary>
        /// <param name="query">The query that was executed</param>
        /// <param name="executionTime">The execution time in milliseconds</param>
        /// <param name="success">True, if the query executed successfully</param>
        public QueryResults
            (
                IQuery query,
                int executionTime,
                bool success = true
            )
        {
            Validate.IsNotNull(query);

            this.QueryName = query.Name;
            this.ExecutionTime = executionTime;
            this.Success = success;
            this.Columns = new QueryColumnInfo[] { };
            this.Rows = new QueryRow[] { };
        }
        
        /// <summary>
        /// Gets the name of the query that generated the results
        /// </summary>
        public string QueryName { get; private set; }

        /// <summary>
        /// Gets the queries execution time
        /// </summary>
        public int ExecutionTime { get; private set; }

        /// <summary>
        /// Gets a flag indicating if the queries ran successfully
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        /// Adds the error messages to the query results
        /// </summary>
        /// <param name="errors">The error messages to add</param>
        /// <returns>The updated query results</returns>
        public QueryResults WithErrors
            (
                IDictionary<string, string> errors
            )
        {
            Validate.IsNotNull(errors);

            this.ErrorMessages = new ReadOnlyDictionary<string, string>
            (
                errors
            );

            return this;
        }

        /// <summary>
        /// Gets any error messages that were generated
        /// </summary>
        /// <remarks>
        /// The error messages are grouped by error code.
        /// </remarks>
        public IDictionary<string, string> ErrorMessages { get; private set; }
        
        /// <summary>
        /// Gets an array of the columns from the query
        /// </summary>
        public QueryColumnInfo[] Columns { get; private set; }

        /// <summary>
        /// Gets an array of the rows in the result
        /// </summary>
        public QueryRow[] Rows { get; private set; }

        /// <summary>
        /// Adds the query data to the result
        /// </summary>
        /// <param name="columns">The queries columns</param>
        /// <param name="rows">Th rows generated by the query</param>
        /// <returns>The updated query result</returns>
        public QueryResults WithData
            (
                IEnumerable<QueryColumnInfo> columns,
                params QueryRow[] rows
            )
        {
            Validate.IsNotNull(columns);
            Validate.IsNotNull(rows);

            if (false == columns.Any())
            {
                throw new ArgumentException
                (
                    "At least one column is required to populate the query."
                );
            }

            foreach (var set in columns.ToList())
            {
                var name = set.Column.Name;

                var matchCount = columns.Count
                (
                    s => s.Column.Name.Trim().ToLower() == name.Trim().ToLower()
                );

                if (matchCount > 1)
                {
                    var message = "The column name '{0}' can only be used once.";

                    throw new ArgumentException
                    (
                        String.Format(message, name)
                    );
                }
            }

            ValidateRows
            (
                columns.ToArray(),
                rows
            );

            this.Columns = columns.ToArray();
            this.Rows = rows;

            return this;
        }

        /// <summary>
        /// Validates a collection of rows against columns
        /// </summary>
        /// <param name="columns">The columns</param>
        /// <param name="rows">The rows to validate</param>
        private void ValidateRows
            (
                QueryColumnInfo[] columns,
                QueryRow[] rows
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
                foreach (var info in columns)
                {
                    var matchFound = row.Cells.Any
                    (
                        c => c.Column.Name.ToLower() == info.Column.Name.ToLower()
                    );

                    if (false == matchFound)
                    {
                        var message = "No data was found for the column '{0}'.";

                        throw new InvalidOperationException
                        (
                            String.Format
                            (
                                message,
                                info.Column.Name
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
        /// <returns>The matching query row</returns>
        public QueryRow this[int index]
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
        public IEnumerator<QueryRow> GetEnumerator()
        {
            return this.Rows.ToList().GetEnumerator();
        }

        /// <summary>
        /// Gets a generic enumerator for the collection of rows
        /// </summary>
        /// <returns>The generic enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
