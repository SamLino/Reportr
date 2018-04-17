﻿namespace Reportr.Data.Querying
{
    using Reportr.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a contract for a single query
    /// </summary>
    public interface IQuery
    {
        /// <summary>
        /// Gets the name of the query
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the data source being used by the query
        /// </summary>
        IDataSource DataSource { get; }

        /// <summary>
        /// Gets an array of the columns generated by the query
        /// </summary>
        QueryColumnInfo[] Columns { get; }

        /// <summary>
        /// Gets an array of sorting rules for the query
        /// </summary>
        QuerySortingRule[] SortingRules { get; }

        /// <summary>
        /// Specifies a sorting rule against a column in the query
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="direction">The sort direction</param>
        void SortColumn
        (
            string columnName,
            SortDirection direction
        );

        /// <summary>
        /// Gets an array of grouping rules for the query
        /// </summary>
        QueryGroupingRule[] GroupingRules { get; }

        /// <summary>
        /// Adds a grouping rule to the query
        /// </summary>
        /// <param name="columnName">The column name</param>
        /// <param name="direction">The sort direction</param>
        void AddGrouping
        (
            string columnName,
            SortDirection direction
        );

        /// <summary>
        /// Gets an array of parameters accepted by the component
        /// </summary>
        ParameterInfo[] Parameters { get; }

        /// <summary>
        /// Executes the component using the parameter values supplied
        /// </summary>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The query results</returns>
        QueryResults Execute
        (
            params ParameterValue[] parameterValues
        );

        /// <summary>
        /// Asynchronously executes the component using the parameter values supplied
        /// </summary>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The query results</returns>
        Task<QueryResults> ExecuteAsync
        (
            params ParameterValue[] parameterValues
        );
    }
}
