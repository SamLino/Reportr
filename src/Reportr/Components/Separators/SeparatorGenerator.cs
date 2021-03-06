﻿namespace Reportr.Components.Separators
{
    using Reportr.Filtering;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a report component separator generator
    /// </summary>
    public class SeparatorGenerator : ReportComponentGeneratorBase
    {
        /// <summary>
        /// Asynchronously generates a component from a report definition and filter
        /// </summary>
        /// <param name="definition">The component definition</param>
        /// <param name="sectionType">The report section type</param>
        /// <param name="filter">The report filter</param>
        /// <returns>The report component generated</returns>
        public override Task<IReportComponent> GenerateAsync
            (
                IReportComponentDefinition definition,
                ReportSectionType sectionType,
                ReportFilter filter
            )
        {
            Validate.IsNotNull(definition);

            var seperator = new Separator
            (
                (SeparatorDefinition)definition
            );

            return Task.FromResult<IReportComponent>
            (
                seperator
            );
        }
    }
}
