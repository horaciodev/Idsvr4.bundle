using Genesis.idlib.Infrastructure;
using userhub.Models;

namespace userhub.Infrastructure.Mappers
{
    public static class PagedInfoExtensionMapper
    {
        public static PagerModel MapToPagerModel(this PagedInfo pagedInfo, string baseUrl, int sortBy, string sortParam)
        {
            var retPagerModel = new PagerModel();
            
            retPagerModel.BaseTargetUrl = baseUrl;
            retPagerModel.CurrentPage = pagedInfo.CurrentPage;
            retPagerModel.SortBy = sortBy;
            retPagerModel.SortParam = sortParam;
            retPagerModel.TotalPages = pagedInfo.TotalPages;

            return retPagerModel;
        }
    }
}