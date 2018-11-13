using System;
using GeekBudget.Core.Helpers;
using Xunit;

namespace GeekBudget.Core.Tests
{
    public class UpdateHelpersTests
    {
        [Fact]
        public void MapNewValues_UpdatesSelectedFields()
        {
            // Arrange
            var target = new ComplexObject
            {
                String = "before",
                Integer = 1,
                Decimal = 10m,
                DateTime = new DateTime(2018, 1, 23)
            };

            var source = new ComplexObject
            {
                String = "after",
                Integer = 2,
                Decimal = 20m,
                DateTime = new DateTime(2018, 2, 24)
            };
            
            // Act
            target.MapNewValues(source,
                x => x.String,
                x => x.Integer,
                x => x.Decimal,
                x => x.DateTime);
            
            // Assert
            Assert.Equal("after", target.String);
            Assert.Equal(2, target.Integer);
            Assert.Equal(20m, target.Decimal);
            Assert.Equal(new DateTime(2018, 2, 24), target.DateTime);
        }
        
        [Fact]
        public void MapNewValues_UpdatesOnlySelectedFields()
        {
            // Arrange
            var target = new ComplexObject
            {
                String = "before",
                Integer = 1,
                Decimal = 10m,
                DateTime = new DateTime(2018, 1, 23)
            };

            var source = new ComplexObject
            {
                String = "after",
                Integer = 2,
                Decimal = 20m,
                DateTime = new DateTime(2018, 2, 24)
            };
            
            // Act
            target.MapNewValues(source,
                x => x.String,
                x => x.DateTime);
            
            // Assert
            Assert.Equal("after", target.String);
            Assert.Equal(new DateTime(2018, 2, 24), target.DateTime);
        }
        
        [Fact]
        public void MapNewValues_DoesNotUpdateEmptyFields()
        {
            // Arrange
            var target = new ComplexObject
            {
                String = "before",
                Integer = 1,
                Decimal = 10m,
                DateTime = new DateTime(2018, 1, 23)
            };

            var source = new ComplexObject
            {
                String = "after",
                DateTime = new DateTime(2018, 2, 24)
            };
            
            // Act
            target.MapNewValues(source,
                x => x.String,
                x => x.DateTime);
            
            // Assert
            Assert.Equal("after", target.String);
            Assert.Equal(1, target.Integer);
            Assert.Equal(10m, target.Decimal);
            Assert.Equal(new DateTime(2018, 2, 24), target.DateTime);
        }

        [Fact]
        public void MapNewValues_DifferentObject_MapsFields()
        {
            // Arrange
            var target = new ComplexObject
            {
                String = "before",
                Integer = 1,
                Decimal = 10m,
                DateTime = new DateTime(2018, 1, 23)
            };

            var source = new AnotherComplexObject
            {
                String = "after",
                NullableInteger = 2,
                NullableDecimal = 20m,
                DateTime = new DateTime(2018, 2, 24)
            };

            // Act
            target.MapNewValues(source,
                (x => x.String, y => y.String),
                (x => x.Integer, y => y.NullableInteger),
                (x => x.Decimal, y => y.NullableDecimal),
                (x => x.DateTime, y => y.DateTime)
            );

            // Assert
            Assert.Equal("after", target.String);
            Assert.Equal(2, target.Integer);
            Assert.Equal(20m, target.Decimal);
            Assert.Equal(new DateTime(2018, 2, 24), target.DateTime);
        }

        [Fact]
        public void MapNewValues_DifferentObject_DoNotCastNullable()
        {
            // Arrange
            var target = new ComplexObject
            {
                String = "before",
                Integer = 1,
                Decimal = 10m
            };

            var source = new AnotherComplexObject
            {
                String = "after",
                NullableInteger = null,
                NullableDecimal = null
            };

            // Act
            target.MapNewValues(source,
                (x => x.String, y => y.String),
                (x => x.Integer, y => y.NullableInteger),
                (x => x.Decimal, y => y.NullableDecimal)
                );

            // Assert
            Assert.Equal("after", target.String);
            Assert.Equal(1, target.Integer);
            Assert.Equal(10m, target.Decimal);
        }
    }

    internal class ComplexObject
    {
        public string String { get; set; }
        public int Integer{ get; set; }
        public decimal Decimal { get; set; }
        public DateTime DateTime { get; set; }
    }

    internal class AnotherComplexObject
    {
        public string String { get; set; }
        public int? NullableInteger { get; set; }
        public decimal? NullableDecimal { get; set; }
        public DateTime DateTime { get; set; }
    }
}