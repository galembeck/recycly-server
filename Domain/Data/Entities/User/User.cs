using Domain.Data.Entities._Base;
using Domain.Data.Entities._Base.Extension;
using Domain.Enumerators;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Data.Entities;

[Table("TBUser")]
public class User : BaseEntity, IBaseEntity<User>
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    
    public DateOnly? BirthDate { get; set; }
    public List<string>? Phones { get; set; }



    public ProfileType ProfileType { get; set; }



    public string Password { get; set; } = string.Empty;



    public DateTimeOffset? LastAccessAt { get; set; }



    public string? PasswordChangeToken { get; set; }
    public DateTimeOffset? PasswordChangeTokenExpiresAt { get; set; }



    [NotMapped]
    public string HashId { get; set; } = string.Empty;



    #region .: METHODS :.

    public User WithoutRelations(User entity)
    {
        if (entity == null)
            return null!;

        var newEntity = new User()
        {
            Name = entity.Name,
            Email = entity.Email,
            Document = entity.Document,

            BirthDate = entity.BirthDate,
            Phones = entity.Phones,

            ProfileType = entity.ProfileType,

            Password = entity.Password,

            LastAccessAt = entity.LastAccessAt,
            
            PasswordChangeToken = entity.PasswordChangeToken,
            PasswordChangeTokenExpiresAt = entity.PasswordChangeTokenExpiresAt,
            
            HashId = entity.HashId,
        };

        newEntity.InitializeInstance(entity);

        return newEntity;
    }

    #endregion .: METHODS :.
}