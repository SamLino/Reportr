﻿namespace Reportr.Components.Metrics
{
    using Reportr.Data.Querying;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// Represents the definition of a single chart
    /// </summary>
    public class ChartDefinition : ReportComponentDefinitionBase
    {
        /// <summary>
        /// Constructs the chart definition with the details
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="title">The title</param>
        public ChartDefinition
            (
                string name,
                string title
            )
            : base(name, title)
        {
            this.DataSets = new Collection<ChartDataSetDefinition>();
        }
        
        /// <summary>
        /// Gets a collection of chart data set definitions
        /// </summary>
        public ICollection<ChartDataSetDefinition> DataSets
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets an array of x-axis label template
        /// </summary>
        public ChartAxisLabel XAxisLabelTemplate { get; protected set; }

        /// <summary>
        /// Gets an array of y-axis label template
        /// </summary>
        public ChartAxisLabel YAxisLabelTemplate { get; protected set; }

        /// <summary>
        /// Adds the label templates to the chart definition
        /// </summary>
        /// <param name="xAxisLabelTemplate">The x-axis label template</param>
        /// <param name="yAxisLabelTemplate">The y-axis label template</param>
        /// <returns>The updated chart definition</returns>
        public ChartDefinition WithLabelTemplates
            (
                ChartAxisLabel xAxisLabelTemplate,
                ChartAxisLabel yAxisLabelTemplate
            )
        {
            this.XAxisLabelTemplate = xAxisLabelTemplate;
            this.YAxisLabelTemplate = yAxisLabelTemplate;

            return this;
        }

        /// <summary>
        /// Gets a collection of all queries being used by the component
        /// </summary>
        /// <returns>A collection of queries</returns>
        public override IEnumerable<IQuery> GetQueriesUsed()
        {
            foreach (var set in this.DataSets)
            {
                yield return set.Query;
            }
        }

        /// <summary>
        /// Gets the component type
        /// </summary>
        public override ReportComponentType ComponentType
        {
            get
            {
                return ReportComponentType.Chart;
            }
        }
    }
}
