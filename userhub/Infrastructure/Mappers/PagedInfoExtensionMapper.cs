using Genesis.idlib.Infrastructure;
using userhub.Models;

namespace userhub.Infrastructure.Mappers
{
    public static class PagedInfoExtensionMapper
    {
        public static PagerModel MapToPagerModel(this PagedInfo pagedInfo, string baseUrl, int sortBy, string sortParam)
        {
            var retPagerModel = new PagerModel(baseUrl,
                                                pagedInfo.CurrentPage,
                                                sortBy,
                                                sortParam,
                                                pagedInfo.TotalPages);                      

            return retPagerModel;
        }
    }
}