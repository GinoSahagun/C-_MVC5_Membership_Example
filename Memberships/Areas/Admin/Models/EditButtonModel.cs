using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Memberships.Areas.Admin.Models
{
    public class EditButtonModel
    {
        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public int SubscriptionId { get; set; }
        public string Link {
            get
            {
                var param = new StringBuilder("?");
                if (ItemId > 0) param.Append(String.Format("{0}={1}&", "itemId", ItemId));
                if (ProductId > 0) param.Append(String.Format("{0}={1}&", "productId", ItemId));
                if (SubscriptionId > 0) param.Append(String.Format("{0}={1}&", "subscriptionId", ItemId));

                return param.ToString().Substring(0, param.Length - 1);

            }
        }

    }
}