using Domain.Entities;

namespace OasisAPI.Tests.DomainTests.Entities;

using Xunit;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_WhenCreated_ShouldHaveId()
    {
        // Arrange
        var entity = new BaseEntity();
        
        // Act
        var id = entity.Id;

        // Assert
        Assert.IsType<int>(id);
    }
    
    [Fact]
    public void BaseEntity_ShouldUpdateIdCorrectly()
    {
        // Arrange
        var entity = new BaseEntity();
        
        // Act
        entity.Id = 1;

        // Assert
        Assert.Equal(1, entity.Id);
    }
}