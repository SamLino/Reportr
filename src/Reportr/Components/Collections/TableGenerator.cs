﻿namespace Reportr.Components.Collections
{
    using Reportr.Data;
    using Reportr.Filtering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a report table generator
    /// </summary>
    public class TableGenerator : ReportComponentGeneratorBase
    {
        /// <summary>
        /// Asynchronously generates a component from a report definition and filter
        /// </summary>
        /// <param name="definition">The component definition</param>
        /// <param name="sectionType">The report section type</param>
        /// <param name="filter">The report filter</param>
        /// <returns>The report component generated</returns>
        public override async Task<IReportComponent> GenerateAsync
            (
                IReportComponentDefinition definition,
                ReportSectionType sectionType,
                ReportFilter filter
            )
        {
            Validate.IsNotNull(definition);
            Validate.IsNotNull(filter);

            var tableDefinition = definition.As<TableDefinition>();
            var query = tableDefinition.Query;
            var defaultParameters = tableDefinition.DefaultParameterValues;

            var parameters = filter.GetQueryParameters
            (
                query,
                defaultParameters.ToArray()
            );

            var results = await query.ExecuteAsync
            (
                parameters.ToArray()
            );

            var tableRows = new List<TableRow>();
            var actionDefinition = tableDefinition.RowAction;

            foreach (var queryRow in results.AllRows)
            {
                var tableCells = new List<TableCell>();

                foreach (var columnDefinition in tableDefinition.Columns)
                {
                    var value = columnDefinition.Binding.Resolve
                    (
                        queryRow
                    );

                    var action = default(ReportAction);

                    if (columnDefinition.CellAction != null)
                    {
                        action = columnDefinition.CellAction.Resolve
                        (
                            queryRow
                        );
                    }

                    var column = new TableColumn
                    (
                        columnDefinition.Name,
                        columnDefinition.Title,
                        columnDefinition.Alignment,
                        columnDefinition.Importance
                    );

                    var cell = new TableCell
                    (
                        column,
                        value,
                        action
                    );

                    tableCells.Add(cell);
                }
                
                var tableRow = new TableRow
                (
                    tableCells.ToArray()
                );

                if (tableDefinition.RowAction != null)
                {
                    var action = actionDefinition.Resolve
                    (
                        queryRow
                    );

                    tableRow = tableRow.WithAction
                    (
                        action
                    );
                }

                tableRows.Add(tableRow);
            }

            if (false == tableDefinition.DisableSorting)
            {
                var sortingRules = filter.GetSortingRules
                (
                    sectionType,
                    definition.Name
                );

                tableRows = SortRows(tableRows, sortingRules).ToList();
            }

            var repeater = new Table
            (
                tableDefinition,
                tableRows.ToArray()
            );

            return repeater;
        }

        /// <summary>
        /// Sorts a collection of table rows by the sorting rules specified
        /// </summary>
        /// <param name="rows">The table rows</param>
        /// <param name="sortingRules">The sorting rules</param>
        /// <returns>The sorted rows</returns>
        private IEnumerable<TableRow> SortRows
            (
                IEnumerable<TableRow> rows,
                IEnumerable<ReportFilterSortingRule> sortingRules
            )
        {
            Validate.IsNotNull(rows);
            Validate.IsNotNull(sortingRules);

            if (false == sortingRules.Any())
            {
                return rows;
            }
            else
            {
                var ruleNumber = 1;
                var sortedRows = (IOrderedEnumerable<TableRow>)rows;

                foreach (var rule in sortingRules)
                {
                    object keySelector(TableRow row) => row.First
                    (
                        cell => cell.Column.Name.ToLower() == rule.ColumnName.ToLower()
                    )
                    .Value;

                    if (rule.Direction == SortDirection.Ascending)
                    {
                        if (ruleNumber == 1)
                        {
                            sortedRows = sortedRows.OrderBy
                            (
                                keySelector
                            );
                        }
                        else
                        {
                            sortedRows = sortedRows.ThenBy
                            (
                                keySelector
                            );
                        }
                    }
                    else
                    {
                        if (ruleNumber == 1)
                        {
                            sortedRows = sortedRows.OrderByDescending
                            (
                                keySelector
                            );
                        }
                        else
                        {
                            sortedRows = sortedRows.ThenByDescending
                            (
                                keySelector
                            );
                        }
                    }

                    ruleNumber++;
                }

                return rows;
            }
        }
    }
}
