using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using OrderManager.Core.Entities;

namespace OrderManager.Functions.Extensions
{
    public static class CloudTableExtensions
    {

        public static async Task<Order> GetOrderByIdAsync(this CloudTable table, string orderId)
        {
            Order child = null;

            TableQuery<Order> rangeQuery = new TableQuery<Order>().Where(
                TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal,
                    orderId));

            var token = default(TableContinuationToken);

            var query = await table.ExecuteQuerySegmentedAsync(rangeQuery, token);

            child = query.FirstOrDefault();

            return child;
        }

        public static async Task<bool> InsertAsync(this CloudTable table, TableEntity entity)
        {
            TableOperation operation = TableOperation.Insert(entity);
            var result = await table.ExecuteAsync(operation);

            return result.HttpStatusCode >= 200 && result.HttpStatusCode <= 299;
        }

        public static async Task<bool> UpdateAsync(this CloudTable table, TableEntity entity)
        {
            TableOperation operation = TableOperation.Replace(entity);
            var result = await table.ExecuteAsync(operation);

            return result.HttpStatusCode >= 200 && result.HttpStatusCode <= 299;
        }

        public static async Task<bool> InsertOrReplaceAsync(this CloudTable table, TableEntity entity)
        {
            TableOperation operation = TableOperation.InsertOrReplace(entity);
            var result = await table.ExecuteAsync(operation);

            return result.HttpStatusCode >= 200 && result.HttpStatusCode <= 299;
        }

    }
}
