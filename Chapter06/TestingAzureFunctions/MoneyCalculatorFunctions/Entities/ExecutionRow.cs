using System;
using System.Collections.Generic;
using System.Text;

namespace MoneyCalculatorFunctions.Entities
{
    public class ExecutionRow
    {
        public ExecutionRow(DateTime calculationTimestamp)
        {
            this.CalculationTimestamp = calculationTimestamp;
            this.PartitionKey = this.CalculationTimestamp.ToString("yyyyMMdd");
            this.RowKey = Guid.NewGuid().ToString();
        }

        public string PartitionKey { get; private set; }

        public string RowKey { get; private set; }

        public decimal MortgageLoan { get; set; }

        public DateTime CalculationTimestamp { get; private set; }

        public uint NumberOfPayments { get; set; }

        public double AnnualInterest { get; set; }

        public decimal? MonthlyRate { get; set; }

        public bool Result { get; set; }

        public string ErrorCode { get; set; }
    }
}
