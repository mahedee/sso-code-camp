using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Models
{
    public class SelectItem
    {
        public SelectItem(string id, string text)
        {
            Id = id;
            Text = text;
        }

        public string Id { get; set; }

        public string Text { get; set; }
    }
}
