using System;
using System.Collections.Generic;

using Xunit;

using userhub.Infrastructure.Extensions;
using userhub.Models;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace userhubTest.Infrastructure.Extensions
{
    public class SelectListConverterTest
    {
        [Fact]
        public void ConvertList_Returns_List_Of_SelectedListItem_Given_An_Input_Of_Generic_IList()
        {
            //arrange
            var myList = new List<CompanyVM>(){
                new CompanyVM{ CompanyId = 1, CompanyName = "Company 1" },
                new CompanyVM{ CompanyId = 2, CompanyName = "Company Two" },
            };
            //act
            var result = myList.ConvertList("CompanyId","CompanyName");
            //assert
            Assert.NotNull(result);
            Assert.IsType<List<SelectListItem>>(result);
            Assert.Equal(2,result.Count);
        }
    }
}