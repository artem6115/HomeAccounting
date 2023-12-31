using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public static class QueryExtensions
    {
        public static IQueryable<IHasUser> WhereIsUser(this IQueryable<IHasUser> query) 
        {
            return query.Where(x => x.UserId == UserContext.UserId);
        }
    }
}
