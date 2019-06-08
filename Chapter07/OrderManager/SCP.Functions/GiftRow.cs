using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace SCP.Functions
{
    public class GiftRow : TableEntity
    {
        public GiftRow()
        {
            
        }

        public GiftRow(string childId) : base("Gifts", Guid.NewGuid().ToString())
        {
            this.ChildId = childId;
        }

        public string ChildId { get; set; }
        public string GiftBrand { get; set; }
        public string GiftName { get; set; }
        public bool IsOrdered { get; set; }

        public override string ToString()
        {
            return $"{nameof(ChildId)}={ChildId}, {nameof(GiftBrand)}={GiftBrand}, {nameof(GiftName)}={GiftName}";
        }
    }
}
