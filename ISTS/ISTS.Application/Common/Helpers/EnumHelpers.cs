using ISTS.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTS.Application.Common.Helpers
{
    public class EnumHelpers
    {
        /// <summary>
        /// Convert a SelectList from ENUM
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<SelectItem> ToSelectList<T>() where T : struct, IComparable
        {
            var selectItems = Enum.GetValues(typeof(T))
                .Cast<T>()
                .Select(x => new SelectItem(Convert.ToInt16(x).ToString(), x.ToString())).ToList();

            return selectItems;
        }
    }
}
