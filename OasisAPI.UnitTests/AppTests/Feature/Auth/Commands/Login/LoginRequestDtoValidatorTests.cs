using Moq;
using OasisAPI.App.Features.Auth.Commands.Login;
using OasisAPI.Infra.Repositories;
using Xunit;
using Domain.Entities;
using FluentValidation.TestHelper;

public class LoginRequestDtoValidatorTests
{
    private readonly LoginRequestDtoValidator _validator;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IRepository<OasisUser>> _userRepositoryMock;

    public LoginRequestDtoValidatorTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _userRepositoryMock = new Mock<IRepository<OasisUser>>();

        _unitOfWorkMock.Setup(u => u.GetRepository<OasisUser>())
            .Returns(_userRepositoryMock.Object);
        
        _validator = new LoginRequestDtoValidator(_unitOfWorkMock.Object);
    }

    [Fact]
    public void Should_Have_Error_When_User_Does_Not_Exist()
    {
        var dto = new LoginRequestDto
        {
            Email = "teste@dominio.com",
            Password = "password123",
        };
        
        var result = _validator.TestValidate(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("User does not exist");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var dto = new LoginRequestDto
        {
            Email = "teste@",
            Password = "password123",
        };
        
        var result = _validator.TestValidate(dto);
        
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Invalid email address");
    }

    [Fact]
    void Should_Have_Error_When_Password_Is_Too_Short()
    {
        var dto = new LoginRequestDto
        {
            Email = "teste@example.com",
            Password = "123",
        };
        
        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}