using Dotnet_Developer_Test_Assignment.models;
using Dotnet_Developer_Test_Assignment.Repositories.Implementations;
using Xunit;

namespace TestProject
{
    public class CountryTests
    {
        private readonly CountryRepository _repo;

        public CountryTests()
        {
            _repo = new CountryRepository();
        }

        [Fact]
        public void AddCountry_Should_Add_New_Country()
        {
            var country = new Country { Code = "EG", Name = "Egypt" };

            var result = _repo.Add(country);

            Assert.True(result);
            Assert.True(_repo.Exists("EG"));
        }

        [Fact]
        public void AddCountry_Should_Fail_When_Duplicate()
        {
            var country = new Country { Code = "EG", Name = "Egypt" };

            _repo.Add(country);
            var result = _repo.Add(country);

            Assert.False(result);
        }

        [Fact]
        public void RemoveCountry_Should_Work()
        {
            var country = new Country { Code = "US", Name = "USA" };
            _repo.Add(country);

            var result = _repo.Remove("US");

            Assert.True(result);
            Assert.False(_repo.Exists("US"));
        }

        [Fact]
        public void RemoveCountry_Should_Fail_When_Not_Exist()
        {
            var result = _repo.Remove("XX");

            Assert.False(result);
        }

        [Fact]
        public void GetAll_Should_Return_All_Countries()
        {
            _repo.Add(new Country { Code = "EG", Name = "Egypt" });
            _repo.Add(new Country { Code = "US", Name = "USA" });

            var countries = _repo.GetAll();

            Assert.Contains(countries, c => c.Code == "EG");
            Assert.Contains(countries, c => c.Code == "US");
        }
    }
}