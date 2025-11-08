using FluentAssertions;
using Restaurants.Domain.Contant;
using Xunit;


namespace Restaurants.Application.Users.Tests;

public class CurrentUserTests
{
    // Kiểm tra khi người dùng có đúng role cần kiểm tra
    // TestMethod_Scenario_ExpectedBehavior
    [Theory()]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMachingRole_ShouldReturnTrue(string roleName)
    {
        // arrange
        var currentUser = new CurrentUser(
            Id: "1",
            Email: "test@test.com",
            Roles: new[] { UserRoles.Admin, UserRoles.User },
            null,
            null);

        // act
        var isInRole = currentUser.IsInRole(roleName);

        // assert
        isInRole.Should().BeTrue();
    }

    // Kiểm tra khi người dùng không có role cần kiểm tra (Owner).
    [Fact()]
    public void IsInRole_WithNotMatchingRole_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser(
            Id: "1",
            Email: "test@test.com",
            Roles: new[] { UserRoles.Admin, UserRoles.User },
            null,
            null);

        // act
        var isInRole = currentUser.IsInRole(UserRoles.Owner);

        // assert
        isInRole.Should().BeFalse();
    }

    // Kiểm tra phân biệt chữ hoa – chữ thường
    [Fact()]
    public void IsInRole_WithNotMatchingRoleCase_ShouldReturnFalse()
    {
        // arrange
        var currentUser = new CurrentUser(
            Id: "1",
            Email: "test@test.com",
            Roles: new[] { UserRoles.Admin, UserRoles.User },
            null,
            null);

        // act
        var isInRole = currentUser.IsInRole(UserRoles.Admin.ToLower());

        // assert
        isInRole.Should().BeFalse();
    }
}
