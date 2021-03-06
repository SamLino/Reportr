﻿namespace Reportr.Data.Querying.Functions
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a query aggregate function that calculates the median
    /// </summary>
    public class MedianFunction : AggregateFunctionBase
    {
        /// <summary>
        /// Constructs the function without a data binding
        /// </summary>
        /// <param name="autoRoundResult">True, to auto round the result</param>
        public MedianFunction
            (
                bool autoRoundResult = false
            )
            : base(autoRoundResult)
        { }

        /// <summary>
        /// Constructs the function with a data binding
        /// </summary>
        /// <param name="binding">The data binding</param>
        /// <param name="autoRoundResult">True, to auto round the result</param>
        public MedianFunction
            (
                DataBinding binding,
                bool autoRoundResult = false
            )
            : base(binding, autoRoundResult)
        { }

        /// <summary>
        /// Computes the function value from the multiple values specified
        /// </summary>
        /// <param name="numbers">The numbers to use in the calculation</param>
        /// <returns>The computed value</returns>
        protected override double Compute
            (
                List<double> numbers
            )
        {
            var halfIndex = numbers.Count / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            var median = default(double);

            if ((numbers.Count % 2) == 0)
            {
                median =
                (
                    (
                        sortedNumbers.ElementAt(halfIndex) +
                        sortedNumbers.ElementAt((halfIndex - 1))
                    )
                    / 2
                );
            }
            else
            {
                median = sortedNumbers.ElementAt
                (
                    halfIndex
                );
            }

            return median;
        }
    }
}
