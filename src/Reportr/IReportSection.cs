﻿namespace Reportr
{
    using Reportr.Components;

    /// <summary>
    /// Defines a contract for a single report section
    /// </summary>
    public interface IReportSection
    {
        /// <summary>
        /// Gets the sections name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the sections title
        /// </summary>
        string Title { get; }
        
        /// <summary>
        /// Gets the component associated with the section
        /// </summary>
        IReportComponent Component { get; }

        /// <summary>
        /// Gets an array of nested report sections
        /// </summary>
        IReportSection[] NestedSections { get; }
    }
}
