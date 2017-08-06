using System;
using Xunit;

using userhub.Models;

namespace userhubTest.Models
{
    public class PagerModelTest
    {
        [Fact]
        public void PagerModel_Ctor_Creates_Object_Successfully()
        {
            //arrange
            
            //act
            var pagerModel = new PagerModel("http://someurl.com",1,1,"SortBy",5);
            //assert
            Assert.NotNull(pagerModel);
            Assert.IsType<PagerModel>(pagerModel);
        }

        [Fact]
        public void PagerModel_PrevPageEnabled_Returs_False_When_CurrentPage_Is_First()
        {
            //arrange
            
            //act
            var pagerModel = new PagerModel("http://someurl.com",1,1,"SortBy",5);
            //assert
            Assert.False(pagerModel.PrevPageEnabled);
        }

        [Fact]
        public void PagerModel_PrevPageEnabled_Returns_True_When_CurrentPage_Is_Greater_Than_1()
        {
            //arrange
            
            //act
            var pagerModel = new PagerModel("http://someurl.com",2,1,"SortBy",5);
            //assert 
            Assert.True(pagerModel.PrevPageEnabled);
        }

        [Fact]
        public void PagerModel_NextPageEnabled_Returns_False_When_CurrentPage_Is_Last_One()
        {
            //arrange
            
            //act
            var pagerModel = new PagerModel("http://someurl.com",5,1,"SortBy",5);
            //assert
            Assert.False(pagerModel.NextPageEnabled);
        }

        [Fact]
        public void PagerModel_NextPageEnabled_Returns_True_When_CurrentPage_IsNot_Last_One()
        {
            //arrange

            //act
            var pagerModel = new PagerModel("http://someurl.com",4,1,"SortBy",5);
            //assert
            Assert.True(pagerModel.NextPageEnabled);
        }

        [Fact]
        public void PagerModel_MaxDisplayPages_Returns_TotalPages_When_TotalPages_Is_Less_Than_MaxDisplayPAges()
        {
            //arrange
            var expected = 3;
            //act
            var pagerModel = new PagerModel("http://someurl.com",1,1,"SortBy",expected);
            //assert
            Assert.Equal(expected, pagerModel.MaxDisplayPages);
        }

        [Fact]
        public void PagerModel_MaxDisplayPages_Returns_Correct_Number_When_TotalPages_Is_Greater_Or_Equal_Than_MaxDisplayPages()
        {
            //arrange
            var expected = 5;
            //act
            var pagerModel = new PagerModel("http://someurl.com",1,1,"SortBy",7);
            //assert
            Assert.Equal(expected, pagerModel.MaxDisplayPages);
        }

        [Fact]
        public void PagerModel_UpperLimit_Returns_Max_Number_Displayed_In_Pager_Button_Given_CurrentPage_Is_First()
        {
            //arrange
            var expected = 5;
            //act
            var pagerModel = new PagerModel("http://someurl.com",1,1,"SortBy",7);
            //assert
            Assert.Equal(expected, pagerModel.UpperLimit);
        }

        [Fact]
        public void PagerModel_UpperLimit_Returns_Max_Number_Displayed_In_Pager_Button_Given_CurrentPage_Equals_MaxDisplayPages_Plus_One()
        {
            //arrange
            var expected = 6;
            //act
            var pagerModel = new PagerModel("http://someurl.com",6,1,"SortBy",7);            
            //assert
            //Console.WriteLine($"StartPagerAtPage:{pagerModel.StartPagerAtPage}");
            Assert.Equal(expected,pagerModel.UpperLimit);
        }

        //TODO: use a theories here
        [Fact]
        public void PagerModel_StartPagerAtPage_Returns_1_When_CurrentPage_Less_Than_MaxDisplayPages()
        {
            //arrange
            var expected = 1;
            //act
            var pagerModel = new PagerModel("http://someurl.com",5,1,"SortBy",7);            
            //assert
            Assert.Equal(expected, pagerModel.StartPagerAtPage);
        }

        [Fact]
        public void PagerModel_StartAtPage_Returns_CorrectNumber_When_CurrentPage_Is_Greater_Than_MaxdisplayPages()
        {
            //arrange
            var expected = 8;
            //act
            var pagerModel = new PagerModel("http://someurl.com",12,1,"SortBy",15);            
            //assert
            Assert.Equal(expected, pagerModel.StartPagerAtPage);

        }

    }

}