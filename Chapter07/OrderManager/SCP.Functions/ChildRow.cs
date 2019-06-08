using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace SCP.Functions
{
    public class ChildRow : TableEntity
    {
        public ChildRow()
        {

        }

        public ChildRow(string childId) : base("Children", childId)
        {
            this.ChildId = childId;
        }
        public string ChildId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Goodness { get; set; }
        public override string ToString()
        {
            return $"{nameof(ChildId)}={ChildId}, {nameof(FirstName)}={FirstName}, {nameof(LastName)}={LastName}, {nameof(Goodness)}={Goodness}";
        }
    }
}
