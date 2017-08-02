using System.Collections.Generic;

namespace userhub.Models
{
    public class CompositeUserPagedModel
    {
        public IList<UserRowViewModel> UserRowList { get; set; }

        public PagerModel PaginationInfo { get; set; }
    }

}