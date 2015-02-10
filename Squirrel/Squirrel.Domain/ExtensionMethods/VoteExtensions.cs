using System.Collections.Generic;
using System.Linq;
using Squirrel.Domain.Enititis;

namespace Squirrel.Domain.ExtensionMethods
{
    public static class VoteExtensions
    {
        public static int Summery(this ICollection<Vote> votes)
        {
            if (votes == null || !votes.Any())
            {
                return 0;
            }

            return votes.Aggregate(0, (current, vote) => vote.Plus ? current + 1 : current - 1);
        }
    }
}
