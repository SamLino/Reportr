﻿namespace Reportr
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines a contract for a single report component
    /// </summary>
    public interface IReportComponent
    {
        /// <summary>
        /// Gets the name of the component
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets an array of parameters accepted by the component
        /// </summary>
        ParameterInfo[] Parameters { get; }

        /// <summary>
        /// Executes the component using the parameter values supplied
        /// </summary>
        /// <param name="parameterValues">The parameter values</param>
        /// <returns>The output generated</returns>
        IReportComponentOutput Execute
        (
            Dictionary<string, object> parameterValues
        );
    }
}
