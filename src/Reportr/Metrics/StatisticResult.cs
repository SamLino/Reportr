﻿namespace Reportr.Metrics
{
    using System;
    
    /// <summary>
    /// Represents the result of a single statistic
    /// </summary>
    public class StatisticResult : OutputResult
    {
        /// <summary>
        /// Constructs the statistic result with the details
        /// </summary>
        /// <param name="value">The value calculated</param>
        /// <param name="executionTime">The execution time in milliseconds</param>
        /// <param name="success">True, if the query executed successfully</param>
        /// <param name="errorMessage">The error message, if there was one</param>
        public StatisticResult
            (
                double value,
                int executionTime,
                bool success = true,
                string errorMessage = null
            )

            : base(executionTime, success, errorMessage)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the value generated by the statistic
        /// </summary>
        public double Value { get; private set; }

        /// <summary>
        /// Gets the lower range value
        /// </summary>
        public double? LowerRange { get; private set; }

        /// <summary>
        /// Gets the upper range value
        /// </summary>
        public double? UpperRange { get; private set; }

        /// <summary>
        /// Gets a flag indicating if the value has a range that it fits into
        /// </summary>
        public bool HasRange { get; private set; }

        /// <summary>
        /// Adds the range values to the statistic result
        /// </summary>
        /// <param name="lower">The lower range</param>
        /// <param name="upper">The upper range</param>
        /// <returns>The updated statistic result</returns>
        public StatisticResult WithRange
            (
                double? lower,
                double? upper
            )
        {
            if (lower == null && upper == null)
            {
                throw new InvalidOperationException
                (
                    "A lower or upper range value must be specified."
                );
            }

            this.LowerRange = lower;
            this.UpperRange = upper;
            this.HasRange = true;

            return this;
        }
    }
}
