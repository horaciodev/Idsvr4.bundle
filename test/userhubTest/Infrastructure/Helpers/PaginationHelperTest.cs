using System;
using Xunit;
using userhub.Infrastructure.Helpers;
using userhub.Models;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace userhubTest.Infrastructure.Helpers
{
    public class PaginationHelperTest
    {
        [Fact]
        public void B3Pager_Returns_Pager_As_IHtmlContent_Object()
        {
            //arrange
            var pagerModel = new PagerModel("http://localhost/",1,1,"SortBy",3);
            //act
            var result = PaginationHelper.B3Pager(pagerModel);
            //result
            Assert.NotNull(result);
            Assert.IsType<TagBuilder>(result);
        }
    }
}