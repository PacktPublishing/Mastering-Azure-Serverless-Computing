using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCP.Functions;

namespace Microsoft.WindowsAzure.Storage.Table
{
    public static class CloudTableExtensions
    {

        public static async Task<ChildRow> GetChildByIdAsync(this CloudTable table, string childId)
        {
            ChildRow child = null;

            TableQuery<ChildRow> rangeQuery = new TableQuery<ChildRow>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal,
                        "Children"),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal,
                        childId)));

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


        public static async Task<IEnumerable<GiftRow>> GetGiftsToOrderAsync(this CloudTable table)
        {
            List<GiftRow> giftsToOrder = new List<GiftRow>();

            TableQuery<GiftRow> rangeQuery = new TableQuery<GiftRow>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Gifts"),
                    TableOperators.And,
                    TableQuery.GenerateFilterConditionForBool("IsOrdered", QueryComparisons.Equal, false)));

            TableContinuationToken token = null;

            do
            {
                var query = await table.ExecuteQuerySegmentedAsync(rangeQuery, token);
                token = query.ContinuationToken;
                giftsToOrder.AddRange(query);

            } while (token != null );
        
            return giftsToOrder;
        }

        public static async Task OrderGiftsAsync(this CloudTable table, IEnumerable<GiftRow> giftsToOrder)
        {
            if (giftsToOrder.Any())
            {
                TableBatchOperation batchOperation = new TableBatchOperation();
                foreach (var item in giftsToOrder)
                {
                    batchOperation.InsertOrReplace(item);
                }
                await table.ExecuteBatchAsync(batchOperation);
            }
        }
    }
}
