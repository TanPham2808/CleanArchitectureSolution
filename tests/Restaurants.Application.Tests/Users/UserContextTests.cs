using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Contant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Restaurants.Application.Users.Tests
{
    public class UserContextTests
    {
        [Fact()]
        public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
        {
            // arrange
            // 1️ Tạo ngày sinh giả định
            var dateOfBirth = new DateOnly(1990, 1, 1);

            // 2️ Mock IHttpContextAccessor – dùng để giả lập HttpContext
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            // 3️ Tạo danh sách Claim (dữ liệu định danh của user)
            // Mỗi Claim mô phỏng một thuộc tính của user: Id, Email, Role, Quốc tịch, Ngày sinh
            var claims = new List<Claim>()
    {
        new(ClaimTypes.NameIdentifier, "1"),
        new(ClaimTypes.Email, "test@test.com"),
        new(ClaimTypes.Role, UserRoles.Admin),
        new(ClaimTypes.Role, UserRoles.User),
        new("Nationality", "German"),
        new("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
    };

            // 4️ Tạo ClaimsPrincipal – đại diện cho user đã đăng nhập
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

            // 5️ Gán user vào HttpContext giả lập
            httpContextAccessorMock.Setup(x => x.HttpContext)
                .Returns(new DefaultHttpContext() { User = user });

            // 6️⃣ Tạo instance của UserContext với HttpContext giả
            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            // 7️ Gọi hàm GetCurrentUser() – hành vi cần test
            var currentUser = userContext.GetCurrentUser();

            // assert
            // 8️ Kiểm tra giá trị trả về có đúng không
            currentUser.Should().NotBeNull();                            // Phải khác null
            currentUser.Id.Should().Be("1");                             // Id đúng
            currentUser.Email.Should().Be("test@test.com");               // Email đúng
            currentUser.Roles.Should().ContainInOrder(UserRoles.Admin, UserRoles.User); // Có đủ 2 role theo thứ tự
            currentUser.Nationality.Should().Be("German");                // Quốc tịch đúng
            currentUser.DateOfBirth.Should().Be(dateOfBirth);             // Ngày sinh đúng
        }

        [Fact]
        public void GetCurrentUser_WithUserContextNotPresent_ThrowsInvalidOperationException()
        {
            // Arrange
            // 1️ Mock IHttpContextAccessor nhưng không có HttpContext (null)
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext)null);

            // 2️ Tạo UserContext với mock ở trên
            var userContext = new UserContext(httpContextAccessorMock.Object);

            // act
            // 3️ Gọi hàm GetCurrentUser() và bắt hành vi lỗi
            Action action = () => userContext.GetCurrentUser();

            // assert
            // 4️ Xác nhận rằng hàm ném ra đúng loại exception và đúng thông điệp
            action.Should()
                .Throw<InvalidOperationException>()
                .WithMessage("User context is not present");
        }
    }
}