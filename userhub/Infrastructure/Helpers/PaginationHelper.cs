using userhub.Models;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Text;

namespace userhub.Infrastructure.Helpers
{
    public static class PaginationHelper
    {
      
        public static IHtmlContent B3Pager(PagerModel pagerModel)
        {
            var builder = new TagBuilder("nav");
            builder.MergeAttribute("aria-label","Page Navigation");

            var ulBuilder = new TagBuilder("ul");
            ulBuilder.AddCssClass("pagination");
           
            ulBuilder.InnerHtml.AppendHtml(B3PagerNavigator(pagerModel.CurrentPage-1,
                                pagerModel.TotalPages, 
                                pagerModel.SortBy, 
                                pagerModel.BaseTargetUrl, 
                                pagerModel.SortParam,
                                PagerNavigationType.Previous,
                                pagerModel.PrevPageEnabled));
            
            for(int i=pagerModel.StartPagerAtPage;i<= pagerModel.UpperLimit; i++)
            {                  
                                
                ulBuilder.InnerHtml.AppendHtml(B3Page(i,pagerModel.BaseTargetUrl,pagerModel.SortBy, pagerModel.SortParam,
                                                     pagerModel.CurrentPage == i));
            }

            ulBuilder.InnerHtml.AppendHtml(B3PagerNavigator(pagerModel.CurrentPage+1,
            pagerModel.TotalPages, 
            pagerModel.SortBy,
            pagerModel.BaseTargetUrl ,
            pagerModel.SortParam,
            PagerNavigationType.Next,
            pagerModel.NextPageEnabled));

            builder.InnerHtml.AppendHtml(ulBuilder);

            return builder;
        }

        private static IHtmlContent B3PagerNavigator(int pageNum,
                                                    int toTalPages,
                                                    int sortBy,
                                                    string baseUrl,
                                                    string sortParam,
                                                    PagerNavigationType navigationType,
                                                    bool enabled)
        {
            string pageNavText = string.Empty;
            string pageNavLabel = string.Empty;


            if(navigationType == PagerNavigationType.Next)
            {
                pageNavText = "&raquo;";
                pageNavLabel = "Next";
            }
            else
            {
               pageNavText = "&laquo";
               pageNavLabel = "Previous";
            }

            var pageNavBuilder = new TagBuilder("li");
            var pageNavAnchorTag = new TagBuilder("a");
            if(!enabled)            
                pageNavBuilder.AddCssClass("disabled");
            
            var pageNavigationTargetUrl = PageUrlBuilder(pageNum,baseUrl,sortBy,sortParam);
            pageNavAnchorTag.MergeAttribute("aria-label",pageNavLabel);
            pageNavAnchorTag.MergeAttribute("href",pageNavigationTargetUrl);
            var pageNavSpanTag = new TagBuilder("span");
            pageNavSpanTag.MergeAttribute("aria-hidden","true");
            pageNavSpanTag.InnerHtml.AppendHtml(pageNavText);
            if(enabled)
            {
            pageNavAnchorTag.InnerHtml.AppendHtml(pageNavSpanTag);
            pageNavBuilder.InnerHtml.AppendHtml(pageNavAnchorTag);
            }
            else
            {
                pageNavBuilder.InnerHtml.AppendHtml(pageNavSpanTag);
            }

            return pageNavBuilder;
        }

        private static IHtmlContent B3Page(int pageNum,string baseUrl,int sortBy, string sortParam, bool isCurrentPage)
        {
            TagBuilder pageBuilder = new TagBuilder("li");
            TagBuilder anchorBuilder = new TagBuilder("a");
            if(isCurrentPage)
                pageBuilder.AddCssClass("active");
            var pageUrl = PageUrlBuilder(pageNum, baseUrl, sortBy, sortParam);
            anchorBuilder.MergeAttribute("href",pageUrl);
            anchorBuilder.InnerHtml.Append(pageNum.ToString());
            pageBuilder.InnerHtml.AppendHtml(anchorBuilder);

            return pageBuilder;            
        }

        private static string PageUrlBuilder(int pageNum,string baseUrl,int sortBy, string sortParam)
        {
            var targetUrlBuilder = new StringBuilder(baseUrl)
                                        .Append("?pageNum")
                                        .Append("=").Append(pageNum.ToString())
                                        .Append("&").Append(sortParam).Append("=").Append(sortBy);

            return targetUrlBuilder.ToString();                                            
        }
        
    }
}