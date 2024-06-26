using Domain;
using Domain.DTOs;
using TechChallenge.Tests.Entitiy;

namespace TechChallenge.Tests.Domain
{
    public class ContactTests
    {
        [Fact]
        public void Create_ShouldReturnContact_WhenContactDTOIsValid()
        {
            // Arrange
            var contactDto = ContactBuilder.Build();

            // Act
            var contact = Contact.Create(contactDto);

            // Assert
            Assert.NotNull(contact);
            Assert.Equal(int.Parse(contactDto.Phone[..2]), contact.DDDId);
            Assert.Empty(contact.ValidationResult.Errors);
            Assert.Equal(contactDto.Name, contact.Name);
            Assert.Equal(contactDto.Phone, string.Concat(contact.DDDId.ToString(), contact.Phone));
            Assert.Equal(contactDto.Email, contact.Email);
        }

        [Fact]
        public void Create_ShouldHaveValidationErrors_WhenEmailContactDTOIsInvalid()
        {
            // Arrange
            var contactDto = ContactBuilder.Build();
            contactDto.Email = contactDto.Email.Replace("@", "");

            // Act
            var contact = Contact.Create(contactDto);

            // Assert
            Assert.NotNull(contact);
            Assert.NotEmpty(contact.ValidationResult.Errors);
            Assert.True(contact.ValidationResult.Errors.Count() == 1);
            Assert.Equal("'Email' é um endereço de email inválido.", contact.ValidationResult.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Update_ShouldUpdateContactDetails()
        {
            // Arrange
            var contact = Contact.Create(ContactBuilder.Build());

            var contactUpdateDto = ContactBuilder.BuildUpdateDto();

            // Act
            contact.Update(contactUpdateDto);

            // Assert
            Assert.Equal(contactUpdateDto.Name, contact.Name);
            Assert.Equal(contactUpdateDto.Phone[2..], contact.Phone);
            Assert.Equal(contactUpdateDto.Email, contact.Email);
            Assert.Equal(int.Parse(contactUpdateDto.Phone[..2]), contact.DDDId);
        }

        [Fact]
        public void Create_ShouldHaveValidationErrors_WhenPhoneContactDTOIsInvalid()
        {
            // Arrange
            var contactDto = ContactBuilder.Build();
            contactDto.Phone = contactDto.Phone[3..];
            // Act
            var contact = Contact.Create(contactDto);

            // Assert
            Assert.NotNull(contact);
            Assert.NotEmpty(contact.ValidationResult.Errors);
            Assert.Equal("'Phone' number must contain 8 or 9 digits.", contact.ValidationResult.Errors[0].ErrorMessage);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenPhoneIsInvalid()
        {
            // Arrange            
            var contact = Contact.Create(ContactBuilder.WrongBuild());

            // Act & Assert            
            Assert.True(contact.ValidationResult.Errors.Count() == 1);
            Assert.Contains(contact.ValidationResult.Errors, e => e.ErrorMessage == "'Phone' number must contain 8 or 9 digits.");            
        }

        [Fact]
        public void Create_ShouldHaveValidationErrors_WhenDDDIsInvalid()
        {
            // Arrange
            var contactDto = ContactBuilder.WrongBuildDDD();
            // Act
            var contact = Contact.Create(contactDto);

            // Assert
            Assert.NotNull(contact);
            Assert.NotEmpty(contact.ValidationResult.Errors);
            Assert.Contains(contact.ValidationResult.Errors, e => e.PropertyName == "DDDId" && e.ErrorMessage == "'DDD Id' deve ser informado.");
        }
    }
}
